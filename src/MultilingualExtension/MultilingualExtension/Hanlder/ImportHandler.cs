using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MultilingualExtension.Helper;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Service;

namespace MultilingualExtension
{
    class ImportHandler : CommandHandler
    {
        protected  override async void Run()
        {
            ProgressBarHelper progress = new ProgressBarHelper("Import rows where comment is 'Final'");
            try
            {
                ImportService importService = new ImportService();
                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                string selectedFilename = selectedItem.Name;
                var result = await importService.ImportCsvToResx(selectedFilename, progress);



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
            var checkfile = RexExHelper.ValidateFilenameIsTargetTypeCsv(selectedFilename);
            if (!checkfile.Success)
            {
                info.Visible = false;
            }
            else
            {
                info.Visible = true;
                info.Text = "Import translations";

            }
        }
    }
   

}
