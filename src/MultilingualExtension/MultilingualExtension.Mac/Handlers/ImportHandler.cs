using System;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
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
            
            try
            {
                ImportService importService = new ImportService();
                var path = IdeApp.Workspace.CurrentSelectedSolution.FileName;
                var projPath = System.IO.Path.GetDirectoryName(path);
                ISettingsService settingsService = new Services.SettingsService(projPath);
                await IdeApp.Workbench.SaveAllAsync();
                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                string selectedFilename = selectedItem.Name;
                //MonoDevelop.Ide.Gui.PadContent pad = new MonoDevelop.Ide.Gui.PadContent();
                //IdeApp.Workbench.AddPad(new Guid().)
               // var dte =  ServiceProvider.GetService(typeof(DTE)) as DTE2;
                var outputPane = OutputWindowHelper.GetOutputWindow();
                var result = await importService.ImportToResx(selectedFilename, outputPane, settingsService);



            }

            catch (Exception ex)
            {
                MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {
                
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

