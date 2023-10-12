using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace ModManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.ConfigureLifecycleEvents(lifecycle =>
            {
                lifecycle.AddWindows(windows =>
                {
                    windows.OnWindowCreated(xamlWindow =>
                    {
                        var window = xamlWindow as MauiWinUIWindow;

                        window.ExtendsContentIntoTitleBar = true; 
                    });
                });
            });

            builder.Logging.AddDebug();

            return builder.Build();
        }
    }
}