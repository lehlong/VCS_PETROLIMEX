using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using LibVLCSharp.Shared;
using System.Windows.Input;
using System.Net.Http.Headers;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using VCS.FORM.Model;
using Microsoft.AspNetCore.Identity.Data;
using System.Text;
using VCS.FORM.Utilities;
using DMS.BUSINESS.Services.Auth;
using DMS.BUSINESS.Services.SMO;

namespace VCS.FORM.Views.Pages
{
    /// <summary>
    /// Interaction logic for CheckInPages.xaml
    /// </summary>
    public partial class CheckInPages : Page
    {
        private LibVLC libVLC;
        private Media media;
        private MediaPlayer player;
        private DispatcherTimer timer;
        private List<OrderListItem> allItems = new List<OrderListItem>();
        private int currentPage = 1;
        private int itemsPerPage = 10;
        private static readonly HttpClient client = new HttpClient();

        public CheckInPages()
        {
            InitializeComponent();
            InitializeTimer();
            LoadData();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //CheckInTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeLibVLC();
            InitializePlayer();
        }

        private void InitializeLibVLC()
        {
            Core.Initialize();
            libVLC = new LibVLC(
                "--network-caching=100",
                "--live-caching=100",
                "--file-caching=100",
                "--clock-jitter=0",
                "--clock-synchro=0",
                "--no-audio",
                "--rtsp-tcp"
            );
        }

        private void InitializePlayer()
        {
            try
            {
                string rtspUrl = "rtsp://admin:D2s@2024@192.168.110.6/Streaming/Channels/1";
                media = new Media(libVLC, rtspUrl, FromType.FromLocation);
                player = new MediaPlayer(media);
                CameraStream.MediaPlayer = player;
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo camera: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ScanQR_Click(object sender, RoutedEventArgs e)
        {
            // Implement QR scanning logic here
            MessageBox.Show("Tính năng đang được phát triển", "Thông báo");
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            StopAndDisposeMediaPlayers();
            timer?.Stop();
        }

        private void StopAndDisposeMediaPlayers()
        {
            player?.Stop();
            player?.Dispose();
            media?.Dispose();
            libVLC?.Dispose();
        }

        private void LicensePlate_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void CheckIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingOverlay.Visibility = Visibility.Visible;
                
                // Gán thời gian hiện tại vào TextBox
                CheckInTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                var (imagePath, snapshotImage) = await TakeSnapshot();
                if (!string.IsNullOrEmpty(imagePath) && snapshotImage != null)
                {
                    VehicleImage.Source = snapshotImage;

                    var (licensePlate, croppedImage) = await DetectLicensePlateAsync(imagePath);
                    
                    if (!string.IsNullOrEmpty(licensePlate) && croppedImage != null)
                    {
                        LicensePlate.Text = licensePlate;
                        LicensePlateImage.Source = croppedImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xử lý check-in: {ex.Message}", "Lỗi");
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void RegisterDriver_Click(object sender, RoutedEventArgs e)
        {
            // Open driver registration window/dialog
            MessageBox.Show("Mở form đăng ký tài xế", "Thông báo");
        }

        private async void DO_SAP_Click(object sender, RoutedEventArgs e)
        {
            var authService = new AuthSMOService();
            string token = await authService.Login();
            var dataService = new DOSAPService();
            string doNumber = DO_SAP.Text.Trim();
            if (string.IsNullOrEmpty(doNumber))
            {
                MessageBox.Show("Vui lòng nhập số DO SAP!");
                return;
            }
            string apiUrl = $"https://smoapiuat.petrolimex.com.vn/api/PO/GetDO?doNumber={doNumber}";
            string data = await dataService.GetData(apiUrl, token);
            MessageBox.Show(data);
        }
       

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
            {
                scrollviewer.LineLeft();
                scrollviewer.LineLeft();
            }
            else
            {
                scrollviewer.LineRight();
                scrollviewer.LineRight();
            }
            e.Handled = true;
        }

        private async Task<(string? licensePlate, BitmapImage? croppedImage)> DetectLicensePlateAsync(string imagePath)
        {
            try
            {
                var byteArray = System.IO.File.ReadAllBytes(imagePath);
                using (var content = new MultipartFormDataContent())
                {
                    var imageContent = new ByteArrayContent(byteArray);
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    content.Add(imageContent, "file", Path.GetFileName(imagePath));

                    var response = await client.PostAsync("http://localhost:5000/api/detect", content);
                    var jsonString = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JObject.Parse(jsonString);

                    // Kiểm tra response
                    if (jsonResponse?.cropped_image == null || jsonResponse?.license_plate == null)
                    {
                        MessageBox.Show("Không nhận diện được biển số xe", "Thông báo");
                        return (null, null);
                    }

                    string base64Image = jsonResponse.cropped_image;
                    byte[] imageBytes = Convert.FromBase64String(base64Image);
                    
                    BitmapImage croppedImage = new BitmapImage();
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        croppedImage.BeginInit();
                        croppedImage.CacheOption = BitmapCacheOption.OnLoad;
                        croppedImage.StreamSource = ms;
                        croppedImage.EndInit();
                        croppedImage.Freeze();
                    }

                    // Lưu ảnh chỉ khi có dữ liệu hợp lệ
                    DateTime now = DateTime.Now;
                    string basePath = @"D:\SavePictrue";
                    string yearPath = Path.Combine(basePath, now.Year.ToString());
                    string monthPath = Path.Combine(yearPath, now.Month.ToString("00"));
                    string dayPath = Path.Combine(monthPath, now.Day.ToString("00"));
                    Directory.CreateDirectory(dayPath);

                    string fileName = $"{Guid.NewGuid()}_LP.jpg";
                    string savePath = Path.Combine(dayPath, fileName);
                    File.WriteAllBytes(savePath, imageBytes);

                    return (jsonResponse.license_plate.ToString(), croppedImage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi nhận diện biển số: {ex.Message}", "Lỗi");
                return (null, null);
            }
        }

        private async Task<(string? filePath, BitmapImage? image)> TakeSnapshot()
        {
            try 
            {
                if (player != null && player.IsPlaying)
                {
                    DateTime now = DateTime.Now;

                    string basePath = @"D:\SavePictrue";
                    string yearPath = Path.Combine(basePath, now.Year.ToString());
                    string monthPath = Path.Combine(yearPath, now.Month.ToString("00"));
                    string dayPath = Path.Combine(monthPath, now.Day.ToString("00"));
                    Directory.CreateDirectory(dayPath);

                    string fileName = $"{Guid.NewGuid()}.png";
                    string filePath = Path.Combine(dayPath, fileName);

                    bool snapshotTaken = player.TakeSnapshot(0, filePath, 0, 0);

                    if (snapshotTaken)
                    {
                        int maxAttempts = 10;
                        int attempts = 0;
                        while (!File.Exists(filePath) && attempts < maxAttempts)
                        {
                            await Task.Delay(100);
                            attempts++;
                        }

                        if (File.Exists(filePath))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.UriSource = new Uri(filePath);
                            bitmap.EndInit();
                            bitmap.Freeze();

                            return (filePath, bitmap);
                        }
                    }

                    throw new Exception("Không thể tạo ảnh snapshot");
                }
                else
                {
                    MessageBox.Show("Không thể chụp ảnh, vui lòng bật luồng video trước.", "Thông báo");
                    return (null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chụp ảnh: {ex.Message}", "Lỗi");
                return (null, null);
            }
        }

        private void LoadData()
        {
            // Giả lập dữ liệu mẫu
            allItems = Enumerable.Range(1, 50).Select(i => new OrderListItem
            {
                Index = i,
                OrderNumber = $"DO00{1000 + i}",
                Time = DateTime.Now.AddMinutes(-i).ToString("dd/MM/yyyy HH:mm:ss"),
                LicensePlate = $"37C-{i:D5}",
                Driver = $"Tài xế {i}",
                OrderInfo = $"Xi măng PCB40 - {20 + i} tấn",
                Status = i % 3 == 0 ? "Đã duyệt" : (i % 3 == 1 ? "Chờ duyệt" : "Từ chối")
            }).ToList();

            UpdatePageDisplay();
        }

        private void UpdatePageDisplay()
        {
            if (allItems != null && allItems.Any())
            {
                var skipCount = (currentPage - 1) * itemsPerPage;
                OrderList.ItemsSource = allItems.Skip(skipCount).Take(itemsPerPage);
                
                int totalPages = (int)Math.Ceiling(allItems.Count / (double)itemsPerPage);
                CurrentPageText.Text = $"{currentPage}/{totalPages}";
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdatePageDisplay();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            int totalPages = (int)Math.Ceiling(allItems.Count / (double)itemsPerPage);
            if (currentPage < totalPages)
            {
                currentPage++;
                UpdatePageDisplay();
            }
        }

        private void PageSize_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (PageSizeComboBox.SelectedItem is ComboBoxItem selectedItem && allItems != null)
            {
                itemsPerPage = int.Parse(selectedItem.Content.ToString());
                currentPage = 1;
                UpdatePageDisplay();
            }
        }
    }
}
