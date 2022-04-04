using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Models;
using MultilingualExtension.Shared.Interfaces;
using FileHelpers;
using FileHelpers.ExcelNPOIStorage;

namespace MultilingualExtension.Shared.Services
{
    public class ExportService
    {
        public ExportService()
        {
        }
        public async Task<Result<Boolean>> ExportToFile(string selectedFilename, string statusToExport, IProgressBar progress, ISettingsService settingsService)
        {
            try
            {
                int exportFileType = settingsService.ExportFileType;
                //------------------------- ResW files --------------------------------------------------------------------
                var checkfileResw = RegExHelper.ValidateFileTypeIsResw(selectedFilename);
                if (checkfileResw.Success)
                {
                    var resultResw = ReswHelpers.GetBasInfo(settingsService.MasterLanguageCode, selectedFilename);
                    if (!resultResw.WasSuccessful)
                        return new Result<bool>(resultResw.ErrorMessage);

                    foreach (var updatePath in resultResw.Model.UpdateFilepaths)
                    {
                        await ExportToFileInternal(false, resultResw.Model.MasterFilepath, updatePath, resultResw.Model.MasterFilename, statusToExport, exportFileType, progress);
                    }

                    if (settingsService.AddCommentNodeMasterResx)
                        await ExportToFileInternal(true, resultResw.Model.MasterFilepath, resultResw.Model.MasterFilepath, resultResw.Model.MasterFilename, statusToExport, exportFileType, progress);
                    return new Result<bool>(true);
                }
                // -------------------- ResX files --------------------------------------------------------
                //validate file
                var checkfile = RegExHelper.ValidateFilenameIsTargetType(selectedFilename);
                var resultResx = ResxHelpers.GetBasInfo(selectedFilename, settingsService.MasterLanguageCode);
                if (!resultResx.WasSuccessful)
                    return new Result<bool>(resultResx.ErrorMessage);

                foreach (var updatePath in resultResx.Model.UpdateFilepaths)
                {
                    var checkfileInFolder = RegExHelper.ValidateFilenameIsTargetType(updatePath);
                    if (checkfileInFolder.Success)
                        await ExportToFileInternal(false, resultResx.Model.MasterFilepath, updatePath, resultResx.Model.MasterFilename, statusToExport, exportFileType, progress);
                }
                //export Master
                if (settingsService.AddCommentNodeMasterResx)
                    await ExportToFileInternal(true, resultResx.Model.MasterFilepath, resultResx.Model.MasterFilepath, resultResx.Model.MasterFilename, statusToExport, exportFileType, progress);

                return new Result<bool>(true);


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                progress.HideAll();
                progress = null;
                Console.WriteLine("ExportToFileInternal file completed");
            }

        }

        private async Task<Result<Boolean>> ExportToFileInternal(bool isMasterFile, string masterPath, string updatePath, string masterFilename, string statusToExport, int exportFileType, IProgressBar progress)
        {
            string folderSeperator = Environment.OSVersion.Platform == PlatformID.Win32NT ? "\\" : "/";
            int folderindex = updatePath.LastIndexOf(folderSeperator);

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

                //commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW), Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW);
                //commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEW), Globals.STATUS_COMMENT_NEW);
                //commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEED_REVIEW), Globals.STATUS_COMMENT_NEED_REVIEW);
                //commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_FINAL), Globals.STATUS_COMMENT_FINAL);
                //commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_ALL_ROWS), Globals.STATUS_COMMENT_ALL_ROWS);
                // commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NO_TRANSLATION), Globals.STATUS_COMMENT_NO_TRANSLATION);

                    if (commentNode != null)
                {
                    bool exportNode = false;

                    switch (statusToExport)
                    {
                        case Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW:
                            exportNode = (commentNode.InnerText == Globals.STATUS_COMMENT_NEW || commentNode.InnerText == Globals.STATUS_COMMENT_NEED_REVIEW);
                            break;
                        case Globals.STATUS_COMMENT_NEW:
                            exportNode = commentNode.InnerText == Globals.STATUS_COMMENT_NEW;
                            break;
                        case Globals.STATUS_COMMENT_NEED_REVIEW:
                            exportNode = commentNode.InnerText == Globals.STATUS_COMMENT_NEED_REVIEW;
                            break;
                        case Globals.STATUS_COMMENT_FINAL:
                            exportNode = commentNode.InnerText == Globals.STATUS_COMMENT_FINAL;
                            break;
                        case Globals.STATUS_COMMENT_ALL_ROWS:
                            exportNode = (commentNode.InnerText == Globals.STATUS_COMMENT_NEW || commentNode.InnerText == Globals.STATUS_COMMENT_NEED_REVIEW || commentNode.InnerText == Globals.STATUS_COMMENT_FINAL);
                            break;
                        default:
                            break;
                    }

                    
                    if (exportNode)
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
                }

                progress.Pulse();
            }
            var checkfile = RegExHelper.GetFilenameResx(updatePath);
            var engine = new FileHelperEngine<TranslationsRow>(System.Text.Encoding.UTF8);
            string exportFileName = !isMasterFile ? masterFolderPath + checkfile.Value : masterFolderPath + masterFilename;
            if (exportFileType == 1)
            {
                //get filename
                engine.HeaderText = engine.GetFileHeader();
                engine.WriteFile(exportFileName + ".csv", rows);
            }
            else
            {
                var provider = new ExcelNPOIStorage(typeof(TranslationsRow))
                {
                    SheetName = "Translations",
                    FileName = exportFileName + ".xlsx"
                };
                char[] delimiterChars = { ';' };
                provider.ColumnsHeaders = new List<string>(engine.GetFileHeader().Split(delimiterChars));
                provider.StartRow = 0;
                provider.StartColumn = 0;
                //This trigger a system exeption if you debug on break on all arrors 
                provider.InsertRecords(rows.ToArray());
            }



            return new Result<bool>(true);
        }

        
    }

}
