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
using VCS.Areas.Alert;

namespace VCS.APP.Services
{
    public class CommonService
    {
        public static TblAdAccount CurrentUser { get; private set; }
        public static Dictionary<string, bool> UserPermissions { get; private set; } = new Dictionary<string, bool>();
      
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
                Global.VcsUrl = config["Setting:VcsUrl"];
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
        public static string GetText(string type)
        {
            if (type == "DCCH")
            {
                return "Di chuyển ra cửa hàng";
            }
            else if (type == "DCNB")
            {
                return "Di chuyển nội bộ ngành";
            }
            else if (type == "XBTX")
            {
                return "Xuất bán tái xuất";
            }
            else if (type == "XBND")
            {
                return "Xuất bán nội địa";
            }
            else if (type == "XTHG")
            {
                return "Xuất trả hàng gửi";
            }
            else if (type == "MHGL")
            {
                return "Mua hàng gửi lại";
            }
            else if (type == "HHK")
            {
                return "Hàng hóa khác";
            }
            else if (type == "KHLH")
            {
                return "Kế hoạch lấy hàng";
            }
            else if (type == "SUM")
            {
                return "Đơn hàng tổng";
            }
            else if (type == "XDTH")
            {
                return "Xuất đổi trả hàng";
            }
            else if (type == "XHND")
            {
                return "Xuất hàng nội dụng";
            }
            return string.Empty;
        }
        public static void Alert(string msg, Alert.enumType type)
        {
            VCS.Areas.Alert.Alert alert = new VCS.Areas.Alert.Alert();
            alert.ShowAlert(msg, type);
        }

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
    }
}
