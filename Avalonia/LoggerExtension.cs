using Avalonia;
using Avalonia.Logging;
using Microsoft.Extensions.Logging;

namespace Avalonia.Extensions.Hosting;
public static class MyLogExtensions
{
    public static AppBuilder LogToMySink(this AppBuilder builder,
        LogEventLevel level = LogEventLevel.Warning,
        params string[] areas)
    {
        Logger.Sink = new AvaloniaLoggerSink(new Logger<AvaloniaLoggerSink>(new LoggerFactory()));
        return builder;
    }
}