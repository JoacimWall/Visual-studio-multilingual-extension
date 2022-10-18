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
           

            try
            {

                SyncFileService syncFileService = new SyncFileService();
                //var dte = ServiceProvider.GetService(typeof(DTE)) as DTE2;
                var path = IdeApp.Workspace.CurrentSelectedSolution.FileName;
                var projPath = System.IO.Path.GetDirectoryName(path);
                ISettingsService settingsService = new Services.SettingsService(projPath);
                await IdeApp.Workbench.SaveAllAsync();

                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                //Dummy for mac 
                var outputPane = OutputWindowHelper.GetOutputWindow();
                var result = await syncFileService.SyncFile(selectedItem.FilePath, outputPane, settingsService);

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
                
                Console.WriteLine("Sync file completed");
            }

        }
         protected override void Update(CommandInfo info)
        {

            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            string selectedFilename = selectedItem.Name;
            var path = IdeApp.Workspace.CurrentSelectedSolution.FileName;
            var projPath = System.IO.Path.GetDirectoryName(path);
            ISettingsService settingsService = new Services.SettingsService(projPath);
            var res_Info = Res_Helpers.FileInfo(settingsService.ExtensionSettings.MasterLanguageCode, selectedFilename);

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
