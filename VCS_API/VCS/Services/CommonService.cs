using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using VCS.APP.Utilities;
using System.Text.Json;
using DMS.CORE;
using Microsoft.Extensions.Configuration;
using VCS.Services;
using Emgu.CV;
using Emgu.CV.Util;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace VCS.APP.Services
{
    public class CommonService
    {
        public static void LoadUserConfig()
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
                Global.SmoApiUsername = config["Setting:SmoApiUsername"];
                Global.SmoApiPassword = config["Setting:SmoApiPassword"];
                Global.SmoApiUrl = config["Setting:SmoApiUrl"];
                Global.PathSaveFile = config["Setting:PathSaveFile"];
                Global.DetectApiUrl = config["Setting:DetectApiUrl"];
                Global.DetectFilePath = config["Setting:DetectFilePath"];
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tải cấu hình người dùng: {ex.Message}");
            }
        }

        #region SMO API
        public static string LoginSmoApi()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"{Global.SmoApiUrl}/Authorize/Login?username={Global.SmoApiUsername}&password={Global.SmoApiPassword}");
                    HttpResponseMessage response = client.Send(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        var data = JsonSerializer.Deserialize<ResponseLoginSmoApi>(responseContent);
                        if (string.IsNullOrEmpty(data.DATA))
                        {
                            return null;
                        }
                        else
                        {
                            return data.DATA;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DOSAPDataDto GetDetailDO(string number)
        {
            try
            {
                var token = LoginSmoApi();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var request = new HttpRequestMessage(HttpMethod.Get, $"{Global.SmoApiUrl}PO/GetDO?doNumber={number}");
                    HttpResponseMessage response = client.Send(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        var data = JsonSerializer.Deserialize<DOSAPDataDto>(responseContent);
                        return data;
                    }
                    else
                    {
                        var resEx = new DOSAPDataDto();
                        resEx.STATUS = false;
                        return resEx;
                    }
                }
            }
            catch (Exception ex)
            {
                var resEx = new DOSAPDataDto();
                resEx.STATUS = false;
                return resEx;
            }
        }

        public static ResponseLoginSmoApi CheckInvoice(string number)
        {
            try
            {
                var token = LoginSmoApi();
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var request = new HttpRequestMessage(HttpMethod.Get, $"{Global.SmoApiUrl}PO/CheckInvoice?doNumber={number}");
                    HttpResponseMessage response = client.Send(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        var data = JsonSerializer.Deserialize<ResponseLoginSmoApi>(responseContent);
                        return data;
                    }
                    else
                    {
                        var resEx = new ResponseLoginSmoApi();
                        resEx.STATUS = false;
                        return resEx;
                    }
                }
            }
            catch (Exception ex)
            {
                var resEx = new ResponseLoginSmoApi();
                resEx.STATUS = false;
                return resEx;
            }
        }


        #endregion

        #region Nhận diện và xử lý file ảnh
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
        public static byte[] CaptureFrameFromRTSP(string rtspUrl)
        {
            using (VideoCapture capture = new VideoCapture(rtspUrl))
            {
                if (!capture.IsOpened)
                {
                    throw new Exception("Không thể kết nối đến luồng RTSP.");
                }
                using (Mat frame = new Mat())
                {
                    capture.Read(frame);

                    if (frame.IsEmpty)
                    {
                        throw new Exception("Không thể lấy khung hình từ luồng RTSP.");
                    }

                    return MatToByteArray(frame);
                }
            }
        }
        public static byte[] MatToByteArray(Mat mat)
        {
            using (VectorOfByte buffer = new VectorOfByte())
            {
                bool success = CvInvoke.Imencode(".jpg", mat, buffer); // Chuyển thành mảng byte

                if (!success || buffer.Size == 0)
                {
                    throw new Exception("Lỗi mã hóa ảnh từ Mat.");
                }

                return buffer.ToArray(); // Chuyển về byte[]
            }
        }
        public static string SaveDetectedImage(byte[] imageBytes)
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
        public static async Task<(string? licensePlate, Image? croppedImage, string? imagePath)> DetectLicensePlateAsync(string imagePath)
        {
            try
            {
                HttpClient _client = new HttpClient();
                var byteArray = File.ReadAllBytes(imagePath);
                using (var content = new MultipartFormDataContent())
                {
                    // Add image file
                    var imageContent = new ByteArrayContent(byteArray);
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    content.Add(imageContent, "file", Path.GetFileName(imagePath));

                    // Add other parameters with default values
                    content.Add(new StringContent("0.5"), "lp_det_iou_threshold");
                    content.Add(new StringContent("0.7"), "lp_det_conf_threshold");
                    content.Add(new StringContent("128"), "ocr_det_min_size");
                    content.Add(new StringContent("0.7"), "ocr_det_binary_threshold");
                    content.Add(new StringContent("0.7"), "ocr_det_polygon_threshold");
                    content.Add(new StringContent("120"), "ocr_rec_image_width");

                    var response = await _client.PostAsync(Global.DetectApiUrl, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"API trả về lỗi: {response.StatusCode}");
                    }

                    var jsonString = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(jsonString);
                    JArray dataArray = (JArray)jsonResponse["data"];

                    if (dataArray == null || !dataArray.Any())
                    {
                        throw new Exception("Không nhận diện được biển số xe");
                    }

                    // Get the first license plate detection
                    var detection = dataArray.First();
                    string licensePlate = detection["text"].ToString();

                    // Extract coordinates
                    int xmin = (int)detection["xmin"];
                    int ymin = (int)detection["ymin"];
                    int xmax = (int)detection["xmax"];
                    int ymax = (int)detection["ymax"];

                    // Load the original image and crop the license plate region
                    using (Image originalImage = Image.FromFile(imagePath))
                    {
                        int width = xmax - xmin;
                        int height = ymax - ymin;

                        using (Bitmap croppedBitmap = new Bitmap(width, height))
                        {
                            using (Graphics g = Graphics.FromImage(croppedBitmap))
                            {
                                g.DrawImage(originalImage, 
                                    new Rectangle(0, 0, width, height),
                                    new Rectangle(xmin, ymin, width, height),
                                    GraphicsUnit.Pixel);
                            }

                            // Save the cropped image
                            string savedImagePath = SaveDetectedImage(ImageToByteArray(croppedBitmap));

                            // Create a new image for return
                            Image croppedImage = Image.FromFile(savedImagePath);
                            return (licensePlate, croppedImage, savedImagePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi nhận diện biển số: {ex.Message}");
            }
        }

        private static byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        #endregion
    }
}
