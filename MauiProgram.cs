using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;

namespace Gradpath
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMauiHandlers(handlers =>
                {
                    // Handlers configuration if needed
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Configure the splash screen
            builder.ConfigureMauiHandlers(handlers =>
            {
                // Set the splash screen image and color
                handlers.AddHandler(typeof(ISplashScreen), typeof(SplashScreenHandler));
            });

            return builder.Build();
        }
    }

    public class SplashScreenHandler : ISplashScreen
    {
        public ImageSource Image => ImageSource.FromFile("Resources/Images/splash_screen_image.png");
        public Color BackgroundColor => Colors.Black;
    }

    public interface ISplashScreen
    {
        ImageSource Image { get; }
        Color BackgroundColor { get; }
    }
}
