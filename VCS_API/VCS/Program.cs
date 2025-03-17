using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DMS.CORE;
using VCS.Areas.Login;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using LibVLCSharp.Shared;
using VCS.APP.Utilities;
using Microsoft.ML.OnnxRuntime;
using Python.Runtime;

namespace VCS
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            if (IsApplicationRunning())
            {
                MessageBox.Show("Ứng dụng đã đang chạy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(exePath);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InitializeLibVLC();
            LoadOnnxModel();
            InitializePython();
            LoadDbContext();
        }
        static void LoadDbContext()
        {
            try
            {

                var host = CreateHostBuilder().Build();
                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                var dbContextFactory = services.GetRequiredService<IDbContextFactory<AppDbContextForm>>();
                using var dbContext = dbContextFactory.CreateDbContext();
                Application.Run(new Login(dbContext));
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải DbContext: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static void InitializeLibVLC()
        {
            try
            {
                Core.Initialize();
                Global._libVLC = new LibVLC(
                    "--network-caching=30", 
                    "--live-caching=30", 
                    "--file-caching=30",
                    "--clock-jitter=0", 
                    "--clock-synchro=0",
                    "--no-audio",
                    "--rtsp-tcp");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi tạo LibVLC: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static void LoadOnnxModel()
        {
            try
            {
                Global._session = new InferenceSession(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models", "model.onnx"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải model ONNX: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static bool IsApplicationRunning()
        {
            string appName = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            Process[] processes = Process.GetProcessesByName(appName);
            return processes.Length > 1;
        }
        static void InitializePython()
        {
            try
            {
                // Đường dẫn đến Python environment của bạn
                string pythonPath = $"{Environment.CurrentDirectory}\\LicensePlateService\\venv";
                Environment.SetEnvironmentVariable("PYTHONNET_PYDLL",
                    Path.Combine(pythonPath, "python312.dll"));

                PythonEngine.Initialize();
                using (Py.GIL())
                {
                    // Import các thư viện cần thiết
                    Global.np = Py.Import("numpy");
                    Global.cv2 = Py.Import("cv2");

                    // Import module chứa hàm OCR
                    string scriptPath = $"{Environment.CurrentDirectory}\\LicensePlateService\\app.py";
                    dynamic sys = Py.Import("sys");
                    sys.path.append(Path.GetDirectoryName(scriptPath));
                    Global.ocr_module = Py.Import("app");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi tạo Python: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }
        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContextFactory<AppDbContextForm>(options =>
                        options.UseSqlServer(context.Configuration.GetConnectionString("Connection")));
                });
    }
}
