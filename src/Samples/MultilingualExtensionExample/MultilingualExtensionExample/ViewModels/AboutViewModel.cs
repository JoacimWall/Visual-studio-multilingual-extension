using System;
using System.Windows.Input;
using MultilingualExtensionExample.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MultilingualExtensionExample.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamain-quickstart"));
            //Sample of translation in code. Ciew AboutView.xaml to se example for in xaml example for hello translation.
            TranslationFromViewModel = AppResources.CSharp_Is_Love;
        }
        public String TranslationFromViewModel { get; set; }
        public ICommand OpenWebCommand { get; }
    }
}