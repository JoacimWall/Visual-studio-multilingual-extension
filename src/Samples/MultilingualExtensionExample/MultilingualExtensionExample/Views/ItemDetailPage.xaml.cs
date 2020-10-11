using System.ComponentModel;
using Xamarin.Forms;
using MultilingualExtensionExample.ViewModels;

namespace MultilingualExtensionExample.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}