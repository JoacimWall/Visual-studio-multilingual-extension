using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using NavigationPage = Microsoft.Maui.Controls.NavigationPage;
using Microsoft.Maui.Controls.PlatformConfiguration;


namespace MultilingualClient.Views;

public class BaseContentPage : ContentPage
{
    public BaseContentPage()
    {
        NavigationPage.SetBackButtonTitle(this, "");
        NavigationPage.SetHasNavigationBar(this, false);
        //BackgroundColor = AppColors.PageBackgroundColor;
        //TODO:MAUI_INFO finns nuget för detta. bugg Vi kan inte använda Shell TilelView då orginal back pilen visas på IOS i 1 sec innan den döljs
        //https://www.nuget.org/packages/PureWeen.Maui.FixesAndWorkarounds kan fixa detta kanske
        Shell.SetNavBarIsVisible(this, false);
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsVisible = false });
        //döljer warnings då vi kräver IOS 12.4 så det är lugnt  
        this.HideSoftInputOnTapped = true;
      
        On<iOS>().SetUseSafeArea(true);
        
    }
    public Color StatusbarBackgroundColor { get; set; } = AppColors.StatusbarBackgroundColor;

    //public void SubscribeToScrollToTopMessage()
    //{  
    //    WeakReferenceMessenger.Default.Register<TmScrollTabbarPageToTopMessage>(this, (r, m) =>
    //    {
    //        if (m.Value == this.GetType().ToString())
    //            ScrollToTopAction();
    //    });
    //}

    public virtual void ScrollToTopAction() { }
    protected override bool OnBackButtonPressed()
    {
        if (BindingContext != null)
            (BindingContext as BaseViewModel)?.GoBack();


        return base.OnBackButtonPressed();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        bool darkStatusBarBackground = true;
        if (this.StatusbarBackgroundColor == AppColors.WhiteColor
                || StatusbarBackgroundColor == AppColors.Gray100Color
                || StatusbarBackgroundColor == AppColors.Primary200Color)
            darkStatusBarBackground = false;


#pragma warning disable CA1416
        //if (darkStatusBarBackground)
        //    this.Behaviors.Add(new CommunityToolkit.Maui.Behaviors.StatusBarBehavior { StatusBarColor = StatusbarBackgroundColor, StatusBarStyle = CommunityToolkit.Maui.Core.StatusBarStyle.LightContent });
        //else
        //    this.Behaviors.Add(new CommunityToolkit.Maui.Behaviors.StatusBarBehavior { StatusBarColor = StatusbarBackgroundColor, StatusBarStyle = CommunityToolkit.Maui.Core.StatusBarStyle.DarkContent });
#pragma warning restore CA1416



        if (BindingContext != null)
        {
            await (BindingContext as BaseViewModel)?.OnAppearingAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                (BindingContext as BaseViewModel)?.OnAppearing();
            });

        }
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext != null)
        {
            await (BindingContext as BaseViewModel)?.OnDisappearingAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                (BindingContext as BaseViewModel)?.OnDisappearing();
            });
        }
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext != null)
        {
            (BindingContext as BaseViewModel)?.OnNavigatedTo(args);
           
        }
    }
}