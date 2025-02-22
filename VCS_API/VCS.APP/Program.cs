using DMS.CORE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VCS.APP.Areas.Login;

namespace VCS.APP
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Load configuration
            var configuration = LoadConfiguration();
            string connectionString = configuration.GetConnectionString("Connection");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContextForm>();
            optionsBuilder.UseSqlServer(connectionString);

            var dbContext = new AppDbContextForm(optionsBuilder.Options);
            Application.Run(new Login(dbContext));
        }

        static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}