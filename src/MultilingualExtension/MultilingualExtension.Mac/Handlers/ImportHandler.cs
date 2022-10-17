using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Shared.Services;
namespace MultilingualExtension
{
    public class ImportHandler : CommandHandler
    {
        protected override async void Run()
        {
            IProgressBar progress = new Services.ProgressBar(Globals.Import_Rows_Info);
            try
            {
                ImportService importService = new ImportService();
                var path = IdeApp.Workspace.CurrentSelectedSolution.FileName;
                var projPath = System.IO.Path.GetDirectoryName(path);
                ISettingsService settingsService = new Services.SettingsService(projPath);
                await IdeApp.Workbench.SaveAllAsync();
                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                string selectedFilename = selectedItem.Name;
                var result = await importService.ImportToResx(selectedFilename, progress, settingsService);



            }

            catch (Exception ex)
            {
                MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {
                progress.HideAll();
                progress = null;
                Console.WriteLine("Import file completed");
            }

        }
        protected override void Update(CommandInfo info)
        {

            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            string selectedFilename = selectedItem.Name;

            //validate file
            var checkfilecsv = RegExHelper.ValidateFilenameIsTargetTypeResx_Csv(selectedFilename);
            var checkfilecxlsx = RegExHelper.ValidateFilenameIsTargetTypeResx_Xlsx(selectedFilename);
            var checkfilecsv_resw = RegExHelper.ValidateFilenameIsTargetTypeResw_Csv(selectedFilename);
            var checkfilecxlsx_resw = RegExHelper.ValidateFilenameIsTargetTypeResw_Xlsx(selectedFilename);

            if (!checkfilecsv.Success && !checkfilecxlsx.Success && !checkfilecsv_resw.Success && !checkfilecxlsx_resw.Success)
            {
                info.Visible = false;
            }
            else
            {
                info.Visible = true;
                info.Text = Globals.Import_Translation_Title;

            }
        }
    }
}

