// SPDX-License-Identifier: MIT

/*
MIT License
Copyright (c) 2023 dSPACE GmbH, Germany, Contributed by Carsten Igel <CIgel@dspace>
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/


using Avalonia.Logging;
using Microsoft.Extensions.Logging;

namespace Avalonia.Extensions.Hosting;

internal class AvaloniaLoggerSink : ILogSink
{
    private readonly ILogger<AvaloniaLoggerSink> logger;
    private IReadOnlyCollection<string> selectedAreas;

    public AvaloniaLoggerSink(ILogger<AvaloniaLoggerSink> logger, params string[] areas)
    {
        this.logger = logger;
        this.selectedAreas = areas ?? Array.Empty<string>();
    }

    bool ILogSink.IsEnabled(LogEventLevel level, string area)
    {
        return this.logger.IsEnabled(FromLogEventLevel(level)) && (this.selectedAreas.Contains(area) || this.selectedAreas.Count == 0);
    }

    void ILogSink.Log(LogEventLevel level, string area, object? source, string messageTemplate)
    {
        LogLevel concreteLevel = FromLogEventLevel(level);
        if (this.logger.IsEnabled(concreteLevel))
        {
            string eventId = $"AvaloniaHost[{area}]";
            if (source is not null)
            {
                eventId = $"{eventId}+{Convert.ToString(source)}";
            }

            this.logger.Log(concreteLevel, new EventId(1, eventId), messageTemplate);
        }
    }

    void ILogSink.Log(LogEventLevel level, string area, object? source, string messageTemplate, params object?[] propertyValues)
    {
        LogLevel concreteLevel = FromLogEventLevel(level);
        if (this.logger.IsEnabled(concreteLevel))
        {
            string eventId = $"AvaloniaHost[{area}]";
            if (source is not null)
            {
                eventId = $"{eventId}+{Convert.ToString(source)}";
            }

            this.logger.Log(concreteLevel, new EventId(1, eventId), messageTemplate, propertyValues);
        }
    }

    private static LogLevel FromLogEventLevel(LogEventLevel eventLevel)
    {
        return eventLevel switch
        {
            LogEventLevel.Verbose => LogLevel.Trace,
            LogEventLevel.Debug => LogLevel.Debug,
            LogEventLevel.Information => LogLevel.Information,
            LogEventLevel.Warning => LogLevel.Warning,
            LogEventLevel.Error => LogLevel.Error,
            LogEventLevel.Fatal => LogLevel.Critical,
            _ => LogLevel.None,
        };
    }
}