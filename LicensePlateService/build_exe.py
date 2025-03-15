import PyInstaller.__main__
import os
import shutil
import time

# Xóa thư mục dist nếu tồn tại
if os.path.exists('dist'):
    try:
        print("Đang xóa thư mục dist cũ...")
        shutil.rmtree('dist')
        # Đợi một chút để đảm bảo thư mục đã được xóa hoàn toàn
        time.sleep(2)
    except Exception as e:
        print(f"Không thể xóa thư mục dist: {str(e)}")
        print("Vui lòng đóng tất cả các ứng dụng đang sử dụng file trong thư mục dist và thử lại.")
        input("Nhấn Enter để thoát...")
        exit(1)

# Tạo thư mục dist mới
os.makedirs('dist', exist_ok=True)

# Tạo thư mục checkpoints trong dist
os.makedirs('dist/checkpoints', exist_ok=True)

# Sao chép các file mô hình vào thư mục dist
if os.path.exists('checkpoints/paddle_ocr_reg_onnx'):
    os.makedirs('dist/checkpoints/paddle_ocr_reg_onnx', exist_ok=True)
    for file in os.listdir('checkpoints/paddle_ocr_reg_onnx'):
        shutil.copy2(os.path.join('checkpoints/paddle_ocr_reg_onnx', file), 
                    os.path.join('dist/checkpoints/paddle_ocr_reg_onnx', file))

if os.path.exists('checkpoints/paddle_ocr_det_onnx'):
    os.makedirs('dist/checkpoints/paddle_ocr_det_onnx', exist_ok=True)
    for file in os.listdir('checkpoints/paddle_ocr_det_onnx'):
        shutil.copy2(os.path.join('checkpoints/paddle_ocr_det_onnx', file), 
                    os.path.join('dist/checkpoints/paddle_ocr_det_onnx', file))

# Sao chép thư mục static vào dist
if os.path.exists('static'):
    os.makedirs('dist/static', exist_ok=True)
    for file in os.listdir('static'):
        shutil.copy2(os.path.join('static', file), 
                    os.path.join('dist/static', file))

# Xóa file spec nếu tồn tại
spec_file = 'LicensePlateOCR.spec'
if os.path.exists(spec_file):
    try:
        os.remove(spec_file)
        print(f"Đã xóa file {spec_file}")
    except Exception as e:
        print(f"Không thể xóa file {spec_file}: {str(e)}")

# Chạy PyInstaller để đóng gói ứng dụng
print("Đang đóng gói ứng dụng...")
PyInstaller.__main__.run([
    'app.py',
    '--name=LicensePlateOCR',
    '--onefile',
    '--windowed',
    '--add-data=checkpoints;checkpoints',
    '--add-data=models;models',
    '--add-data=static;static',
    '--add-data=config.py;.',
    '--add-data=box_post_processing.py;.',
    '--add-data=definitions.py;.',
    '--hidden-import=flask',
    '--hidden-import=werkzeug',
    '--hidden-import=pystray',
    '--hidden-import=PIL',
    '--hidden-import=PIL.Image',
    '--hidden-import=PIL.ImageDraw',
    '--hidden-import=webbrowser',
    '--hidden-import=threading',
    '--hidden-import=time',
    '--hidden-import=ctypes',
])

print("Đã đóng gói ứng dụng thành công!") 