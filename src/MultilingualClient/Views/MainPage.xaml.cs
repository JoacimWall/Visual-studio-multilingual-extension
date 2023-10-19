namespace MultilingualClient.Views;

public partial class MainPage : BaseContentPage
{
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    void OnWebViewGoToRepoClicked(System.Object sender, System.EventArgs e)
    {
    }
}


