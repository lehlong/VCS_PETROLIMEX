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
using DMS.CORE.Entities.AD;

namespace VCS.APP.Services
{
    public class CommonService
    {
        public static TblAdAccount CurrentUser { get; private set; }
        public static Dictionary<string, bool> UserPermissions { get; private set; } = new Dictionary<string, bool>();

        public static void SetCurrentUser(TblAdAccount user)
        {
            CurrentUser = user;
        }

        public static void LoadUserPermissions(TblAdAccount user)
        {
            UserPermissions.Clear();

            if (user == null) return;

            var groupRights = user.Account_AccountGroups?
                .Where(ag => ag.AccountGroup != null)
                .SelectMany(ag => ag.AccountGroup.ListAccountGroupRight ?? new List<TblAdAccountGroupRight>())
                .Select(r => r.RightId)
                .Distinct()
                .ToList() ?? new List<string>();

            foreach (var rightId in groupRights)
            {
                UserPermissions[rightId] = true;
            }

            var accountRights = user.AccountRights ?? new List<TblAdAccountRight>();

            foreach (var right in accountRights.Where(r => r.IsAdded == true))
            {
                UserPermissions[right.RightId] = true;
            }

            foreach (var right in accountRights.Where(r => r.IsRemoved == true))
            {
                UserPermissions[right.RightId] = false;
            }
        }
        public static bool HasPermission(string rightId)
        {
            return UserPermissions.ContainsKey(rightId) && UserPermissions[rightId];
        }
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
        public static (string? filePath, Image? image) TakeSnapshot(LibVLCSharp.Shared.MediaPlayer player)
        {
            try
            {
                // Kiểm tra nhanh nếu player không hợp lệ hoặc không đang phát
                if (player == null || !player.IsPlaying)
                    throw new Exception("Camera không hoạt động");

                // Tạo đường dẫn lưu ảnh theo cấu trúc năm/tháng/ngày
                DateTime now = DateTime.Now;
                string basePath = Global.PathSaveFile;
                string yearPath = Path.Combine(basePath, now.Year.ToString());
                string monthPath = Path.Combine(yearPath, now.Month.ToString("00"));
                string dayPath = Path.Combine(monthPath, now.Day.ToString("00"));

                // Tạo thư mục nếu chưa có
                Directory.CreateDirectory(dayPath);

                string fileName = $"{Guid.NewGuid()}.png";
                string filePath = Path.Combine(dayPath, fileName);

                // Chụp ảnh snapshot đồng bộ
                bool snapshotTaken = player.TakeSnapshot(0, filePath, 0, 0);

                if (!snapshotTaken)
                    throw new Exception("Không thể tạo ảnh snapshot");

                // Kiểm tra nhanh nếu tệp tin đã được tạo
                if (File.Exists(filePath))
                {
                    // Trả về ngay nếu ảnh đã được tạo thành công
                    return (filePath, Image.FromFile(filePath));
                }

                // Nếu tệp tin chưa có, tối ưu hóa vòng lặp
                // Đợi tối đa 3 lần với khoảng cách ngắn hơn
                for (int attempts = 0; attempts < 3; attempts++)
                {
                    Thread.Sleep(50); // Đợi ít hơn, giảm độ trễ

                    // Nếu file đã tồn tại, đọc ảnh
                    if (File.Exists(filePath))
                    {
                        return (filePath, Image.FromFile(filePath));
                    }
                }

                throw new Exception("Ảnh snapshot không được tạo sau nhiều lần thử");
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
                // Tái sử dụng HttpClient
                using (HttpClient _client = new HttpClient())
                {
                    var byteArray = File.ReadAllBytes(imagePath);
                    using (var content = new MultipartFormDataContent())
                    {
                        // Add image file
                        var imageContent = new ByteArrayContent(byteArray);
                        imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        content.Add(imageContent, "file", Path.GetFileName(imagePath));

                        // Add other parameters
                        AddDetectionParameters(content);

                        var response = await _client.PostAsync(Global.DetectApiUrl, content);
                        if (!response.IsSuccessStatusCode)
                            throw new Exception($"API trả về lỗi: {response.StatusCode}");

                        var jsonString = await response.Content.ReadAsStringAsync();
                        JObject jsonResponse = JObject.Parse(jsonString);
                        JArray dataArray = (JArray)jsonResponse["data"];

                        if (dataArray == null || !dataArray.Any())
                            throw new Exception("Không nhận diện được biển số xe");

                        // Get the first license plate detection
                        var detection = dataArray.First();
                        string licensePlate = detection["text"].ToString();

                        // Extract coordinates
                        var (xmin, ymin, xmax, ymax) = ExtractCoordinates(detection);

                        // Crop image
                        var croppedImage = CropImage(imagePath, xmin, ymin, xmax, ymax);

                        // Save and return the cropped image
                        string savedImagePath = SaveDetectedImage(croppedImage);
                        return (licensePlate, croppedImage, savedImagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi nhận diện biển số: {ex.Message}");
            }
        }

        // Phương thức để thêm các tham số cần thiết vào content
        private static void AddDetectionParameters(MultipartFormDataContent content)
        {
            content.Add(new StringContent("0.5"), "lp_det_iou_threshold");
            content.Add(new StringContent("0.7"), "lp_det_conf_threshold");
            content.Add(new StringContent("128"), "ocr_det_min_size");
            content.Add(new StringContent("0.7"), "ocr_det_binary_threshold");
            content.Add(new StringContent("0.7"), "ocr_det_polygon_threshold");
            content.Add(new StringContent("120"), "ocr_rec_image_width");
        }

        // Phương thức để trích xuất tọa độ
        private static (int xmin, int ymin, int xmax, int ymax) ExtractCoordinates(JToken detection)
        {
            int xmin = (int)detection["xmin"];
            int ymin = (int)detection["ymin"];
            int xmax = (int)detection["xmax"];
            int ymax = (int)detection["ymax"];
            return (xmin, ymin, xmax, ymax);
        }

        // Phương thức cắt ảnh từ tọa độ
        private static Image CropImage(string imagePath, int xmin, int ymin, int xmax, int ymax)
        {
            using (Image originalImage = Image.FromFile(imagePath))
            {
                int width = xmax - xmin;
                int height = ymax - ymin;

                Bitmap croppedBitmap = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(croppedBitmap))
                {
                    g.DrawImage(originalImage,
                                new Rectangle(0, 0, width, height),
                                new Rectangle(xmin, ymin, width, height),
                                GraphicsUnit.Pixel);
                }

                return croppedBitmap;
            }
        }

        // Phương thức lưu ảnh đã cắt
        private static string SaveDetectedImage(Image croppedImage)
        {
            string savedImagePath = Path.Combine(Global.PathSaveFile, $"{Guid.NewGuid()}.png");

            // Lưu ảnh vào thư mục được chỉ định
            croppedImage.Save(savedImagePath, System.Drawing.Imaging.ImageFormat.Png);

            return savedImagePath;
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
