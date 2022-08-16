using System;
using MonoDevelop.Components.Commands;
using MultilingualExtension.Shared.Helpers;

namespace MultilingualExtension
{
	public class SettingsHandler : CommandHandler
	{
        protected async override void Run()
        {
            Console.WriteLine("Sorry Visual studio 2022 does not support GTK. working on solution");
            return;
            SettingsWindow settingsWindow = new SettingsWindow();



        }
        protected override void Update(CommandInfo info)
        {
            info.Text = Globals.Show_Setting_Title;

        }
     
    }
}

