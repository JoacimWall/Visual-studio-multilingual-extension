namespace MultilingualClient.Views;

public static class ViewsExtensions
{
    public static MauiAppBuilder ConfigureViews(this MauiAppBuilder builder)
    {
        
        
        builder.Services.AddTransient<MainPage>();
        return builder;
    }
}

