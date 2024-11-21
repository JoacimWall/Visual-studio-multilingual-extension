using System.Xml;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Shared.Models;
using MultilingualExtension.Services;

namespace MultilingualExtension.Shared.Services
{
    public class SyncFileService
    {
        public SyncFileService()
        {
        }
        public async Task<Result<Boolean>> UpdateNodeStatus(string selectedFilename, UpdateStatusForTranslation updateStatusForTranslation, MultilingualExtension.Shared.Interfaces.IStatusPadLoger outputPane, ISettingsService settingsService)
        {
            try
            {

                bool addCommentNodeToMasterResx = settingsService.ExtensionSettings.ExportMasterFileOnExport;

                //validate file
                var checkfile = RegExHelper.ValidateFilenameIsTargetType(selectedFilename);
                if (!checkfile.Success)
                {
                    string folderSeperator = Environment.OSVersion.Platform == PlatformID.Win32NT ? "\\" : "/";
                    int folderindex = selectedFilename.LastIndexOf(folderSeperator);
                    
                    string masterFolderPath = selectedFilename.Substring(0, folderindex);

                    string[] fileEntries = Directory.GetFiles(masterFolderPath);
                    foreach (string fileName in fileEntries)
                    {
                        var checkfileInFolder = RegExHelper.ValidateFilenameIsTargetType(fileName);
                        if (checkfileInFolder.Success)
                        {
                            var result = await UpdateStatusInternal(selectedFilename, fileName, updateStatusForTranslation, addCommentNodeToMasterResx, outputPane);
                            if (!result.WasSuccessful)
                                return result;
                        }
                    }

                }
                else
                {
                    string masterPath = selectedFilename.Substring(0, checkfile.Index) + ".resx";
                    var result = await UpdateStatusInternal(masterPath, selectedFilename, updateStatusForTranslation, addCommentNodeToMasterResx, outputPane);
                    if (!result.WasSuccessful)
                        return result;
                }
                return new Result<bool>(true);


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                
                Console.WriteLine("Sync file completed");
            }


        }
        
        public async Task<Result<Boolean>> SyncFile(string selectedFilename, IStatusPadLoger outputPane, ISettingsService settingsService)
        {
            try
            {
                bool addCommentNodeToMasterResx = settingsService.ExtensionSettings.ExportMasterFileOnExport;
                var checkfileResw = RegExHelper.ValidateFileTypeIsResw(selectedFilename);
                if (checkfileResw.Success)
                {
                    var resultResw = ReswHelpers.GetBasInfo(settingsService.ExtensionSettings.MasterLanguageCode, selectedFilename);
                    if (!resultResw.WasSuccessful)
                        return new Result<bool>(resultResw.ErrorMessage);

                    if (resultResw.Model.IsMasterFile)
                        outputPane.WriteText("Sync all files with " + settingsService.ExtensionSettings.MasterLanguageCode);

                    foreach (var updatePath in resultResw.Model.UpdateFilepaths)
                    {
                        var result = await SyncFileInternal(resultResw.Model, updatePath, settingsService.ExtensionSettings, outputPane);
                         var fileinfo =Helpers.Res_Helpers.FileInfo(settingsService.ExtensionSettings.MasterLanguageCode,updatePath);
                        outputPane.WriteText(string.Format("Sync done for {0}-{1}: {2} rows added, {3} rows removed.",fileinfo.Model.LanguageBase, fileinfo.Model.LanguageCulture, result.Model.Item1.ToString(), result.Model.Item2.ToString())) ;

                    }

                    return new Result<bool>(true);
                }
                //------------------ RESX failes -------------------------------------------------// 
                //validate file
                var resultResx = ResxHelpers.GetBasInfo(selectedFilename, settingsService.ExtensionSettings.MasterLanguageCode);
                if (!resultResx.WasSuccessful)
                    return new Result<bool>(resultResx.ErrorMessage);
                if (resultResx.Model.IsMasterFile)
                    outputPane.WriteText("Sync all files with " + resultResx.Model.MasterFilename);

                foreach (var updatePath in resultResx.Model.UpdateFilepaths)
                {
                    
                    var result = await SyncFileInternal(resultResx.Model, updatePath, settingsService.ExtensionSettings, outputPane);
                    var fileinfo = Helpers.Res_Helpers.FileInfo(settingsService.ExtensionSettings.MasterLanguageCode, updatePath);
                    outputPane.WriteText(string.Format("Sync done for {0}-{1}: {2} rows added, {3} rows removed.", fileinfo.Model.LanguageBase, fileinfo.Model.LanguageCulture, result.Model.Item1.ToString(), result.Model.Item2.ToString()));

                }

                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                return new Result<bool>(ex.Message);
            }
            finally
            {
                
                Console.WriteLine("Sync file completed");
            }


        }
        
        private async Task<Result<Tuple<int, int>>> SyncFileInternal(Res_Files masterfileInfo, string updatefilePath, ExtensionSettings settings, IStatusPadLoger outputPane)
        {
            XmlDocument masterdoc = new XmlDocument();
            masterdoc.Load(masterfileInfo.MasterFilepath);
            XmlNode rootMaster = masterdoc.DocumentElement;

            XmlDocument updatedoc = new XmlDocument();
            updatedoc.Load(updatefilePath);
            XmlNode rootUpdate = updatedoc.DocumentElement;

            List<string> ListEnums = new List<string>();

            // Select all nodes data in Master
            bool updateFileChanged = false;
            bool masterFileChanged = false;
            int removedCount = 0;
            int AddedCount = 0;
            XmlNodeList nodeListMaster = rootMaster.SelectNodes("//data");
            foreach (XmlNode dataMaster in nodeListMaster)
            {
                //used to create a enum file for all translations
                ListEnums.Add(dataMaster.Attributes.GetNamedItem("name").Value);

                var masterCommentNode = dataMaster.SelectSingleNode("comment");
                //Check if we are shold not translat this node
                if (masterCommentNode != null && masterCommentNode.InnerText == Globals.STATUS_COMMENT_NO_TRANSLATION)
                    continue;

                //Add comment node to master if that is set in settings

                if (settings.ExportMasterFileOnExport)
                {
                    if (masterCommentNode == null)
                    {
                        XmlElement elem = masterdoc.CreateElement("comment"); //item1 ,item2..
                        elem.InnerText = Globals.STATUS_COMMENT_NEED_REVIEW;
                        dataMaster.AppendChild(elem);
                        masterFileChanged = true;
                    }
                }

                XmlNode exist = rootUpdate.SelectSingleNode("//data[@name='" + dataMaster.Attributes.GetNamedItem("name").Value + "']");
                if (exist == null)
                {
                    //Add to file
                    updateFileChanged = true;
                    AddedCount++;
                    XmlNode newEntry = updatedoc.ImportNode(dataMaster, true);
                    updatedoc.DocumentElement.AppendChild(newEntry);
                    //check if comment exist from master
                    var updateCommentNode = newEntry.SelectSingleNode("comment");
                    if (updateCommentNode == null)
                    {
                        XmlElement elem = updatedoc.CreateElement("comment"); //item1 ,item2..
                        elem.InnerText = Globals.STATUS_COMMENT_NEW;
                        newEntry.AppendChild(elem);
                    }
                    else
                    {
                        updateCommentNode.InnerText = Globals.STATUS_COMMENT_NEW;
                    }
                }
                else
                {
                    //Check if comment exist or not
                    var updateCommentNode = exist.SelectSingleNode("comment");
                    if (updateCommentNode == null)
                    {   //If comment not exists then we think this is old row that are allredy translated and final
                        XmlElement elem = updatedoc.CreateElement("comment"); //item1 ,item2..
                        elem.InnerText = Globals.STATUS_COMMENT_FINAL;
                        exist.AppendChild(elem);
                        updateFileChanged = true;
                    }
                    else
                    {  //Comment not ok set to need review
                        if (updateCommentNode.InnerText != Globals.STATUS_COMMENT_NEW && updateCommentNode.InnerText != Globals.STATUS_COMMENT_NEED_REVIEW && updateCommentNode.InnerText != Globals.STATUS_COMMENT_FINAL)
                        {
                            updateFileChanged = true;
                            updateCommentNode.InnerText = Globals.STATUS_COMMENT_NEED_REVIEW;
                        }
                    }
                }


               


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
                    updateFileChanged = true;
                    removedCount++;
                    rootUpdate.RemoveChild(dataUpdate);

                }

            }
            if (updateFileChanged)
                updatedoc.Save(updatefilePath);

            if (masterFileChanged)
                masterdoc.Save(masterfileInfo.MasterFilepath);

            if (!string.IsNullOrEmpty(settings.TranslationEnumNamespace))
            {
                string body = string.Empty;
                foreach (var item in ListEnums)
                    body = body + string.Format("{0}{1}{2}", item, ",", Environment.NewLine);


                //Create enum file
               
                string enumfileBody = string.Format("{0} {1}public enum {2} {3};", "namespace " + settings.TranslationEnumNamespace, Environment.NewLine, masterfileInfo.MasterFilename.Replace(".resx", "Enums"), Environment.NewLine);
                enumfileBody = enumfileBody  + "{" + Environment.NewLine + body + Environment.NewLine + "}";

                string newFilename = masterfileInfo.MasterFilename.Replace(".resx", "Enums.cs");
                string enumFileSavePath = Path.Combine(masterfileInfo.MasterFilepath.Replace(masterfileInfo.MasterFilename, ""), newFilename);
                using FileStream outputStream = File.OpenWrite(enumFileSavePath);
                using StreamWriter streamWriter = new StreamWriter(outputStream, System.Text.Encoding.UTF8);
                //string output = editFileText.Replace("“", "\"").Replace("”", "\"");
                await streamWriter.WriteAsync(enumfileBody);
                outputPane.WriteText("Translation Enum file created, " + newFilename);
            }
            

            return new Result<Tuple<int, int>>(new Tuple<int, int>(AddedCount, removedCount));


        }
        private async Task<Result<Boolean>> UpdateStatusInternal(string masterfilePath, string updatefilePath, UpdateStatusForTranslation updateStatusForTranslation, bool addMasterCommentNode, IStatusPadLoger outputPane)
        {

            XmlDocument masterdoc = new XmlDocument();
            masterdoc.Load(masterfilePath);
            XmlNode rootMaster = masterdoc.DocumentElement;

            XmlDocument updatedoc = new XmlDocument();
            updatedoc.Load(updatefilePath);
            XmlNode rootUpdate = updatedoc.DocumentElement;

           

            // Select all nodes data in Master
            bool updateFileChanged = false;

            XmlNode nodeMaster = rootMaster.SelectSingleNode("//data[@name='" + updateStatusForTranslation.NodeName + "']");
            if (nodeMaster == null)
            {
                return new Result<bool>("Save master resx file before sync"); 
            }


            XmlNode exist = rootUpdate.SelectSingleNode("//data[@name='" + updateStatusForTranslation.NodeName + "']");
            if (exist == null)
            {
                //Add to file
                updateFileChanged = true;
                XmlNode newEntry = updatedoc.ImportNode(nodeMaster, true);
                updatedoc.DocumentElement.AppendChild(newEntry);
                //check if comment exist from master
                var commentNode = newEntry.SelectSingleNode("comment");
                if (commentNode == null)
                {
                    XmlElement elem = updatedoc.CreateElement("comment"); //item1 ,item2..
                    elem.InnerText = updateStatusForTranslation.NewStatus; 
                    newEntry.AppendChild(elem);
                }
                else
                {
                    commentNode.InnerText = updateStatusForTranslation.NewStatus;
                }
            }
            else
            {
                //Check if comment exist or not
                var commentNode = exist.SelectSingleNode("comment");
                if (commentNode == null)
                {   //If comment not exists then we think this is old row that are allredy translated and final
                    XmlElement elem = updatedoc.CreateElement("comment"); //item1 ,item2..
                    elem.InnerText = updateStatusForTranslation.NewStatus;
                    exist.AppendChild(elem);
                    updateFileChanged = true;
                }
                else
                {  
                        updateFileChanged = true;
                        commentNode.InnerText = updateStatusForTranslation.NewStatus;
                }
            }

            


            
            if (updateFileChanged)
                updatedoc.Save(updatefilePath);

           

            return new Result<bool>(true);


        }

    }
}
