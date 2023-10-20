using CommunityToolkit.Mvvm.Messaging;
using UIKit;

namespace MultilingualClient;

public partial class App : Application
{
    ISettingsService settingsService;

    public App()
	{
		InitializeComponent();
        //this.settingsService = ServiceHelper.GetService<ISettingsService>();
        MultilingualClientGlobals.App = this;
        MainPage = new StartupView();
	}
    private async Task<bool> initApp(bool fromResume)
    {
        
        App.Current.MainPage = new AppShell();//
        
        return true;
    }
    protected async override void OnStart()
    {
        //then we don't want the Initnavigation to do the same thing. the app crash on IOS works on Android  
        //if (((Shell)(App.Current.MainPage)).CurrentPage is StartupView)
        await initApp(false);
        // Get information about the source directory
       
        this.settingsService = ServiceHelper.GetService<ISettingsService>();
        MultilingualClientGlobals.CurrentRootPath = "/Users/joacimwall/GitRepos/NbcMaui/LiberoClub/";
        this.settingsService.ReInit(MultilingualClientGlobals.CurrentRootPath);
        MultilingualClientGlobals.ApplicationState = ApplicationState.Active;
    }
    protected override async void OnResume()
    {
        MultilingualClientGlobals.ApplicationState = ApplicationState.Active;

    }
    protected override async void OnSleep()
    {

        // WeakReferenceMessenger.Default.Send(new AppSleepMessage(true));



        MultilingualClientGlobals.ApplicationState = ApplicationState.InActive;
    }

}

