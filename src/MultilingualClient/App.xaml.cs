using CommunityToolkit.Mvvm.Messaging;
using UIKit;

namespace MultilingualClient;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

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

