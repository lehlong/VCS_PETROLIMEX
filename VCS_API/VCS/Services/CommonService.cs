using System.Net.Http.Headers;
using VCS.APP.Utilities;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using VCS.Services;
using Emgu.CV;
using Emgu.CV.Util;
using System.Drawing.Imaging;
using VCS.Areas.Alert;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Text;
using Python.Runtime;
using System.Security.Cryptography;
using VCS.DbContext.Entities.AD;

namespace VCS.APP.Services
{
    public class CommonService
    {
        public static Dictionary<string, bool> UserPermissions { get; private set; } = new Dictionary<string, bool>();

        #region Xử lý phân quyền 
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
        #endregion

        #region Load Config vào Global từ appsetting.json
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
               
                Global.VcsUrl = config["Setting:VcsUrl"];
                Global.CropWidth = string.IsNullOrEmpty(config["Setting:CropImagesWidth"].ToString()) ? 0: Math.Max(0, Convert.ToUInt32(config["Setting:CropImagesWidth"]));
                Global.CropHeight = string.IsNullOrEmpty(config["Setting:CropImagesHeight"].ToString()) ? 0 : Math.Max(0, Convert.ToUInt32(config["Setting:CropImagesHeight"]));
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tải cấu hình người dùng: {ex.Message}");
            }
        }

        #endregion

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
        public static void PostStatusVehicleToSMO(PostStatusVehicleToSMO model)
        {
            try
            {
                var token = LoginSmoApi();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var request = new HttpRequestMessage(HttpMethod.Post, $"{Global.SmoApiUrl}PO/InOutStore/")
                    {
                        Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
                    };

                    HttpResponseMessage response = client.Send(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        Alert("Chưa cập nhật được trạng thái lên SMO!", VCS.Areas.Alert.Alert.enumType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                Alert("Chưa cập nhật được trạng thái lên SMO!", VCS.Areas.Alert.Alert.enumType.Warning);
            }
        }

        public static DOSAPDataDto GetDetailDO(string number, string isOnlyInfo)
        {
            try
            {
                var token = LoginSmoApi();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var r = isOnlyInfo == "1" ? $"{Global.SmoApiUrl}PO/GetDO?doNumber={number}&isOnlyInfo={isOnlyInfo}" : $"{Global.SmoApiUrl}PO/GetDO?doNumber={number}";
                    var request = new HttpRequestMessage(HttpMethod.Get, r);
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


        public static Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms);
            }
        }
        #endregion

        #region Xử lý nhận diện biển số và cắt
        public static ResultDectect DetectLicensePlate(string imagePath, string croppedPath)
        {
            try
            {
                using (Bitmap bitmap = new Bitmap(imagePath))
                {
                    int originalWidth = bitmap.Width;
                    int originalHeight = bitmap.Height;
                    var inputTensor = PreprocessImage(bitmap);

                    var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor("images", inputTensor) };
                    using (var results = Global._session.Run(inputs))
                    {
                        var output = results.First().AsTensor<float>();
                        var bestPlateRect = ProcessOutput(output, originalWidth, originalHeight, 0.5f, 0.45f);

                        if (bestPlateRect == Rectangle.Empty)
                        {
                            return new ResultDectect();
                        }

                        using (Bitmap licensePlate = bitmap.Clone(bestPlateRect, bitmap.PixelFormat))
                        {
                            licensePlate.Save(croppedPath, ImageFormat.Jpeg);
                            var txt = ProcessImage(croppedPath);
                            return new ResultDectect
                            {
                                ImageCrop = new Bitmap(licensePlate),
                                LicensePlateNumber = txt
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi để dễ debug
                Console.WriteLine($"Error in DetectLicensePlate: {ex.Message}");
                return new ResultDectect();
            }
        }

        public static string ProcessImage(string imagePath)
        {
            try
            {
                using (Py.GIL())
                {
                    // Đọc ảnh bằng OpenCV
                    dynamic img = Global.cv2.imread(imagePath);

                    // Gọi hàm run_ocr
                    var result = Global.ocr_module.run_ocr(img);

                    // Kết quả trả về là tuple (text, boxes)
                    string licensePlateText = result[0].As<string>();
                    return licensePlateText;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"Error in ProcessImage: {ex.Message}");
                return null;
            }
        }

        private static Rectangle ProcessOutput(Tensor<float> output, int originalWidth, int originalHeight, float confThreshold = 0.5f, float iouThreshold = 0.45f)
        {
            var outputArray = new List<float[]>();
            int rowLength = output.Dimensions[1]; // Số features cho mỗi detection
            int numDetections = output.Dimensions[2];

            for (int i = 0; i < numDetections; i++)
            {
                var row = new float[rowLength];
                for (int j = 0; j < rowLength; j++)
                {
                    row[j] = output[0, j, i];
                }
                outputArray.Add(row);
            }

            float xFactor = (float)originalWidth / 640;
            float yFactor = (float)originalHeight / 640;

            var boxes = new List<Rectangle>();
            var scores = new List<float>();

            foreach (var row in outputArray)
            {
                float score = row[4] * 10;
                if (score >= confThreshold)
                {
                    float x = row[0];
                    float y = row[1];
                    float w = row[2];
                    float h = row[3];

                    int left = (int)((x - w / 2) * xFactor);
                    int top = (int)((y - h / 2) * yFactor);
                    int width = (int)(w * xFactor);
                    int height = (int)(h * yFactor);

                    left = Math.Max(0, left);
                    top = Math.Max(0, top);
                    width = Math.Min(width, originalWidth - left);
                    height = Math.Min(height, originalHeight - top);

                    boxes.Add(new Rectangle(left, top, width, height));
                    scores.Add(score);
                }
            }

            var indices = NonMaxSuppression(boxes, scores, iouThreshold);
            return indices.Count > 0 ? boxes[indices[0]] : Rectangle.Empty;
        }

        private static List<int> NonMaxSuppression(List<Rectangle> boxes, List<float> scores, float iouThreshold)
        {
            var indices = new List<int>();
            var sortedIndices = scores
                .Select((score, idx) => new { Score = score, Index = idx })
                .OrderByDescending(x => x.Score)
                .Select(x => x.Index)
                .ToList();

            while (sortedIndices.Any())
            {
                int currentIdx = sortedIndices.First();
                indices.Add(currentIdx);

                sortedIndices = sortedIndices.Skip(1)
                    .Where(idx => CalculateIoU(boxes[currentIdx], boxes[idx]) <= iouThreshold)
                    .ToList();
            }
            return indices;
        }

        private static float CalculateIoU(Rectangle box1, Rectangle box2)
        {
            int intersectionLeft = Math.Max(box1.Left, box2.Left);
            int intersectionTop = Math.Max(box1.Top, box2.Top);
            int intersectionRight = Math.Min(box1.Right, box2.Right);
            int intersectionBottom = Math.Min(box1.Bottom, box2.Bottom);

            if (intersectionRight < intersectionLeft || intersectionBottom < intersectionTop)
                return 0;

            float intersectionArea = (intersectionRight - intersectionLeft) *
                                     (intersectionBottom - intersectionTop);
            float box1Area = box1.Width * box1.Height;
            float box2Area = box2.Width * box2.Height;
            float unionArea = box1Area + box2Area - intersectionArea;

            return intersectionArea / unionArea;
        }

        private static DenseTensor<float> PreprocessImage(Bitmap bitmap)
        {
            const int targetSize = 640;
            Bitmap resized = new Bitmap(bitmap, new Size(targetSize, targetSize));
            var input = new DenseTensor<float>(new[] { 1, 3, targetSize, targetSize });

            BitmapData bitmapData = resized.LockBits(
                new Rectangle(0, 0, resized.Width, resized.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bitmapData.Scan0;
                for (int y = 0; y < targetSize; y++)
                {
                    for (int x = 0; x < targetSize; x++)
                    {
                        int index = (y * bitmapData.Stride) + (x * 3);
                        input[0, 0, y, x] = ptr[index + 2] / 255.0f; // R
                        input[0, 1, y, x] = ptr[index + 1] / 255.0f; // G
                        input[0, 2, y, x] = ptr[index] / 255.0f;     // B
                    }
                }
            }
            resized.UnlockBits(bitmapData);
            return input;
        }

        #endregion

        #region GetText Mapping
        private static readonly Dictionary<string, string> TypeMappings = new()
        {
            { "DCCH", "Di chuyển ra cửa hàng" }, { "DCNB", "Di chuyển nội bộ ngành" },
            { "XBTX", "Xuất bán tái xuất" }, { "XBND", "Xuất bán nội địa" },
            { "XTHG", "Xuất trả hàng gửi" }, { "MHGL", "Mua hàng gửi lại" },
            { "HHK", "Hàng hóa khác" }, { "KHLH", "Kế hoạch lấy hàng" },
            { "SUM", "Đơn hàng tổng" }, { "XDTH", "Xuất đổi trả hàng" },
            { "XHND", "Xuất hàng nội dung" }
        };

        public static string GetText(string type) => TypeMappings.GetValueOrDefault(type, string.Empty);
        #endregion

        #region Alert
        public static void Alert(string msg, Alert.enumType type)
        {
            VCS.Areas.Alert.Alert alert = new VCS.Areas.Alert.Alert();
            alert.ShowAlert(msg, type);
        }
        #endregion

        #region Upload ảnh lên server
        public static async void UploadImagesServer(List<string> imagePaths)
        {
            if (imagePaths.Count() == 0)
            {
                return;
            }
            HttpClient _client = new HttpClient();
            var content = new MultipartFormDataContent();
            foreach (var filePath in imagePaths)
            {
                var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(filePath));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                content.Add(fileContent, "files", System.IO.Path.GetFileName(filePath));
            }

            var response = await _client.PostAsync($"{Global.VcsUrl}/api/Header/UploadImage", content);
            if (!response.IsSuccessStatusCode)
            {
                Alert("Lưu ý! Chưa đẩy được thông tin ảnh lên server", VCS.Areas.Alert.Alert.enumType.Warning);
            }
            
        }
        #endregion

        public static string CryptographyMD5(string source)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] bytHash = MD5.HashData(buffer);
            string result = string.Empty;
            foreach (byte a in bytHash)
            {
                result += int.Parse(a.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
            }
            return result;
        }
    }

    public class ResultDectect
    {
        public Bitmap? ImageCrop { get; set; }
        public string? LicensePlateNumber { get; set; }
    }
}
