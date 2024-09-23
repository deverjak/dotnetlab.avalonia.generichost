using Avalonia;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SimpleToDoList.ViewModels;
using SimpleToDoList.Views;
using Microsoft.Extensions.Configuration;
using SimpleToDoList.Avalonia;

namespace SimpleToDoList;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) =>
        BuildAvaloniaApp(args)
           .StartWithClassicDesktopLifetime(args);


    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .Build();

        // Store the ServiceProvider for later use
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .WithServiceProvider(host.Services)
            .LogToTrace();
    }

}


