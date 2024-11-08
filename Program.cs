using Avalonia;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SimpleToDoList.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Runtime.Versioning;
using System.Runtime.CompilerServices;
using Lemon.Hosting.AvaloniauiDesktop;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Extensions.Hosting;
using System.Text.Json;
using Loki.Extensions.Logging;
using Karambolo.Extensions.Logging.File.Json;

namespace SimpleToDoList;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("macos")]
    [RequiresDynamicCode("Calls Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder()")]
    public static void Main(string[] args)
    {

        var hostBuilder = Host.CreateApplicationBuilder();

        // config IConfiguration
        hostBuilder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddCommandLine(args);



        // config ILogger
        hostBuilder.Services.AddLogging(logging =>
        {
            logging.ClearProviders(); // Clears all logging providers
            logging.AddJsonConsole(options =>
            {
                options.JsonWriterOptions = new JsonWriterOptions
                {
                    Indented = false
                };
                options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
            });
            logging.AddJsonFile(options =>
            {
                options.TextBuilder = new JsonFileLogEntryTextBuilder(new JsonFileLogFormatOptions
                {
                    JsonWriterOptions = new JsonWriterOptions
                    {
                        Indented = false // Disable JSON indentation
                    }
                });
            });
            logging.AddLoki(options =>
            {
                options.ApplicationName = "TodoList.App";
            });
        });
        // add some services

        RunAppWithServiceProvider(hostBuilder, args);
    }

    public static AppBuilder ConfigAvaloniaAppBuilder(AppBuilder appBuilder) =>
        appBuilder
            .UsePlatformDetect()
            .WithInterFont();
    //.LogToMySink(LogEventLevel.Verbose);


    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("macos")]
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static void RunAppWithServiceProvider(HostApplicationBuilder hostBuilder, string[] args)
    {
        // add avaloniaui application and config AppBuilder
        hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(ConfigAvaloniaAppBuilder);
        // add MainWindowViewModelWithParams
        hostBuilder.Services.AddSingleton<MainViewModel>();
        // build host
        var appHost = hostBuilder.Build();

        Avalonia.Logging.Logger.Sink = new AvaloniaLoggerSink(new Logger<AvaloniaLoggerSink>(appHost.Services.GetRequiredService<ILoggerFactory>()));
        // run app
        appHost.RunAvaloniauiApplication(args);
    }

}


