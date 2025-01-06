using DMS.BUSINESS.Services.Auth;
using DMS.CORE;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using VCS.APP.Areas.CheckIn;
using VCS.APP.Utilities;

namespace VCS.APP
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            Application.Run(ServiceProvider.GetRequiredService<Areas.Login.Login>());
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")))
               .AddJsonFile("appsettings.json")
               .Build();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Connection"));
            }, ServiceLifetime.Scoped);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddScoped<IAuthService, AuthService>();
            
            services.AddTransient<Areas.Login.Login>();
            services.AddTransient<Main>();
            services.AddTransient<CheckIn>();
        }
    }
}