using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VCS.SERVICE_IMAGE;
using Serilog;
string logFilePath = Path.Combine(AppContext.BaseDirectory, "logs", "log.txt");
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .CreateLogger();
IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .UseSerilog()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<WorkerOptions>(context.Configuration.GetSection("WorkerSettings"));
        services.Configure<ApiSettings>(context.Configuration.GetSection("ApiSettings"));
        services.Configure<ConnectionStrings>(context.Configuration.GetSection("ConnectionStrings"));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
