using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using FileHelpers;

//using DocumentFormat.OpenXml.Packaging;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MultilingualExtension
{
    class ImportHandler : CommandHandler
    {
        protected override void Run()
        {
            Helper.ProgressBarHelper progress = new Helper.ProgressBarHelper("Import rows where comment is 'Final'");
            try
            {

                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                string selectedFilename = selectedItem.Name;

                //validate file
                var checkfile = Helper.RexExHelper.ValidateFilenameIsTargetTypeCsv(selectedFilename);
                if (!checkfile.Success)
                {
                    //TODO: Show message you select file have the format .sv-SE.resx
                    return;

                }
                else
                {
                    string updatePath = selectedFilename.Substring(0, selectedFilename.Length-4);
                    ImportCsvToResx(selectedFilename, updatePath, progress);
                }

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
            var checkfile = Helper.RexExHelper.ValidateFilenameIsTargetTypeCsv(selectedFilename);
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
        private void ImportCsvToResx(string masterPath, string updatePath, Helper.ProgressBarHelper progress)
        {
            


            XmlDocument updatedoc = new XmlDocument();
            updatedoc.Load(updatePath);
            XmlNode rootUpdate = updatedoc.DocumentElement;

            var engine = new FileHelperEngine<TranslationsRow>();
            engine.HeaderText = engine.GetFileHeader();
            var records = engine.ReadFile(masterPath);
            bool updatefilechanged = false;
            foreach (var record in records)
            {
                if (record.Status == Globals.STATUS_COMMENT_FINAL)
                {
                    var dataNode = rootUpdate.SelectSingleNode("//data[@name='" + record.Name + "']");
                    if (dataNode != null)
                    {
                        updatefilechanged = true;
                        var dataNodeValue = dataNode.SelectSingleNode("value");
                        var dataNodeComment = dataNode.SelectSingleNode("comment");
                        if (dataNodeValue != null && dataNodeComment != null)
                        {
                            dataNodeValue.InnerText = record.TargetLanguage;
                            dataNodeComment.InnerText = Globals.STATUS_COMMENT_FINAL;
                        }
           
                    }

                }
                progress.pdata.pbar.Pulse();

            }

            if (updatefilechanged)
                updatedoc.Save(updatePath);

            

        }

    }
   

}
