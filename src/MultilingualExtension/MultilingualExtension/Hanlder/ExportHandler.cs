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
    class ExportHandler : CommandHandler
    {
        protected override void Run()
        {
            Helper.ProgressBarHelper progress = new Helper.ProgressBarHelper("export rows where comment is 'New' or 'Need review'");
            try
            {

                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                string selectedFilename = selectedItem.Name;

                //validate file
                var checkfile = Helper.RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
                if (!checkfile.Success)
                {
                    //TODO: Show message you have selected master .resx file we will update all other resx files in this folder that have the format .sv-SE.resx
                    int folderindex = selectedFilename.LastIndexOf("/");
                    string masterFolderPath = selectedFilename.Substring(0, folderindex);

                    string[] fileEntries = Directory.GetFiles(masterFolderPath);
                    foreach (string fileName in fileEntries)
                    {
                        var checkfileInFolder = Helper.RexExHelper.ValidateFilenameIsTargetType(fileName);
                        if (checkfileInFolder.Success)
                            ExportToExcel(selectedFilename, fileName, progress);
                    }

                }
                else
                {
                    string masterPath = selectedFilename.Substring(0, checkfile.Index) + ".resx";
                    ExportToExcel(masterPath, selectedFilename, progress);
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
            var checkfile = Helper.RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
            if (!checkfile.Success)
            {
                info.Text = "Export all .xx-xx.resx files";
            }
            else
            {
                info.Text = "Export this .xx-xx.resx file";

            }
        }
        private void ExportToExcel(string masterPath, string updatePath, Helper.ProgressBarHelper progress)
        {
            int folderindex = updatePath.LastIndexOf("/");
            string masterFolderPath = updatePath.Substring(0, folderindex + 1);


            XmlDocument updatedoc = new XmlDocument();
            updatedoc.Load(updatePath);
            XmlNode rootUpdate = updatedoc.DocumentElement;

            XmlDocument masterdoc = new XmlDocument();
            masterdoc.Load(masterPath);
            XmlNode rootMaster = masterdoc.DocumentElement;


            var rows = new List<TranslationsRow>();

            // Select all nodes data
            XmlNodeList nodeListUpdate = rootUpdate.SelectNodes("//data");
            foreach (XmlNode dataUpdate in nodeListUpdate)
            {
                //check if comment exist
                var commentNode = dataUpdate.SelectSingleNode("comment");
                if (commentNode != null && (commentNode.InnerText == Globals.STATUS_COMMENT_NEW || commentNode.InnerText == Globals.STATUS_COMMENT_NEED_REVIEW))
                {
                    //Get master value
                    XmlNode masterExist = rootMaster.SelectSingleNode("//data[@name='" + dataUpdate.Attributes.GetNamedItem("name").Value + "']");

                    if (masterExist != null)
                    {
                        var masterValueNode = masterExist.SelectSingleNode("value");
                        if (masterValueNode == null)
                        {
                            continue;
                            //TODO: log error    
                        }
                        var updateValueNode = dataUpdate.SelectSingleNode("value");
                        if (updateValueNode == null)
                        {
                            continue;
                            //TODO: log error  

                        }

                        rows.Add(new TranslationsRow()
                        {
                            Name = dataUpdate.Attributes.GetNamedItem("name").Value,
                            SourceLanguage = masterValueNode.InnerText,
                            TargetLanguage = updateValueNode.InnerText,
                            Status = commentNode.InnerText
                        });

                    }
                }


               

                progress.pdata.pbar.Pulse();
            }

            var engine = new FileHelperEngine<TranslationsRow>(System.Text.Encoding.UTF8);
            //get filename
            var checkfile = Helper.RexExHelper.GetFilenameResx(updatePath);
            engine.HeaderText = engine.GetFileHeader();
            engine.WriteFile(masterFolderPath + checkfile.Value +  ".csv", rows);

        }

    }
    /// <summary>
    /// Layout for a file delimited by ,
    /// </summary>
    [DelimitedRecord(";")]
    public class TranslationsRow
    {
        

        public string Name;

       
        public string SourceLanguage;

        public string TargetLanguage;

        public string Status;
    }

}
