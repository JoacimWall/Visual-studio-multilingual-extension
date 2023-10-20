using System;
using UIKit;

namespace MultilingualClient;

public class MultilingualClientGlobals
{
    public static DateTime LastOnSleep { get; set; }
    public static DateTime LastOnStart { get; set; }
    public static DateTime LastOnResume { get; set; }
    public static string CurrentRootPath { get; set; }
    public static Application App { get; set; }

    public static ApplicationState ApplicationState { get; set; }
    public MultilingualClientGlobals()
    {
    }
    #region AppCenter Crashes
    public static void ReportErrorToAppCenter(Exception ex, Dictionary<string, string> info)
    {

#if DEBUG
        ConsoleWriteLineDebug("HANDLED ERROR " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);

#endif
        //#if RELEASE
        Dictionary<string, string> logInfo = new Dictionary<string, string>();
        logInfo.Add("AppPackageName", AppInfo.PackageName);
        logInfo.Add("LastOnStart", LastOnStart.ToString());
        logInfo.Add("LastOnSleep", LastOnSleep.ToString());
        logInfo.Add("LastOnResume", LastOnResume.ToString());
        foreach (var item in info)
        {
            logInfo.Add(item.Key, item.Value);
        }

        //Crashes.TrackError(ex, logInfo);
        //#endif
    }
    public static void ReportErrorToAppCenter(Exception ex, string info)
    {

#if DEBUG
        ConsoleWriteLineDebug("HANDLED ERROR " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);

#endif
        //#if RELEASE
        Dictionary<string, string> logInfo = new Dictionary<string, string>();
        logInfo.Add("AppPackageName", AppInfo.PackageName);
        logInfo.Add("LastOnStart", LastOnStart.ToString());
        logInfo.Add("LastOnSleep", LastOnSleep.ToString());
        logInfo.Add("LastOnResume", LastOnResume.ToString());
        if (!string.IsNullOrEmpty(info))
            logInfo.Add("Info", info);


        //Crashes.TrackError(ex, logInfo);
        //#endif
    }
    #endregion
    public static void ConsoleWriteLineDebug(Exception ex)
    {
#if DEBUG
        MainThread.BeginInvokeOnMainThread(() => { Console.WriteLine(ex); });
#endif
    }
    public static void ConsoleWriteLineDebug(string stringInfo)
    {
#if DEBUG
        MainThread.BeginInvokeOnMainThread(() => { Console.WriteLine(stringInfo); });
#endif

    }
}
public enum ApplicationState
{
    NotSet,
    Active,
    InActive,
}
