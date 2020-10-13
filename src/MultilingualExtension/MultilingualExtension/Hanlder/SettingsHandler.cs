using System;
using MonoDevelop.Components.Commands;

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

            
        }
        public SettingsHandler()
        {
        }
    }
}
