using System.Globalization;
using System.Threading;

namespace MultilingualExtensionExample
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //sample of hard set language that not need to match language on device
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("nb-NO");
            //Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            //MultilingualExtensionExample.Resources.AppResources.Culture = Thread.CurrentThread.CurrentUICulture;

        }

    }
}
