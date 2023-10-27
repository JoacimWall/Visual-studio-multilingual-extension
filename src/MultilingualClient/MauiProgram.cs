using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace MultilingualClient;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureServices()
            .ConfigureViewModels()
            .ConfigureViews()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("CascadiaCode.ttf", "CascadiaCode");
                
                fonts.AddFont("appicons.ttf", "AppIconFont");
            })
            .ConfigureMauiHandlers(_ =>
            {
#if MACCATALYST
                Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("Smart", (h, v) =>
                {
                    h.PlatformView.SmartQuotesType = UIKit.UITextSmartQuotesType.No;
                    h.PlatformView.SmartDashesType = UIKit.UITextSmartDashesType.No;
                    h.PlatformView.SmartInsertDeleteType = UIKit.UITextSmartInsertDeleteType.No;
                });
#endif
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

