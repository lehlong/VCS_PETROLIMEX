using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using VCS.Areas.Login;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using LibVLCSharp.Shared;
using VCS.APP.Utilities;
using Microsoft.ML.OnnxRuntime;
using Python.Runtime;
using VCS.DbContext.Common;
using IWshRuntimeLibrary;
using VCS.Areas.Startup;

namespace VCS
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Application.ExecutablePath)).Length > 1)
            {
                MessageBox.Show("Ứng dụng đã đang chạy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (Startup s = new Startup())
            {
                s.Show();
                Application.DoEvents();
                try { InitializeSystem(); } catch (Exception ex) { MessageBox.Show($"Lỗi khởi tạo hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }
            using var host = CreateHostBuilder().Build();
            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContextForm>>().CreateDbContext();
            Application.Run(new Login(dbContext));
        }

        static void InitializeSystem()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(exePath);
            Core.Initialize();
            Global._libVLC = new LibVLC("--network-caching=50", "--live-caching=50", "--file-caching=50", "--drop-late-frames", "--skip-frames", "--clock-jitter=0", "--clock-synchro=0", "--no-audio", "--rtsp-tcp");
            Global._session = new InferenceSession(Path.Combine(exePath, "checkpoints", "license_plate_onnx", "model.onnx"));
            InitializePython(exePath);
            CreateDesktopShortcut();
            LoadUserConfig();
            using var host = CreateHostBuilder().Build();
            using var scope = host.Services.CreateScope();
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContextForm>>();
            using var dbContext = dbContextFactory.CreateDbContext();
            if (!dbContext.Database.CanConnect()) Console.WriteLine("Failed to connect to database.");
        }

        static void InitializePython(string exePath)
        {
            string pythonDll = Path.Combine(exePath, "LicensePlateService", "venv", "python312.dll");
            if (!System.IO.File.Exists(pythonDll)) throw new FileNotFoundException($"Không tìm thấy Python DLL: {pythonDll}");
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);
            if (!PythonEngine.IsInitialized) PythonEngine.Initialize();
            using (Py.GIL())
            {
                Global.np = Py.Import("numpy");
                Global.cv2 = Py.Import("cv2");
                string scriptPath = Path.Combine(exePath, "LicensePlateService", "app.py");
                if (!System.IO.File.Exists(scriptPath)) throw new FileNotFoundException($"Không tìm thấy app.py: {scriptPath}");
                dynamic sys = Py.Import("sys");
                sys.path.append(Path.GetDirectoryName(scriptPath));
                Global.ocr_module = Py.Import("app");
            }
        }

        static void CreateDesktopShortcut()
        {
            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "VCS.lnk");
            if (System.IO.File.Exists(shortcutPath)) return;
            try
            {
                var shell = new WshShell();
                var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = Application.ExecutablePath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                shortcut.Description = "VCS System";
                shortcut.IconLocation = Application.ExecutablePath;
                shortcut.Save();
            }
            catch (Exception ex) { MessageBox.Show($"Không thể tạo shortcut phần mềm: {ex.Message}"); }
        }

        static void LoadUserConfig()
        {
            try
            {
                var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
                Global.SmoApiUsername = config["Setting:SmoApiUsername"];
                Global.SmoApiPassword = config["Setting:SmoApiPassword"];
                Global.SmoApiUrl = config["Setting:SmoApiUrl"];
                Global.PathSaveFile = config["Setting:PathSaveFile"];
                Global.VcsUrl = config["Setting:VcsUrl"];
                Global.CropWidth = string.IsNullOrEmpty(config["Setting:CropImagesWidth"]) ? 0 : Math.Max(0, Convert.ToUInt32(config["Setting:CropImagesWidth"]));
                Global.CropHeight = string.IsNullOrEmpty(config["Setting:CropImagesHeight"]) ? 0 : Math.Max(0, Convert.ToUInt32(config["Setting:CropImagesHeight"]));
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi khởi tạo cấu hình: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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
