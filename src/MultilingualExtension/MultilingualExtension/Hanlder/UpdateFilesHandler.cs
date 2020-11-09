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
                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                bool addCommentNodeToMasterResx = Service.SettingsService.AddCommentNodeMasterResx;

                string selectedFilename = selectedItem.Name;

                //validate file
                var checkfile = RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
                if (!checkfile.Success)
                {
                    //TODO: Show message you have selected master .resx file we will update all other resx files in this folder that have the format .sv-SE.resx
                    int folderindex = selectedFilename.LastIndexOf("/");
                    string masterFolderPath = selectedFilename.Substring(0, folderindex);

                    string[] fileEntries = Directory.GetFiles(masterFolderPath);
                    foreach (string fileName in fileEntries)
                    {
                        var checkfileInFolder =  RexExHelper.ValidateFilenameIsTargetType(fileName);
                        if (checkfileInFolder.Success)
                            syncFileService.SyncFile(selectedFilename, fileName, addCommentNodeToMasterResx, progress);

                    }

                }
                else
                {
                    string masterPath = selectedFilename.Substring(0, checkfile.Index) + ".resx";
                    syncFileService.SyncFile(masterPath, selectedFilename, addCommentNodeToMasterResx, progress);
                }


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
            //TODO: PArse resx file and add row rto other
            // var p = MonoDevelop.Projects.Services.ProjectService;

            //    var d = MonoDevelop.Core.FileService;
            //var w = MonoDevelop.Projects.So.Gui.Wor
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
