using Avalonia;

namespace SimpleToDoList.Avalonia;


public static class AppBuilderExtensions
{
    public static AppBuilder WithServiceProvider(this AppBuilder builder, IServiceProvider serviceProvider)
    {
        builder.AfterSetup(_ =>
        {
            if (Application.Current is App app)
            {
                app.SetServiceProvider(serviceProvider);
            }
        });
        return builder;
    }
}