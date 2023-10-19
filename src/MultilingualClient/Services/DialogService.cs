using Acr.UserDialogs;
namespace MultilingualClient.Services;

public interface IDialogService
{
    Task ShowAlertAsync(string message, string title, string buttonLabel);
    Task<bool> ShowAlertAsync(string message, string title, string buttonAccept, string buttonCancel);
    Task<string> ShowPromptAsync(string message, string title, string ok, string cancel, string initvalue = "");
    Task<string> ShowActionSheetAsync(string title, string cancel, string destructive, params String[] buttons);

    Task<bool> ConfirmAsync(string message, string title, string ok, string cancel);
    IProgressDialog GetProgress(string title);
    void ShowToast(string message, ToastPosition toastPosition = ToastPosition.Top);
}

public class DialogService : IDialogService
{
    public async Task ShowAlertAsync(string message, string title, string buttonLabel)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
       {
           await Application.Current.MainPage.DisplayAlert(title, message, buttonLabel);
       });


    }
    public async Task<bool> ShowAlertAsync(string message, string title, string buttonAccept, string buttonCancel)
    {
        return await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, buttonAccept, buttonCancel);
        });


    }
    public async Task<string> ShowActionSheetAsync(string title, string cancel, string destructive, params string[] buttons)
    {
        if (String.IsNullOrEmpty(title))
            title = "";

        buttons = buttons.Where(c => c != null).ToArray();

        string result = null;

        await MainThread.InvokeOnMainThreadAsync(async () =>
       {
           result = await Application.Current.MainPage.DisplayActionSheet(title, cancel, destructive, buttons);
       });



        if (result == null)
            return string.Empty;

        return result;

    }
    public async Task<string> ShowPromptAsync(string message, string title, string ok, string cancel, string initvalue = "")
    {
        return await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            return await Application.Current.MainPage.DisplayPromptAsync(title, message, ok, cancel, initialValue: initvalue);
        });


    }

    public async Task<bool> ConfirmAsync(string message, string title, string ok, string cancel)
    {
        return await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, ok, cancel);
        });

    }

    public IProgressDialog GetProgress(string title)
    {
        var v = new ProgressDialogConfig();
        v.SetTitle(title);
        //v.MaskType = MaskType.Gradient;
        v.SetMaskType(MaskType.Gradient);
        return UserDialogs.Instance.Progress(v);
    }

    public void ShowToast(string message, ToastPosition toastPosition = ToastPosition.Top)
    {
        // Add top and botton space to iOS
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            message = "\n" + message + "\n";
        }
        MainThread.BeginInvokeOnMainThread(() =>
      {
          UserDialogs.Instance.Toast(new ToastConfig(message)
                 .SetBackgroundColor(System.Drawing.Color.Gray)
                 .SetMessageTextColor(System.Drawing.Color.White)
                 .SetDuration(TimeSpan.FromSeconds(3))
                 .SetPosition(toastPosition)


             );
      });
    }

}
