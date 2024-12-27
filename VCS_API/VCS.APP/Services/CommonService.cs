using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using VCS.APP.Utilities;

namespace VCS.APP.Services
{
    public class CommonService
    {
        private static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// Chụp ảnh từ camera stream
        /// </summary>
        public static async Task<(string? filePath, Image? image)> TakeSnapshot(LibVLCSharp.Shared.MediaPlayer player)
        {
            try
            {
                if (player != null && player.IsPlaying)
                {
                    // Tạo đường dẫn theo cấu trúc năm/tháng/ngày
                    DateTime now = DateTime.Now;
                    string basePath = Global.PathSaveFile;
                    string yearPath = Path.Combine(basePath, now.Year.ToString());
                    string monthPath = Path.Combine(yearPath, now.Month.ToString("00"));
                    string dayPath = Path.Combine(monthPath, now.Day.ToString("00"));
                    Directory.CreateDirectory(dayPath);

                    string fileName = $"{Guid.NewGuid()}.png";
                    string filePath = Path.Combine(dayPath, fileName);

                    bool snapshotTaken = player.TakeSnapshot(0, filePath, 0, 0);

                    if (snapshotTaken)
                    {
                        // Đợi file được tạo
                        int maxAttempts = 10;
                        int attempts = 0;
                        while (!File.Exists(filePath) && attempts < maxAttempts)
                        {
                            await Task.Delay(100);
                            attempts++;
                        }

                        if (File.Exists(filePath))
                        {
                            Image image = Image.FromFile(filePath);
                            return (filePath, image);
                        }
                    }

                    throw new Exception("Không thể tạo ảnh snapshot");
                }
                else
                {
                    throw new Exception("Camera không hoạt động");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi chụp ảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Nhận diện biển số xe từ ảnh
        /// </summary>
        public static async Task<(string? licensePlate, Image? croppedImage, string? imagePath)> DetectLicensePlateAsync(string imagePath)
        {
            try
            {
                var byteArray = File.ReadAllBytes(imagePath);
                using (var content = new MultipartFormDataContent())
                {
                    var imageContent = new ByteArrayContent(byteArray);
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    content.Add(imageContent, "file", Path.GetFileName(imagePath));

                    var response = await _client.PostAsync(Global.DetectApiUrl, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"API trả về lỗi: {response.StatusCode}");
                    }

                    var jsonString = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JObject.Parse(jsonString);

                    if (jsonResponse?.cropped_image == null || jsonResponse?.license_plate == null)
                    {
                        throw new Exception("Không nhận diện được biển số xe");
                    }

                    string base64Image = jsonResponse.cropped_image;
                    byte[] imageBytes = Convert.FromBase64String(base64Image);

                    // Lưu ảnh đã cắt
                    string savedImagePath = SaveDetectedImage(imageBytes);

                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        Image croppedImage = Image.FromStream(ms);
                        return (jsonResponse.license_plate.ToString(), croppedImage, savedImagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi nhận diện biển số: {ex.Message}");
            }
        }

        /// <summary>
        /// Lưu ảnh biển số đã được cắt
        /// </summary>
        private static string SaveDetectedImage(byte[] imageBytes)
        {
            try
            {
                DateTime now = DateTime.Now;
                string basePath = Global.PathSaveFile;
                string yearPath = Path.Combine(basePath, now.Year.ToString());
                string monthPath = Path.Combine(yearPath, now.Month.ToString("00"));
                string dayPath = Path.Combine(monthPath, now.Day.ToString("00"));
                Directory.CreateDirectory(dayPath);

                string fileName = $"{Guid.NewGuid()}_LP.jpg";
                string savePath = Path.Combine(dayPath, fileName);
                File.WriteAllBytes(savePath, imageBytes);

                return savePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu ảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Dọn dẹp tài nguyên camera
        /// </summary>
        public static void CleanupCameraResources(Dictionary<string, LibVLCSharp.Shared.MediaPlayer> mediaPlayers, LibVLCSharp.Shared.LibVLC? libVLC)
        {
            try
            {
                foreach (var player in mediaPlayers.Values)
                {
                    player.Stop();
                    player.Dispose();
                }
                mediaPlayers.Clear();
                libVLC?.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi dọn dẹp tài nguyên: {ex.Message}");
            }
        }
    }
}
