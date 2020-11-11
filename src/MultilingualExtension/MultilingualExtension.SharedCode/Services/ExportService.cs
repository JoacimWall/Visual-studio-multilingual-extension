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
        public async Task<Result<Boolean>> ExportToFile(string selectedFilename, IProgressBar progress, ISettingsService settingsService)
        {
            try
            {

                //validate file
                var checkfile = RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
                int exportFileType = settingsService.ExportFileType;
                if (!checkfile.Success)
                {
                    int folderindex;

                    if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
                        folderindex = selectedFilename.LastIndexOf("\\");
                    else
                        folderindex = selectedFilename.LastIndexOf("/");


                    string masterFolderPath = selectedFilename.Substring(0, folderindex);

                    string[] fileEntries = Directory.GetFiles(masterFolderPath);
                    foreach (string fileName in fileEntries)
                    {
                        var checkfileInFolder = RexExHelper.ValidateFilenameIsTargetType(fileName);
                        if (checkfileInFolder.Success)
                           await ExportToFileInternal(selectedFilename, fileName, exportFileType, progress);
                    }

                }
                else
                {
                    string masterPath = selectedFilename.Substring(0, checkfile.Index) + ".resx";
                    await ExportToFileInternal(masterPath, selectedFilename, exportFileType, progress);
                }
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

        private async Task<Result<Boolean>> ExportToFileInternal(string masterPath, string updatePath,int exportFileType, IProgressBar progress)
        {


            int folderindex;
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
                folderindex = updatePath.LastIndexOf("\\");
            else
                folderindex = updatePath.LastIndexOf("/");

            
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

                progress.Pulse();
            }
            var checkfile = RexExHelper.GetFilenameResx(updatePath);
            var engine = new FileHelperEngine<TranslationsRow>(System.Text.Encoding.UTF8);
            if (exportFileType == 1)
            {

                //get filename

                engine.HeaderText = engine.GetFileHeader();
                engine.WriteFile(masterFolderPath + checkfile.Value + ".csv", rows);
            }
            else
            {
                var provider = new ExcelNPOIStorage(typeof(TranslationsRow))
                {
                    SheetName = "Translations",
                    FileName = masterFolderPath + checkfile.Value + ".xlsx"
                };
                char[] delimiterChars = {';' };
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
