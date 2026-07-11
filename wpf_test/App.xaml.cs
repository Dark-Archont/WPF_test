using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmartMealWpf.Services;
using SmartMealWpf.ViewModels;

namespace SmartMealWpf;

public partial class App : Application
{
    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = CreateHostBuilder().Build();
        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        Log.CloseAndFlush();
        base.OnExit(e);
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .UseSerilog((_, _, loggerConfig) =>
            {
                var logDir = Path.Combine(AppContext.BaseDirectory, "logs");
                Directory.CreateDirectory(logDir);

                loggerConfig
                    .MinimumLevel.Information()
                    .WriteTo.File(
                        path: Path.Combine(logDir, "test-sms-wpf-app-.log"),
                        rollingInterval: RollingInterval.Day,
                        // Serilog appends the date automatically; the format matches test-sms-wpf-app-yyyyMMdd.log
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}");
            })
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(AppContext.BaseDirectory)
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IEnvironmentVariableService, EnvironmentVariableService>();
                services.AddSingleton<CommentStoreService>();
                services.AddSingleton<IAppLogger>(sp =>
                    new SerilogAppLogger(Serilog.Log.Logger));

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
            });
    }
}
