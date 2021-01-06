using MultilingualExtensionExample.Resources;
using MultilingualExtensionExample.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MultilingualExtensionExample.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            TranslationTest(AppResources.Enter_Your_Cat, AppResources.How_Old_Are_You);
          
            string uppercaseword = AppResources.Enter_Your_Email.ToUpper();
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
        private void TranslationTest(string tans1, string tarans2)
        {


        }
    }
}
