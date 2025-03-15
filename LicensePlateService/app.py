import base64
import cv2
import numpy as np
import re
import os
import sys
import logging
import threading
import webbrowser
import time
import ctypes
import winreg
import socket
from flask import Flask, request, jsonify, send_from_directory
from werkzeug.utils import secure_filename
from models.paddle_ocr_det_onnx import PaddleDetOnnx
from models.paddle_ocr_rec_onnx import PaddleCrnnOnnx
import config as cfg
from box_post_processing import merge_box_horizontal, merge_box_vertical, sort_polygon

# Import thư viện cho system tray
import pystray
from PIL import Image, ImageDraw

# Cấu hình logging cơ bản
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s - %(levelname)s - %(message)s",
    handlers=[logging.StreamHandler(sys.stdout)]
)

logger = logging.getLogger("license_plate_app")

# Đảm bảo đường dẫn tương đối hoạt động khi đóng gói với PyInstaller
if getattr(sys, 'frozen', False):
    # Nếu ứng dụng được đóng gói
    application_path = os.path.dirname(sys.executable)
    os.chdir(application_path)
    static_folder = os.path.join(application_path, "static")
else:
    # Nếu ứng dụng chạy từ script
    application_path = os.path.dirname(os.path.abspath(__file__))
    static_folder = os.path.join(application_path, "static")

logger.info(f"Đường dẫn ứng dụng: {application_path}")
logger.info(f"Thư mục static: {static_folder}")

# Khởi tạo Flask app
app = Flask(__name__, static_folder=static_folder)

# Biến toàn cục để lưu trạng thái server
server_running = False
server_thread = None
server_url = "http://localhost:1002"
SERVER_PORT = 1002

# Tên ứng dụng
APP_NAME = "LicensePlateOCR"
# Đường dẫn đến file thực thi
EXECUTABLE_PATH = os.path.join(application_path, "LicensePlateOCR.exe") if getattr(sys, 'frozen', False) else sys.executable
# Đường dẫn đến file cấu hình
CONFIG_FILE = os.path.join(application_path, "config.ini")

# Hàm kiểm tra xem ứng dụng đã đang chạy hay chưa
def is_app_already_running():
    try:
        # Tạo socket để kiểm tra cổng
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        # Đặt timeout để không chờ quá lâu
        sock.settimeout(1)
        # Thử kết nối đến cổng 1002
        result = sock.connect_ex(('127.0.0.1', SERVER_PORT))
        sock.close()
        # Nếu kết nối thành công (result = 0), tức là cổng đã được sử dụng
        return result == 0
    except Exception as e:
        logger.error(f"Lỗi khi kiểm tra ứng dụng đang chạy: {str(e)}")
        return False

# Hàm kiểm tra xem ứng dụng đã được cấu hình để khởi động cùng Windows chưa
def is_startup_enabled():
    try:
        # Mở registry key
        key = winreg.OpenKey(
            winreg.HKEY_CURRENT_USER,
            r"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
            0,
            winreg.KEY_READ
        )
        
        # Kiểm tra xem ứng dụng đã được đăng ký chưa
        try:
            value, _ = winreg.QueryValueEx(key, APP_NAME)
            winreg.CloseKey(key)
            return True
        except WindowsError:
            winreg.CloseKey(key)
            return False
    except WindowsError:
        return False

# Hàm thêm ứng dụng vào startup của Windows
def add_to_startup():
    try:
        # Mở registry key
        key = winreg.OpenKey(
            winreg.HKEY_CURRENT_USER,
            r"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
            0,
            winreg.KEY_WRITE
        )
        
        # Thêm ứng dụng vào startup
        winreg.SetValueEx(key, APP_NAME, 0, winreg.REG_SZ, f'"{EXECUTABLE_PATH}"')
        winreg.CloseKey(key)
        logger.info("Đã thêm ứng dụng vào startup của Windows")
        return True
    except WindowsError as e:
        logger.error(f"Lỗi khi thêm ứng dụng vào startup: {str(e)}")
        return False

# Hàm xóa ứng dụng khỏi startup của Windows
def remove_from_startup():
    try:
        # Mở registry key
        key = winreg.OpenKey(
            winreg.HKEY_CURRENT_USER,
            r"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
            0,
            winreg.KEY_WRITE
        )
        
        # Xóa ứng dụng khỏi startup
        winreg.DeleteValue(key, APP_NAME)
        winreg.CloseKey(key)
        logger.info("Đã xóa ứng dụng khỏi startup của Windows")
        return True
    except WindowsError as e:
        logger.error(f"Lỗi khi xóa ứng dụng khỏi startup: {str(e)}")
        return False

# Hàm kiểm tra xem ứng dụng đã chạy lần đầu chưa
def is_first_run():
    return not os.path.exists(CONFIG_FILE)

# Hàm đánh dấu ứng dụng đã chạy lần đầu
def mark_as_run():
    try:
        with open(CONFIG_FILE, 'w') as f:
            f.write("first_run=false\n")
        return True
    except Exception as e:
        logger.error(f"Lỗi khi tạo file cấu hình: {str(e)}")
        return False

# Hàm hiển thị hộp thoại hỏi người dùng có muốn khởi động cùng Windows không
def ask_for_startup():
    result = ctypes.windll.user32.MessageBoxW(
        0,
        "Bạn có muốn ứng dụng tự động khởi động cùng Windows không?",
        "Nhận dạng biển số xe",
        4  # Yes/No
    )
    # 6 = Yes, 7 = No
    return result == 6

# Hàm hiển thị thông báo sử dụng MessageBox của Windows
def show_message(title, message, style=0):
    return ctypes.windll.user32.MessageBoxW(0, message, title, style)
    # Styles: 0 : OK
    #         1 : OK | Cancel
    #         2 : Abort | Retry | Ignore
    #         3 : Yes | No | Cancel
    #         4 : Yes | No
    #         5 : Retry | Cancel 
    #         6 : Cancel | Try Again | Continue

# Hàm hiển thị thông báo tự động đóng sau 3 giây
def show_auto_close_message(title, message):
    # Tạo một thread để hiển thị và tự động đóng thông báo
    def show_and_close():
        # Tạo một cửa sổ thông báo không có nút
        hwnd = ctypes.windll.user32.MessageBoxW(0, message, title, 0)
        # Đợi 3 giây
        time.sleep(3)
        # Đóng cửa sổ thông báo
        if hwnd:
            ctypes.windll.user32.SendMessageW(hwnd, 0x0010, 0, 0)  # 0x0010 là WM_CLOSE
    
    # Chạy trong một thread riêng để không chặn luồng chính
    thread = threading.Thread(target=show_and_close)
    thread.daemon = True
    thread.start()
    return thread

# Tạo icon cho system tray
def create_image():
    # Tạo một hình ảnh đơn giản cho icon
    width = 64
    height = 64
    color1 = (0, 0, 255)  # Màu đỏ
    color2 = (255, 255, 255)  # Màu trắng
    
    image = Image.new('RGB', (width, height), color1)
    dc = ImageDraw.Draw(image)
    
    # Vẽ chữ LP (License Plate) lên icon
    dc.rectangle([(10, 10), (width-10, height-10)], fill=color2)
    dc.text((20, 25), "LP", fill=color1)
    
    return image

# Hàm mở trình duyệt
def open_browser():
    webbrowser.open(server_url)

# Hàm thoát ứng dụng
def exit_app(icon):
    global server_running
    icon.stop()
    server_running = False
    logger.info("Đang tắt ứng dụng...")
    os._exit(0)

# Hàm khởi tạo system tray
def setup_tray():
    image = create_image()
    
    # Hàm xử lý khi người dùng chọn bật/tắt khởi động cùng Windows
    def toggle_startup(icon, item):
        if is_startup_enabled():
            remove_from_startup()
            show_message("Nhận dạng biển số xe", "Đã tắt chế độ khởi động cùng Windows", 0)
        else:
            add_to_startup()
            show_message("Nhận dạng biển số xe", "Đã bật chế độ khởi động cùng Windows", 0)
    
    # Kiểm tra trạng thái khởi động cùng Windows để hiển thị đúng trên menu
    startup_text = "Tắt khởi động cùng Windows" if is_startup_enabled() else "Bật khởi động cùng Windows"
    
    menu = pystray.Menu(
        pystray.MenuItem("Mở ứng dụng", lambda: open_browser()),
        pystray.MenuItem(startup_text, toggle_startup),
        pystray.MenuItem("Thoát", exit_app)
    )
    icon = pystray.Icon("license_plate", image, "Nhận dạng biển số xe", menu)
    return icon

# Khởi tạo các mô hình OCR
try:
    logger.info("Đang khởi tạo các mô hình OCR...")
    db_processor = PaddleDetOnnx(**cfg.config_lp_ocr_det)
    crnn_processor = PaddleCrnnOnnx(**cfg.config_paddle_ocr_rec)
    logger.info("Đã khởi tạo các mô hình OCR thành công")
except Exception as e:
    error_msg = f"Lỗi khi khởi tạo mô hình OCR: {str(e)}"
    logger.error(error_msg)
    # Hiển thị thông báo lỗi
    show_message("Nhận dạng biển số xe - Lỗi", error_msg, 0)
    sys.exit(1)

def remove_special_characters(input_string):
    cleaned_string = re.sub(r'[^A-Z0-9]', '', input_string)
    return cleaned_string

def run_ocr(image, ocr_det_min_size=256, ocr_det_binary_threshold=0.7, ocr_det_polygon_threshold=0.7, ocr_rec_image_width=120):
    try:
        bboxes, bbox_confs = db_processor.run([image], min_size=ocr_det_min_size, polygon_threshold=ocr_det_polygon_threshold, binary_threshold=ocr_det_binary_threshold)
        bboxes[0] = sort_polygon(list(bboxes[0]))
        text_boxes_word = crnn_processor.run(image, bboxes[0], img_width=ocr_rec_image_width)
        text_boxes_word = merge_box_horizontal(text_boxes_word)
        text_boxes_word = merge_box_vertical(text_boxes_word)
        result_text = ""
        for box in text_boxes_word:
            result_text += box.text
        result_text = remove_special_characters(result_text)
        return result_text, text_boxes_word
    except Exception as e:
        logger.error(f"Lỗi khi thực hiện OCR: {str(e)}")
        return "", []

@app.route('/')
def index():
    return send_from_directory(static_folder, 'index.html')

@app.route('/<path:path>')
def static_files(path):
    return send_from_directory(static_folder, path)

@app.route('/license_plate_ocr_api', methods=['POST'])
def recognize_license_plate_text():
    try:
        if 'file' not in request.files:
            return jsonify({"error": "Không tìm thấy file"}), 400
            
        file = request.files['file']
        if file.filename == '':
            return jsonify({"error": "Không có file được chọn"}), 400
            
        extension = file.filename.split(".")[-1]
        if extension.lower() not in ["jpeg", "jpg", "jpe", "webp", "jp2", "png"]:
            return jsonify({"error": "File phải là định dạng hình ảnh!"}), 400
            
        # Đọc file hình ảnh
        file_bytes = file.read()
        np_arr = np.frombuffer(file_bytes, np.uint8)
        img = cv2.imdecode(np_arr, cv2.IMREAD_COLOR)
        
        # Thực hiện OCR
        license_text, text_boxes = run_ocr(
            image=img, 
            ocr_det_min_size=256, 
            ocr_det_binary_threshold=0.7, 
            ocr_det_polygon_threshold=0.7, 
            ocr_rec_image_width=120
        )
        
        # Vẽ kết quả lên hình ảnh
        visualization_img = img.copy()
        for box in text_boxes:
            cv2.rectangle(visualization_img, (box.xmin, box.ymin), (box.xmax, box.ymax), (0, 255, 0), 2)
            cv2.putText(visualization_img, box.text, (box.xmin, box.ymin - 5), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 255), 2)
        
        # Trả về kết quả
        result = {"license_text": license_text}
        return jsonify(result), 200
        
    except Exception as e:
        logger.error(f"Lỗi khi xử lý hình ảnh: {str(e)}")
        return jsonify({"error": f"Lỗi khi xử lý hình ảnh: {str(e)}"}), 500

# Hàm chạy Flask server trong một thread riêng
def run_server():
    global server_running
    server_running = True
    try:
        app.run(host="0.0.0.0", port=SERVER_PORT, debug=False, use_reloader=False)
    except Exception as e:
        logger.error(f"Lỗi khi khởi động server: {str(e)}")
        # Hiển thị thông báo lỗi
        threading.Thread(target=lambda: show_message("Nhận dạng biển số xe - Lỗi", 
                                                   f"Lỗi khi khởi động server: {str(e)}", 
                                                   0)).start()
        server_running = False

if __name__ == "__main__":
    logger.info("Đang khởi động ứng dụng nhận dạng biển số xe...")
    
    # Kiểm tra xem ứng dụng đã đang chạy hay chưa
    if is_app_already_running():
        logger.info("Ứng dụng đã đang chạy")
        show_message("Nhận dạng biển số xe", "Hệ thống đã được chạy!", 0)
        sys.exit(0)
    
    # Kiểm tra xem ứng dụng đã chạy lần đầu chưa
    first_run = is_first_run()
    if first_run and getattr(sys, 'frozen', False):
        logger.info("Đây là lần đầu chạy ứng dụng")
        # Hỏi người dùng có muốn khởi động cùng Windows không
        if ask_for_startup():
            add_to_startup()
        # Đánh dấu ứng dụng đã chạy lần đầu
        mark_as_run()
    
    # Khởi động server trong một thread riêng
    server_thread = threading.Thread(target=run_server)
    server_thread.daemon = True
    server_thread.start()
    
    # Đợi server khởi động
    time.sleep(2)
    
    if server_running:
        # Hiển thị thông báo thành công tự động đóng sau 3 giây
        show_auto_close_message("Nhận dạng biển số xe", 
                              "Khởi động hệ thống nhận diện thành công!")
        
        # Không tự động mở trình duyệt
        
        # Khởi tạo và chạy system tray
        icon = setup_tray()
        icon.run()
    else:
        # Hiển thị thông báo lỗi
        show_message("Nhận dạng biển số xe - Lỗi", 
                    "Không thể khởi động ứng dụng", 
                    0)
        input("Nhấn Enter để thoát...")
        sys.exit(1) 