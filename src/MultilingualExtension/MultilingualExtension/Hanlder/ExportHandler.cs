using System;

//using DocumentFormat.OpenXml.Packaging;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MultilingualExtension.Helper;
using MultilingualExtension.Shared;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Service;

namespace MultilingualExtension
{
    class ExportHandler : CommandHandler
    {
        protected async override void Run()
        {
            ProgressBarHelper progress = new ProgressBarHelper("export rows where comment is 'New' or 'Need review'");
            try
            {
                ExportService exportService = new ExportService();
                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                string selectedFilename = selectedItem.Name;


              var result = await exportService.ExportToFile(selectedFilename, progress);
            }

            catch (Exception ex)
            {
                MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {
                progress.pdata.window.HideAll();
                progress = null;
                Console.WriteLine("Translate file completed");
            }

        }
        protected override void Update(CommandInfo info)
        {

            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            string selectedFilename = selectedItem.Name;

            //validate file
            var checkfile = RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
            if (!checkfile.Success)
            {
                info.Text = "Export all .xx-xx.resx files";
            }
            else
            {
                info.Text = "Export this .xx-xx.resx file";

            }
        }

    }

}
