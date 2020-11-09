using System;
using MonoDevelop.Components.Commands;
using Gtk;
using MonoDevelop.Ide;
using System.Collections.Generic;
using System.Resources;

using MonoDevelop.Projects;
using System.Collections;
using System.Xml;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Service;

namespace MultilingualExtension
{

    class UpdateFilesHandler : CommandHandler
    {

        protected async override void Run()
        {
            Shared.Interface.IProgressBar progress = new Helper.ProgressBarHelper("synchronizes lines from master RESX file.");

            try
            {

                SyncFileService syncFileService = new SyncFileService();
                Service.SettingsService settingsService = new Service.SettingsService();
                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;

               var result = await syncFileService.SyncFile(selectedItem.FilePath, progress, settingsService);

              


            }
            catch (Exception ex)
            {
                MonoDevelop.Ide.MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

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
            var checkfile = RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
            if (!checkfile.Success)
            {
                info.Text = "Sync all .xx-xx.resx files with this";
            }
            else
            {
                info.Text = "Sync this .xx-xx.resx file";

            }
        }
    }
    public enum MultilingualExtensionCommands
    {
        UpdateFiles,
        TranslateFiles,
        ShowSettings,
        ExportFiles,
        ImportFiles
    }

}
