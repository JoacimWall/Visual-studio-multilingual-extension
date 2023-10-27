namespace MultilingualClient.Views;

public partial class MainPage : BaseContentPage
{
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        ((MainViewModel)this.BindingContext).LogText = this.LogText;
        Loaded += MainPage_Loaded;
    }

    private void MainPage_Loaded(object sender, EventArgs e)
    {
        fileEditor.Focus();
        fileEditor.Unfocus();
    }
}


