@echo off
echo Đang cài đặt các thư viện cần thiết...
pip install -r requirements.txt

echo Đang đóng gói ứng dụng phiên bản thông báo tự động đóng...
python build_exe.py

if %ERRORLEVEL% EQU 0 (
    echo Đã hoàn thành! Bạn có thể chạy ứng dụng bằng cách chạy file dist\LicensePlateOCR.exe
) else (
    echo Đóng gói không thành công. Vui lòng kiểm tra lỗi ở trên.
)
pause 