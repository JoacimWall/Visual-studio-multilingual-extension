using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MultilingualExtension.Shared.Helpers;

using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Shared.Models;
using MultilingualExtension.Shared.Services;

namespace MultilingualExtension
{
    class ExportHandler : CommandHandler
    {
        protected async override void Run(object dataItem)
        {
            
            IProgressBar progress = new Helpers.ProgressBarHelper(Globals.Export_Rows_Info);
            try
            {
                string statusToExport = dataItem as string;
                ExportService exportService = new ExportService();
                await IdeApp.Workbench.SaveAllAsync();
                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                ISettingsService settingsService = new Services.SettingsService();
                string selectedFilename = selectedItem.Name;


              var result = await exportService.ExportToFile(selectedFilename, statusToExport, progress, settingsService);
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
        protected override void Update(CommandArrayInfo info)
        {

            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            string selectedFilename = selectedItem.Name;
            var commandSet = new CommandInfoSet();
            ISettingsService settingsService = new Services.SettingsService();
            var res_Info = Res_Helpers.FileInfo(settingsService.MasterLanguageCode, selectedFilename);

            commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW), Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW);
            commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEW), Globals.STATUS_COMMENT_NEW);
            commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEED_REVIEW), Globals.STATUS_COMMENT_NEED_REVIEW);
            commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_FINAL), Globals.STATUS_COMMENT_FINAL);
            commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_ALL_ROWS), Globals.STATUS_COMMENT_ALL_ROWS);

            if (res_Info.Model.IsMasterFile)
            {
                commandSet.Text = Globals.Export_All_Files_Title;
            }
            else
            {
                commandSet.Text = Globals.Export_Seleted_File_Title;
            }
            
            info.Add(commandSet);

        }

    }

}
