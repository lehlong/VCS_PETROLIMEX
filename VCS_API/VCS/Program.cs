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
                    "--no-sub-autodetect-file",
                    "--no-snapshot-preview", 
                    "--no-spu",
                    "--no-video-title-show",
                    "--no-osd",
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
