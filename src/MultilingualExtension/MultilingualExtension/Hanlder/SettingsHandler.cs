﻿using MonoDevelop.Components.Commands;
using MultilingualExtension.Shared.Helpers;

namespace MultilingualExtension
{
     class SettingsHandler: CommandHandler
    {
        protected async override void Run()
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            


        }
        protected override void Update(CommandInfo info)
        {
            info.Text = Globals.Show_Setting_Title;

        }
        public SettingsHandler()
        {
        }
    }
}
