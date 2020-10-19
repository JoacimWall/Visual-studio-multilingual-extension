﻿using System;
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

namespace MultilingualExtension
{

    class UpdateFilesHandler : CommandHandler
    {

        protected async override void Run()
        {
            Helper.ProgressBarHelper progress = new Helper.ProgressBarHelper("synchronizes lines from master RESX file.");
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
                        var checkfileInFolder =  Helper.RexExHelper.ValidateFilenameIsTargetType(fileName);
                        if (checkfileInFolder.Success)
                            SyncFile(selectedFilename, fileName, progress);

                    }

                }
                else
                {
                    string masterPath = selectedFilename.Substring(0, checkfile.Index) + ".resx";
                    SyncFile(masterPath, selectedFilename, progress);
                }


            }
            catch (Exception ex)
            {
                MonoDevelop.Ide.MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {
                progress.pdata.window.HideAll();
                progress = null;
                Console.WriteLine("Sync file completed");
            }
            //TODO: PArse resx file and add row rto other
            // var p = MonoDevelop.Projects.Services.ProjectService;

            //    var d = MonoDevelop.Core.FileService;
            //var w = MonoDevelop.Projects.So.Gui.Wor
        }
        private  Helper.Result<Boolean> SyncFile(string masterPath,string updatefilePath, Helper.ProgressBarHelper progress)
        {

            

            XmlDocument masterdoc = new XmlDocument();
            masterdoc.Load(masterPath);
            XmlNode rootMaster = masterdoc.DocumentElement;

            XmlDocument updatedoc = new XmlDocument();
            updatedoc.Load(updatefilePath);
            XmlNode rootUpdate = updatedoc.DocumentElement;

            // Add the namespace.  
            //XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            //nsmgr.AddNamespace("bk", "urn:newbooks-schema");

            // Select all nodes data in Master
            bool updatefilechanged = false;
            XmlNodeList nodeListMaster = rootMaster.SelectNodes("//data");
            foreach (XmlNode dataMaster in nodeListMaster)
            {
                // <data name="Select_All" xml:space="preserve">
                //< value > Select All </ value >
                //<comment>New,Translated,Finish</comment>
                //  </ data >

                XmlNode exist = rootUpdate.SelectSingleNode("//data[@name='" + dataMaster.Attributes.GetNamedItem("name").Value + "']");
                if (exist == null)
                {
                    //Add to file
                    updatefilechanged = true;
                    XmlNode newEntry = updatedoc.ImportNode(dataMaster, true);
                    updatedoc.DocumentElement.AppendChild(newEntry);
                    //check if comment exist from master
                    var commentNode = newEntry.SelectSingleNode("comment");
                    if (commentNode == null)
                    {
                        XmlElement elem = updatedoc.CreateElement("comment"); //item1 ,item2..
                        elem.InnerText = Globals.STATUS_COMMENT_NEW;
                        newEntry.AppendChild(elem);
                    }
                    else
                    {
                        commentNode.InnerText = Globals.STATUS_COMMENT_NEW;
                    }
                }
                else
                {
                    //Check if comment exist or not
                    var commentNode = exist.SelectSingleNode("comment");
                    if (commentNode == null)
                    {
                        XmlElement elem = updatedoc.CreateElement("comment"); //item1 ,item2..
                        elem.InnerText = Globals.STATUS_COMMENT_FINAL;
                        exist.AppendChild(elem);
                    }
                    else
                    {
                        commentNode.InnerText = Globals.STATUS_COMMENT_FINAL;
                    }
                }


                progress.pdata.pbar.Pulse();

                
            }
            //Select all rows data in Updatefile to remove row that not exist in master 
            XmlNodeList nodeListUpdate = rootUpdate.SelectNodes("//data");
            foreach (XmlNode dataUpdate in nodeListUpdate)
            {
                // <data name="Select_All" xml:space="preserve">
                //< value > Select All </ value >
                //<comment>New,Translated,Finish</comment>
                //  </ data >

                XmlNode exist = rootMaster.SelectSingleNode("//data[@name='" + dataUpdate.Attributes.GetNamedItem("name").Value + "']");
                if (exist == null)
                {
                    //remove from file
                    updatefilechanged = true;
                    rootUpdate.RemoveChild(dataUpdate);

                }

                progress.pdata.pbar.Pulse();

              

            }
            if (updatefilechanged)
                updatedoc.Save(updatefilePath);

            return new Helper.Result<bool>(true);


        }
        protected override void Update(CommandInfo info)
        {

            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            string selectedFilename = selectedItem.Name;

            //validate file
            var checkfile = Helper.RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
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
