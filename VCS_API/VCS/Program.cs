using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DMS.CORE;
using VCS.Areas.Login;
using Microsoft.Extensions.Configuration;

namespace VCS
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
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
