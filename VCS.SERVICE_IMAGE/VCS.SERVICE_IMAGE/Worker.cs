namespace VCS.SERVICE_IMAGE
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.IO.Compression;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net.WebSockets;
    using Newtonsoft.Json.Linq;
    using System.Net.Http.Json;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _connectionString;
        private readonly string _apiUrl;
       


        public Worker(ILogger<Worker> logger, IOptions<ConnectionStrings> connectionStrings, IOptions<ApiSettings> apiSettings, IOptions<WorkerOptions> workerOptions)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _connectionString = connectionStrings.Value.DefaultConnection;
            _apiUrl = apiSettings.Value.UploadUrl;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("<-------------------------------------------------->");
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            var Soucefile = Directory.GetParent(AppContext.BaseDirectory).FullName;
            var a = AppContext.BaseDirectory;
            var Disk = Path.GetPathRoot(Soucefile);

            DriveInfo driveInfo = new DriveInfo($"{Disk[0]}");
            var FreeMemory = driveInfo.AvailableFreeSpace / 1024 / 1024 / 1024;
            string Pathbaseapp = Soucefile.Replace("VCS.SERVICE.IMAGE", "VCS.APP");
            string folderPathjson = @$"{Pathbaseapp}\appsettings.json";
           
           
            while (!stoppingToken.IsCancellationRequested)
            {
                string jsonContent = File.ReadAllText(folderPathjson);
                JObject jsonObject = JObject.Parse(jsonContent);
                string pathSaveFile = (string)jsonObject["Setting"]["PathSaveFile"];
                double TimeLoop = (double)jsonObject["Setting"]["TimeService"];
                var (filePaths, status) = await GetFilePathsFromDatabase();
                try
                {

                    // doc json
                   
                    var chunk =filePaths.Chunk(10);
                    if (filePaths.Count > 0)
                    {
                      

                        foreach (var item in chunk)
                        {
                            await UploadFilesToApi(item);
                 

                        }


                    }
                    else
                    {
                        _logger.LogInformation("Không có file hợp lệ để upload.");
                    }
                    //nén file
                    DateTime now = DateTime.Now;
                    string sourceDirectory = @$"{pathSaveFile}";
                    var fileCompress = @$"{Disk[0]}:\CompressedfileImageVCS";
                    string zipFilePath = @$"{Disk[0]}:\CompressedfileImageVCS\{now.Year}-{now.Month.ToString("00")}-{now.Day.ToString("00")}-{now.Hour}-{now.Minute}.zip";


                    if (FreeMemory < 10 && filePaths.Count == 0 && status == true)
                    {
                        if (!Directory.Exists(fileCompress))
                        {
                            Directory.CreateDirectory(fileCompress);
                        }

                        if (Directory.Exists(sourceDirectory))
                        {
                            ZipFile.CreateFromDirectory(sourceDirectory, zipFilePath, CompressionLevel.Optimal, true);
                            Directory.Delete(sourceDirectory, true);
                        }

                    }

                    _logger.LogInformation("{folderPathjson}", folderPathjson);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi trong quá trình thực thi worker.");
                  
                }

             
                await Task.Delay(TimeSpan.FromMinutes(TimeLoop), stoppingToken);
            }
        }

        private async Task<(List<string>,bool)> GetFilePathsFromDatabase()
        {
            List<string> filePaths = new List<string>();
            string query = "SELECT PATH FROM T_BU_IMAGE WHERE IS_SYNC=0";
            bool status = true;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        string path = reader["PATH"].ToString();
                        if (!string.IsNullOrEmpty(path) && File.Exists(path))
                        {
                            filePaths.Add(path);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    
                    _logger.LogError(ex, "Lỗi khi lấy dữ liệu từ SQL Server.");
                    status = false;
                  
                }
            }
            return (filePaths,status);
        }
        private async Task UpdateIsSyncStatus(string ConditionSql)
        {
            string query = "UPDATE T_BU_IMAGE SET IS_SYNC = 1 WHERE " +ConditionSql;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                  
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi cập nhật IS_SYNC trong SQL Server.");
                }
            }
        }

        
        private async Task UploadFilesToApi(string[] filePaths)
        {
            using (MultipartFormDataContent formData = new MultipartFormDataContent())
            {
                try
                {
                    if (filePaths.Length > 0)
                    {
                        var content = "";
                        foreach (var filePath in filePaths)
                        {
                            if (File.Exists(filePath))
                            {
                                byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                                ByteArrayContent fileContent = new ByteArrayContent(fileBytes);
                                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                                formData.Add(fileContent, "files", Path.GetFileName(filePath));

                                // Thêm đường dẫn file vào form data
                                formData.Add(new StringContent(filePath), "filePaths");
                                content += " PATH= " + "'"+filePath+"'" + " or";
                            }
                        }
                        content = content.Remove(content.Length - 2);

                        HttpResponseMessage response = await _httpClient.PostAsync(_apiUrl, formData);

                        if (response.IsSuccessStatusCode)
                        {
                            _logger.LogInformation($"Upload thành công: {response.StatusCode}");
                            UpdateIsSyncStatus(content);
                        }
                        else
                        {
                            _logger.LogError($"Lỗi khi upload: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi gọi API.");
                }
            }
        }

    }

}
public class WorkerOptions
{
    public int IntervalMinutes { get; set; }
}
public class ApiSettings
{
    public string UploadUrl { get; set; }
}
public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
}
public class StatusSQL
{
    public bool Status { get; set; }
}

//{
//    "Setting": {
//        "SmoApiUsername": "smoapi",
//    "SmoApiPassword": "smoapi123",
//    "SmoApiUrl": "https://smoapiuat.petrolimex.com.vn/api/",
//    "PathSaveFile": "D://AttachImageVCS",
//    "DetectApiUrl": "http://localhost:5000/api/detect",
//    "DetectFilePath": "D:\\license_detec.exe"
//    },
//  "ConnectionStrings": {
//        "Connection": "Server=sso.d2s.com.vn,1608;Database=VCS_PETROLIMEX;User ID=sa;Password=sa@d2s.com.vn; TrustServerCertificate=true; MultipleActiveResultSets=true"
//  }
//}
