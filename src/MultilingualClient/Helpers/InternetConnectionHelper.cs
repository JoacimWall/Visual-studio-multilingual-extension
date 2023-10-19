namespace MultilingualClient.Helpers;
public static class InternetConnectionHelper
{
    public static bool InternetAccess()
    {
        var current = Connectivity.NetworkAccess;

        if (current == NetworkAccess.Internet)
        {
            return true;
        }

        return false;
    }
    private static DateTime lastmess;
    public static bool InternetAccess(IDialogService dialogService, bool showError = true)
    {
        var current = Connectivity.NetworkAccess;

        if (current == NetworkAccess.Internet)
        {
            return true;
        }
        if (showError)
        {
            //Check if last mess was recently so we dont show to many mess
            if (lastmess > DateTime.Now)
                return false;

            dialogService.ShowAlertAsync("Internet_Connection_Missing", "", "Ok");
            lastmess = DateTime.Now.AddSeconds(5);
        }
        return false;
    }
    public static bool OnWifi(IDialogService dialogService)
    {
        var profiles = Connectivity.ConnectionProfiles;
        if (profiles.Contains(ConnectionProfile.WiFi))
        {
            return true;
        }

        return false;
    }
}
