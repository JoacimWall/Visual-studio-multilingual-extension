using System;
using System.Collections.Generic;
using MultilingualExtensionExample.ViewModels;
using MultilingualExtensionExample.Views;
using Xamarin.Forms;

namespace MultilingualExtensionExample
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
