using Serilog.Core;
using Serilog.Events;

namespace SimpleToDoList;

public class CustomSerilogEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent.Properties.TryGetValue("SourceContext", out var sourceContextValue) &&
            sourceContextValue is ScalarValue scalarValue &&
            scalarValue.Value is string fullTypeName)
        {
            // Get only the class name from the full type name
            var className = fullTypeName.Substring(fullTypeName.LastIndexOf('.') + 1);
            var classNameProperty = new LogEventProperty("ClassName", new ScalarValue(className));

            // Replace the "SourceContext" with the shortened "ClassName"
            logEvent.AddOrUpdateProperty(classNameProperty);
        }
    }
}