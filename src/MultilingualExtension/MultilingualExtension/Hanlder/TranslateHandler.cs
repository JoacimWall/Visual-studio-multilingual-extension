using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Gtk;
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
            IProgressBar progress = new Helpers.ProgressBarHelper(Globals.Translate_Rows_Info);
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
            var checkfile = RegExHelper.ValidateFilenameIsTargetType(selectedFilename);
            if (!checkfile.Success)
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
