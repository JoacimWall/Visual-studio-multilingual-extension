//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;
//using System.Xml;

//using MultilingualExtension.Shared.Helpers;

//namespace MultilingualExtension.Shared.Service
//{
//    public class ExportService
//    {
//        public ExportService()
//        {
//        }
//        public async Task<Result<Boolean>> ExportToFile(string selectedFilename, Interface.IProgressBar progress)
//        {
//            try
//            {

//                //validate file
//                var checkfile = RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
//                if (!checkfile.Success)
//                {
//                    int folderindex;

//                    if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
//                        folderindex = selectedFilename.LastIndexOf("\\");
//                    else
//                        folderindex = selectedFilename.LastIndexOf("/");


//                    string masterFolderPath = selectedFilename.Substring(0, folderindex);

//                    string[] fileEntries = Directory.GetFiles(masterFolderPath);
//                    foreach (string fileName in fileEntries)
//                    {
//                        var checkfileInFolder = RexExHelper.ValidateFilenameIsTargetType(fileName);
//                        if (checkfileInFolder.Success)
//                           await ExportToFileInternal(selectedFilename, fileName, progress);
//                    }

//                }
//                else
//                {
//                    string masterPath = selectedFilename.Substring(0, checkfile.Index) + ".resx";
//                    await ExportToFileInternal(masterPath, selectedFilename, progress);
//                }
//                return new Result<bool>(true);

//            }
//            catch (Exception ex)
//            {
//                throw ex;

//            }
//            finally
//            {
//                progress.HideAll();
//                progress = null;
//                Console.WriteLine("ExportToFileInternal file completed");
//            }

//        }

//        private async Task<Result<Boolean>> ExportToFileInternal(string masterPath, string updatePath, Interface.IProgressBar progress)
//        {


//            int folderindex;
//            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
//                folderindex = updatePath.LastIndexOf("\\");
//            else
//                folderindex = updatePath.LastIndexOf("/");

            
//            string masterFolderPath = updatePath.Substring(0, folderindex + 1);


//            XmlDocument updatedoc = new XmlDocument();
//            updatedoc.Load(updatePath);
//            XmlNode rootUpdate = updatedoc.DocumentElement;

//            XmlDocument masterdoc = new XmlDocument();
//            masterdoc.Load(masterPath);
//            XmlNode rootMaster = masterdoc.DocumentElement;
            

//            var rows = new List<TranslationsRow>();

//            // Select all nodes data
//            XmlNodeList nodeListUpdate = rootUpdate.SelectNodes("//data");
//            foreach (XmlNode dataUpdate in nodeListUpdate)
//            {
//                //check if comment exist
//                var commentNode = dataUpdate.SelectSingleNode("comment");
//                if (commentNode != null && (commentNode.InnerText == Globals.STATUS_COMMENT_NEW || commentNode.InnerText == Globals.STATUS_COMMENT_NEED_REVIEW))
//                {
//                    //Get master value
//                    XmlNode masterExist = rootMaster.SelectSingleNode("//data[@name='" + dataUpdate.Attributes.GetNamedItem("name").Value + "']");

//                    if (masterExist != null)
//                    {
//                        var masterValueNode = masterExist.SelectSingleNode("value");
//                        if (masterValueNode == null)
//                        {
//                            continue;
//                            //TODO: log error    
//                        }
//                        var updateValueNode = dataUpdate.SelectSingleNode("value");
//                        if (updateValueNode == null)
//                        {
//                            continue;
//                            //TODO: log error  

//                        }

//                        rows.Add(new TranslationsRow()
//                        {
//                            Name = dataUpdate.Attributes.GetNamedItem("name").Value,
//                            SourceLanguage = masterValueNode.InnerText,
//                            TargetLanguage = updateValueNode.InnerText,
//                            Status = commentNode.InnerText
//                        });

//                    }
//                }




//                progress.Pulse();
//            }

//            //var engine = new FileHelperEngine<TranslationsRow>(System.Text.Encoding.UTF8);
//            ////get filename
//            //var checkfile = RexExHelper.GetFilenameResx(updatePath);
//            //engine.HeaderText = engine.GetFileHeader();
//            //engine.WriteFile(masterFolderPath + checkfile.Value + ".csv", rows);

//            return new Result<bool>(true);
//        }

//    }

//}
