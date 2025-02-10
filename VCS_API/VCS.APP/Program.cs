using DMS.BUSINESS.Common;
using DMS.BUSINESS.Services.Auth;
using DMS.BUSINESS.Services.BU;
using DMS.BUSINESS.Services.HUB;
using DMS.CORE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using VCS.APP.Areas.CheckIn;
using VCS.APP.Services;
using VCS.APP.Utilities;

namespace VCS.APP
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
            
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            Application.Run(ServiceProvider.GetRequiredService<Areas.Login.Login>());
            PreloadDlls();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.json", optional: true)
                .AddEnvironmentVariables().Build();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Connection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, ServiceLifetime.Scoped);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddScoped<IAuthService, AuthService>();
            
            services.AddTransient<Areas.Login.Login>();
            services.AddTransient<Main>();
            services.AddTransient<CheckIn>();
            services.AddScoped<IWOrderService, WOrderService>();
            // services.AddTransient<IOrderService, OrderService>();
            //services.AddHttpClient();
        }
        static void PreloadDlls()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string[] dllFiles = Directory.GetFiles(exePath, "*.dll");

            foreach (string dll in dllFiles)
            {
                try
                {
                    Assembly.LoadFrom(dll);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load {dll}: {ex.Message}");
                }
            }
        }
    }
}