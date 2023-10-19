

namespace MultilingualClient.ViewModels;

public interface IAnalyticsService
{
    void LogEvent(string eventId);
    void LogEvent(string eventId, string paramName, string value);
    void LogEvent(string eventName, IDictionary<string, object> parameters);
    void SetUserProperty(string name, string value);
    void SetCurrentScreen(string screenName, string screenClass);
    //Info: Denna ska returnera true ifall den aktuella vm tillhör en tabbpage
    //så att vi får med den sidan i statistiken för visade view 
    bool IsTabbedPageViewModel(string viewModelName);

}

public partial class BaseViewModel : ObservableObject, IQueryAttributable
{
    protected readonly IDialogService DialogService;
    protected readonly IAnalyticsService AnalyticsService;
    //protected readonly ISettingsService SettingsService;
    public BaseViewModel()
    {
        DialogService =  ServiceHelper.GetService<IDialogService>();
        //AnalyticsService = ServiceHelper.GetService<IAnalyticsService>();
        //SettingsService =  ServiceHelper.GetService<ISettingsService>();
        FirstTimeAppearing = true;
        FirstTimeNavigatingTo = true;
    }
    #region Propertys
    public virtual ICommand NavBackCommand => new Command(async () => await GoBack());
    public virtual ICommand RefreshCommand => new Command(async (layoutState) => await RefreshAsync(layoutState));


    [ObservableProperty]
    bool showNavbarBackbutton;

    [ObservableProperty]
    bool firstTimeAppearing;

    [ObservableProperty]
    bool firstTimeNavigatingTo;

    [ObservableProperty]
    bool reloadNeededOnappering;

    [ObservableProperty]
    string title = string.Empty;

    //[ObservableProperty]
    //bool IsLoading;

    [ObservableProperty]
    bool isValid;
    [ObservableProperty]
    bool isSuccess;
    [ObservableProperty]
    bool isRefreshing;
    [ObservableProperty]
    bool isLoading;
    [ObservableProperty]
    bool isError;
    [ObservableProperty]
    bool isEmpty;
    [ObservableProperty]
    bool isSaving;

    [ObservableProperty]
    string _stateEmptyMessage = "Nothing to Show";
    [ObservableProperty]
    string _stateSavingMessage;
    [ObservableProperty]
    private bool cleanupPerformed;
    #endregion

    #region Virutal functions
    //Async
    //public async virtual Task InitializeAsync(object navigationData, BaseViewModel parentViewModel = null) => await Task.FromResult(false);
    //public async virtual Task OnBackButtonPressedAsync() => await GoBack(); //Android hardwear button

    public async virtual Task OnDisappearingAsync() => await Task.FromResult(true);
    public async virtual Task OnAppearingAsync() => await Task.FromResult(true);

    public async virtual Task RefreshAsync(object layoutState) => await Task.FromResult(false);
    public async virtual Task<bool> NavbackIsAllowed() => await Task.FromResult(true);
    //Void
    // public virtual void Initialize(object navigationData, BaseViewModel parentViewModel = null) { }
    public virtual void CleanUp() { }
    public virtual void OnAppearing() => OnAppearingLocal();
    public virtual void OnDisappearing() { }
    public virtual void OnNavigatedTo(NavigatedToEventArgs args) => OnNavigatedToLocal();
    public virtual void RefreshTranslations() => RefreshTranslationsLocal();
    //Denna funtion ska du lägga längst ned i din override
    //base.ApplyQueryAttributes(query);
    //Då funtionnen också körs på navigate back annars sätts sidan i samma state som första gången du laddad den. 
    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        query.Clear();
    }
    public virtual async Task PushModal(Page page)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.Navigation.PushModalAsync(page);
        });
    }
    public virtual async Task GoToAsync(string page)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.GoToAsync(page);

        });

    }
    public virtual async Task GoToAsync(string page, Dictionary<string, object> dic)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.GoToAsync(page, dic);

        });

    }
    public async virtual Task GoBack()
    {
        try
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var shellRoot = Application.Current.MainPage as Shell;
                if (shellRoot != null && shellRoot.Navigation != null)
                {
                    var result = await RunNavbackIsAllowed(shellRoot.CurrentPage);
                    if (!result)
                        return;
                    RunCleanUp(shellRoot.CurrentPage);
                    await Shell.Current.Navigation.PopAsync();
                    //await navigationPage.Navigation.PopAsync();
                }
                else
                {
                    var navigationPage = Application.Current.MainPage as NavigationPage;
                    var result = await RunNavbackIsAllowed(navigationPage.CurrentPage);
                    if (!result)
                        return;

                    RunCleanUp(navigationPage.CurrentPage);
                    await navigationPage.Navigation.PopAsync();
                }
            });
        }
        catch (Exception ex)
        {
            var logdic = new Dictionary<string, string>();
            logdic.Add("Info", "Error in RemovePageAsync Function");
            logdic.Add("Error", ex.StackTrace);
           // TietoGlobals.ReportErrorToAppCenter(ex, logdic);
        }
    }
    public async virtual Task RemoveModalPageAsync()
    {
        try
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {

                var shellRoot = Application.Current.MainPage as Shell;
                if (shellRoot != null && shellRoot.Navigation != null)
                {
                    RunCleanUp(shellRoot.CurrentPage);
                    await Shell.Current.Navigation.PopModalAsync();
                }
                else
                {
                    var navigationPage = Application.Current.MainPage as NavigationPage;
                    RunCleanUp(navigationPage.CurrentPage);
                    await navigationPage.Navigation.PopAsync();
                }

            });
        }
        catch (Exception ex)
        {
            var logdic = new Dictionary<string, string>();
            logdic.Add("Info", "Error in RemoveModalPageAsync");
            logdic.Add("Error", ex.StackTrace);
            //TietoGlobals.ReportErrorToAppCenter(ex, logdic);
        }
    }
    public virtual void RemovePreviousPage()
    {
        try
        {
            
                if (Application.Current.MainPage is Shell mainPage)
                {
                    // We cannot remove root page so lets check the count before we remove
                    var indexToRemove = mainPage.Navigation.NavigationStack.Count - 2;
                    if (indexToRemove > 0)
                    {
                        RunCleanUp(mainPage.Navigation.NavigationStack[indexToRemove]);
                        mainPage.Navigation.RemovePage(mainPage.Navigation.NavigationStack[indexToRemove]);
                    }
                }
           
        }
        catch (Exception ex)
        {
            var logdic = new Dictionary<string, string>();
            logdic.Add("Info", "Error in RemovePreviousPageAsync");
            logdic.Add("Error", ex.StackTrace);
           // TietoGlobals.ReportErrorToAppCenter(ex, logdic);
        }
    }
    #endregion
    #region Internal functions


    #endregion

    #region Public functions
    public void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
    {
        // `lock` ensures that only one thread access the collection at a time
        lock (collection)
        {
            accessMethod?.Invoke();
        }
    }

    public bool PromptToConfirmExit
    {
        get
        {

            //bool promptToConfirmExit = false;
            //if (App.Current.MainPage is ContentPage)
            //{
            //    return true;
            //}
            ////else if (App.Current.MainPage is MasterDetailPage masterDetailPage
            ////    && masterDetailPage.Detail is NavigationPage detailNavigationPage)
            ////{
            ////    return detailNavigationPage.Navigation.NavigationStack.Count <= 1;
            ////}
            //else if (App.Current.MainPage is NavigationPage mainPage)
            //{
            //    if (mainPage.CurrentPage is TabbedPage tabbedPage
            //        && tabbedPage.CurrentPage is NavigationPage navigationPage)
            //    {
            //        return navigationPage.Navigation.NavigationStack.Count <= 1;
            //    }
            //    else
            //    {
            //        return mainPage.Navigation.NavigationStack.Count <= 1;
            //    }
            //}
            //else if (App.Current.MainPage is TabbedPage tabbedPage && tabbedPage.CurrentPage is NavigationPage navigationPage)
            //{
            //    return navigationPage.Navigation.NavigationStack.Count <= 1;
            //}
            return false;// promptToConfirmExit;
        }
    }
    string _stateErrorMessage = string.Empty;
    public string StateErrorMessage
    {
        get => _stateErrorMessage;
        set => SetProperty(ref _stateErrorMessage, value);
    }
    //public enum LayoutState
    //{
    //    None,
    //    Loading,
    //    Saving,
    //    Success,
    //    Error,
    //    Empty,
    //    Custom
    //}
    //LayoutState _currentLayoutState;
    //public LayoutState CurrentLayoutState
    //{
    //    get => _currentLayoutState;
    //    private set => SetProperty(ref _currentLayoutState, value);
    //}
    public enum ViewState : int
    {
        Loading = 0,
        Refreshing = 1,
        Empty = 2,
        Success = 3,
        Error = 4,
        Saving = 5
    }
    public ViewState CurrentViewState { get; set; }
    public virtual void SetCurrentState(object currentState)
    {
        if (currentState is ViewState)
        {
            switch ((ViewState)currentState)
            {
                case ViewState.Refreshing:
                    IsRefreshing = true;
                    IsLoading = false;
                    IsError = false;
                    IsSuccess = false;
                    IsEmpty = false;
                    IsSaving = false;
                    break;

                case ViewState.Loading:
                    IsRefreshing = false;
                    IsLoading = true;
                    IsError = false;
                    IsSuccess = false;
                    IsEmpty = false;
                    IsSaving = false;
                    break;
                case ViewState.Success:
                    IsRefreshing = false;
                    IsLoading = false;
                    IsError = false;
                    IsSuccess = true;
                    IsEmpty = false;
                    IsSaving = false;
                    break;
                case ViewState.Empty:
                    IsRefreshing = false;
                    IsLoading = false;
                    IsError = false;
                    IsSuccess = false;
                    IsEmpty = true;
                    IsSaving = false;
                    break;
                case ViewState.Saving:
                    IsRefreshing = false;
                    IsLoading = false;
                    IsError = false;
                    IsSuccess = false;
                    IsEmpty = false;
                    IsSaving = true;
                    break;
                default:
                    IsRefreshing = false;
                    IsLoading = false;
                    IsError = false;
                    IsSuccess = false;
                    IsError = true;
                    IsSaving = false;
                    break;
            }
            //TietoGlobals.ConsoleWriteLineDebug("CurrentViewState  " + CurrentViewState.ToString() + " Changed to " + (currentState).ToString());

            if ((ViewState)currentState != CurrentViewState)
                CurrentViewState = (ViewState)currentState;
        }
    }




    #endregion

    #region Private functions
    private void RunCleanUp(Page view)
    {
        if (view.BindingContext != null)
        {
            if (view.BindingContext is BaseViewModel viewModelBase)
            {
                if (!viewModelBase.CleanupPerformed)
                    viewModelBase.CleanUp();
            }
        }
    }
    private void RefreshTranslationsLocal()
    {

        OnPropertyChanged(nameof(Title));

    }
    private void OnAppearingLocal()
    {
        if (FirstTimeAppearing)
        {
            if (AnalyticsService != null)
                AnalyticsService.SetCurrentScreen(this.GetType().Name, this.GetType().Name);
        }
        else if (AnalyticsService != null && AnalyticsService.IsTabbedPageViewModel(this.GetType().Name))
        {
            AnalyticsService.SetCurrentScreen(this.GetType().Name, this.GetType().Name);
        }

        FirstTimeAppearing = false;

    }

    private void OnNavigatedToLocal()
    {
        FirstTimeNavigatingTo = false;
    }


    private async Task<bool> RunNavbackIsAllowed(Page view)
    {
        if (view.BindingContext != null)
        {
            if (view.BindingContext is BaseViewModel viewModelBase)
            {

                return await viewModelBase.NavbackIsAllowed();
            }
        }
        return true;
    }




    #endregion



}