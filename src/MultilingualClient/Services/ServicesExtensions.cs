

namespace MultilingualClient.Services;

public static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        //Singelton
        builder.Services.AddSingleton<IDialogService, DialogService>();
        
        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IStatusPadLoger, StatusPadLoger>();
        //Transient
        // builder.Services.AddTransient<IRestClient, RestClient>();

        return builder;
    }
}

