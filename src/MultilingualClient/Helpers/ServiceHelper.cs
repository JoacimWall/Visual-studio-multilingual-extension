namespace MultilingualClient.Helpers;

public static class ServiceHelper
{

    public static IServiceProvider Services { get; private set; }

    public static void Initialize(IServiceProvider serviceProvider) =>
        Services = serviceProvider;

    public static T GetService<T>() => Services.GetService<T>();
    //    public static TService GetService<TService>() => Current.GetService<TService>();

    //    public static IServiceProvider Current =>
    //#if ANDROID
    //        MauiApplication.Current.Services;
    //#elif IOS || MACCATALYST 
    //        MauiUIApplicationDelegate.Current.Services;
    //#else
    //        null;
    //#endif
    
}
