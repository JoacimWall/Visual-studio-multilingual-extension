using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Services;
using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Services;

namespace MultilingualExtension
{
          
    internal class UpdateFilesHandler : CommandHandler
    {

        protected async override void Run()
        {
            IProgressBar progress = new ProgressBar(Globals.Synchronize_Rows_Info);

            try
            {

                SyncFileService syncFileService = new SyncFileService();
                ISettingsService settingsService = new Services.SettingsService();
                await IdeApp.Workbench.SaveAllAsync();

                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;

                var result = await syncFileService.SyncFile(selectedItem.FilePath, progress, settingsService);

                if (!result.WasSuccessful)
                {
                    MessageService.GenericAlert(new GenericMessage { Text = result.ErrorMessage });

                }

            }
            catch (Exception ex)
            {
                MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {
                progress.HideAll();
                progress = null;
                Console.WriteLine("Sync file completed");
            }

        }
         protected override void Update(CommandInfo info)
        {

            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            string selectedFilename = selectedItem.Name;

            //validate file
            ISettingsService settingsService = new Services.SettingsService();
            var res_Info = Res_Helpers.FileInfo(settingsService.MasterLanguageCode, selectedFilename);

            if (res_Info.Model.IsMasterFile)
            {
                info.Text = Globals.Synchronize_All_Files_Title;
            }
            else
            {
                info.Text = Globals.Synchronize_Seleted_File_Title;
            }

        }
    }
    public enum MultilingualExtensionCommands
    {
        UpdateFiles,
        TranslateFiles,
        ShowSettings,
        ExportFiles,
        ImportFiles,
        TranslateAction,
        TranslateSelectedNode,
        ListUnusedTranslations
    }

}
