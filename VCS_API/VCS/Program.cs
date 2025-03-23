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
                    "--network-caching=50", 
                    "--live-caching=50", 
                    "--file-caching=50",
                    "--drop-late-frames",
                    "--skip-frames",
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
                // Lấy đường dẫn Python từ biến môi trường, hoặc sử dụng đường dẫn mặc định
                string pythonPath = $"{Environment.CurrentDirectory}\\LicensePlateService\\venv";
                string pythonDll = Path.Combine(pythonPath, "python312.dll");

                if (!File.Exists(pythonDll))
                {
                    throw new FileNotFoundException($"Không tìm thấy thư viện Python: {pythonDll}");
                }

                // Cài đặt biến môi trường
                Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);

                if (!PythonEngine.IsInitialized)
                {
                    PythonEngine.Initialize();
                }

                using (Py.GIL())
                {
                    // Import các thư viện cần thiết
                    Global.np = Py.Import("numpy");
                    Global.cv2 = Py.Import("cv2");

                    // Import module chứa hàm OCR
                    string scriptPath = $"{Environment.CurrentDirectory}\\LicensePlateService\\app.py";
                    string scriptDir = Path.GetDirectoryName(scriptPath);

                    if (!File.Exists(scriptPath))
                    {
                        throw new FileNotFoundException($"Không tìm thấy tệp Python OCR script: {scriptPath}");
                    }

                    dynamic sys = Py.Import("sys");
                    sys.path.append(scriptDir);
                    Global.ocr_module = Py.Import("app");
                }
            }
            catch (FileNotFoundException fileEx)
            {
                Console.WriteLine($"Lỗi tệp: {fileEx.Message}");
            }
            catch (PythonException pyEx)
            {
                Console.WriteLine($"Lỗi từ Python: {pyEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định: {ex.Message}");
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
