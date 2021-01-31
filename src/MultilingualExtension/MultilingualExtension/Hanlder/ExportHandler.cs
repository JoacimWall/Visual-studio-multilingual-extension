﻿using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MultilingualExtension.Shared.Helpers;

using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Shared.Services;

namespace MultilingualExtension
{
    class ExportHandler : CommandHandler
    {
        protected async override void Run()
        {
            
            IProgressBar progress = new Helpers.ProgressBarHelper(Globals.Export_Rows_Info);
            try
            {
                ExportService exportService = new ExportService();
                await IdeApp.Workbench.SaveAllAsync();
                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                ISettingsService settingsService = new Services.SettingsService();
                string selectedFilename = selectedItem.Name;


              var result = await exportService.ExportToFile(selectedFilename, progress, settingsService);
            }

            catch (Exception ex)
            {
                MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {
                progress.HideAll();
                progress = null;
                Console.WriteLine("Translate file completed");
            }

        }
        protected override void Update(CommandInfo info)
        {

            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            string selectedFilename = selectedItem.Name;

            ISettingsService settingsService = new Services.SettingsService();
            var res_Info = Res_Helpers.FileInfo(settingsService.MasterLanguageCode, selectedFilename);

            if (res_Info.Model.IsMasterFile)
            {
                info.Text = Globals.Export_All_Files_Title;
            }
            else
            {
                info.Text = Globals.Export_Seleted_File_Title;

            }
            
        }

    }

}
