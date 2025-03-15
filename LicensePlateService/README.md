# Ứng dụng nhận dạng biển số xe

Ứng dụng này sử dụng công nghệ OCR (Optical Character Recognition) để nhận dạng biển số xe từ hình ảnh.

## Cách đóng gói ứng dụng

### Bước 1: Đóng gói ứng dụng
Chạy file batch để tự động cài đặt các thư viện cần thiết và đóng gói ứng dụng:
```
.\build.bat
```

### Bước 2: Chạy ứng dụng
Sau khi đóng gói, bạn có thể chạy file `dist\LicensePlateOCR.exe`.

## Tính năng của ứng dụng

1. **System Tray**: Ứng dụng chạy ẩn trong khay hệ thống (system tray)
2. **Thông báo tự động đóng**: Hiển thị thông báo "Khởi động hệ thống nhận diện thành công!" và tự động đóng sau 3 giây
3. **API nhận dạng biển số**: Cung cấp API để nhận dạng biển số xe từ hình ảnh
4. **Khởi động cùng Windows**: Tự động hỏi người dùng có muốn khởi động cùng Windows khi chạy lần đầu
5. **Kiểm tra ứng dụng đã chạy**: Kiểm tra xem ứng dụng đã đang chạy hay chưa, nếu đã chạy thì hiển thị thông báo và không khởi động thêm

## Cách sử dụng ứng dụng

1. Khởi động ứng dụng bằng cách chạy file `dist\LicensePlateOCR.exe`
2. Ứng dụng sẽ hiển thị thông báo khởi động thành công và chạy ẩn trong khay hệ thống
3. Nhấp chuột phải vào biểu tượng trong khay hệ thống để:
   - Mở ứng dụng: Mở trình duyệt web với địa chỉ http://localhost:1002
   - Bật/Tắt khởi động cùng Windows: Cho phép ứng dụng tự động khởi động khi Windows khởi động
   - Thoát: Đóng ứng dụng
4. Sử dụng API `/license_plate_ocr_api` để nhận dạng biển số xe từ hình ảnh

### Ví dụ sử dụng API với cURL
```
curl -X POST "http://localhost:1002/license_plate_ocr_api" -H "accept: application/json" -H "Content-Type: multipart/form-data" -F "file=@duong_dan_den_hinh_anh.jpg"
```

### Ví dụ sử dụng API với Python
```python
import requests

url = "http://localhost:1002/license_plate_ocr_api"
files = {"file": open("duong_dan_den_hinh_anh.jpg", "rb")}
response = requests.post(url, files=files)
print(response.json())
```

## Lưu ý
- Ứng dụng chỉ hỗ trợ các định dạng hình ảnh: jpeg, jpg, jpe, webp, jp2, png
- Kết quả trả về bao gồm văn bản biển số xe đã được nhận dạng
- Ứng dụng sử dụng Flask làm web server và chạy trên cổng 1002
- Để đóng gói lại ứng dụng, luôn sử dụng file `build.bat`
- Khi chạy lần đầu, ứng dụng sẽ hỏi bạn có muốn khởi động cùng Windows không
- Nếu ứng dụng đã đang chạy, khi bạn cố gắng chạy lại, sẽ hiển thị thông báo "Hệ thống đã được chạy!" và không khởi động thêm 