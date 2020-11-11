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

namespace MultilingualExtension
{
    class TranslateHandler : CommandHandler
    {
        
        protected async override void Run()
        {
            IProgressBar progress = new Helpers.ProgressBarHelper("Translate rows where comment has value 'New'");
            Service.SettingsService settingsService  = new Service.SettingsService();
            try
            {
                Shared.Service.TranslationService translationService = new Shared.Service.TranslationService();
                
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
            var checkfile = RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
            if (!checkfile.Success)
            {
                info.Text = "Translate all .xx-xx.resx files";
            }
            else
            {
                info.Text = "Translate this .xx-xx.resx file";

            }
        }
        //public struct ProgressData
        //{
        //    public Gtk.Window window;
        //    public Gtk.ProgressBar pbar;
        //    public uint timer;
        //    public bool activity_mode;
        //}
    }

}
