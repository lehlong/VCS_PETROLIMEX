namespace VCS.SERVICE_IMAGE
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

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
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    List<string> filePaths = await GetFilePathsFromDatabase();
                    var chunk =filePaths.Chunk(10);
                    if (filePaths.Count > 0)
                    {
                        //await UploadFilesToApi(filePaths);
                        
                        foreach (var item in chunk)
                        {
                            await UploadFilesToApi(item);
                            //var a= item.ToArray();
                        
                        }

                    }
                    else
                    {
                        _logger.LogInformation("Không có file hợp lệ để upload.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi trong quá trình thực thi worker.");
                }

                // Chờ thời gian được cấu hình từ appsettings.json trước khi chạy lại
                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }

        private async Task<List<string>> GetFilePathsFromDatabase()
        {
            List<string> filePaths = new List<string>();
            string query = "SELECT PATH FROM T_BU_IMAGE WHERE IS_SYNC=0";

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
                }
            }
            return filePaths;
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