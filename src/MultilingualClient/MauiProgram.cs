using Microsoft.Extensions.Logging;

namespace MultilingualClient;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			
            .ConfigureServices()
            .ConfigureViewModels()
            .ConfigureViews()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("appicons.ttf", "AppIconFont");
            });

#if DEBUG
		builder.Logging.AddDebug();
        MultilingualClientGlobals.ConsoleWriteLineDebug("CacheDirectory   " + FileSystem.CacheDirectory);
        MultilingualClientGlobals.ConsoleWriteLineDebug("AppDataDirectory   " + FileSystem.AppDataDirectory);
#endif

        var app = builder.Build();
        //we must initialize our service helper before using it
        ServiceHelper.Initialize(app.Services);
		return app;
    }
}

