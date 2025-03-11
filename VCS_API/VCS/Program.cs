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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var dbContextFactory = services.GetRequiredService<IDbContextFactory<AppDbContextForm>>();
                using var dbContext = dbContextFactory.CreateDbContext();
                Application.Run(new Login(dbContext));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chạy chương trình: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static bool IsApplicationRunning()
        {
            // Lấy tên ứng dụng của bạn
            string appName = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);

            // Kiểm tra nếu đã có một instance của ứng dụng này đang chạy
            Process[] processes = Process.GetProcessesByName(appName);

            // Nếu có hơn một tiến trình, thì ứng dụng đã chạy
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
