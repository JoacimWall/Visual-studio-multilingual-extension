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

               
                string filename = selectedItem.Name;

                //validate file
                Regex regex = new Regex(".[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx");
                var checkfile = regex.Match(filename);
                if (!checkfile.Success)
                {
                    //TODO: Show message select file to update
                    return;

                }

                string masterPath = filename.Substring(0, checkfile.Index) + ".resx";

                XmlDocument masterdoc = new XmlDocument();
                masterdoc.Load(masterPath);
                XmlNode rootMaster = masterdoc.DocumentElement;

                XmlDocument updatedoc = new XmlDocument();
                updatedoc.Load(filename);
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

                    progress.pdata.pbar.Pulse();

                    //TODO:Remove before publish 
                    //Silmulate time 
                    await Task.Delay(2000);

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

                    //TODO:Remove before publish 
                    //Silmulate time 
                    await Task.Delay(1000);

                }
                if (updatefilechanged)
                    updatedoc.Save(filename);

                progress.pdata.window.HideAll();
                progress = null;


            }
            catch (Exception ex)
            {
                MonoDevelop.Ide.MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {
                progress.pdata.window.HideAll();
                progress = null;
            }
            //TODO: PArse resx file and add row rto other
            // var p = MonoDevelop.Projects.Services.ProjectService;

            //    var d = MonoDevelop.Core.FileService;
            //var w = MonoDevelop.Projects.So.Gui.Wor
        }

        protected override void Update(CommandInfo info)
        {

            //TODO: Check if resx files exist.
        }
    }
    public enum MultilingualExtensionCommands
    {
        UpdateFiles,
        TranslateFiles,
        ShowSettings
    }

}
