using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Shared.Services;

namespace MultilingualExtension
{
    class TranslateHandler : CommandHandler
    {
        
        protected async override void Run()
        {
            IProgressBar progress = new Services.ProgressBar(Globals.Translate_Rows_Info);
            ISettingsService settingsService  = new Services.SettingsService();
            try
            {
                TranslationService translationService = new TranslationService();
                await IdeApp.Workbench.SaveAllAsync();
                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                string selectedFilename = selectedItem.Name;

              var result= await translationService.TranslateFile(selectedFilename, progress, settingsService);

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

            //validate file
            ISettingsService settingsService = new Services.SettingsService();
            var res_Info = Res_Helpers.FileInfo(settingsService.MasterLanguageCode, selectedFilename);
           
            if (res_Info.Model.IsMasterFile)
            {
                info.Text = Globals.Translate_All_Files_Title;
            }
            else
            {
                info.Text = Globals.Translate_Seleted_File_Title;

            }
        }
       
    }

}
