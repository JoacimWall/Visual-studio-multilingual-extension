using System.Xml;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Models;
using MultilingualExtension.Shared.Interfaces;
using ICG.NetCore.Utilities.Spreadsheet;
using Microsoft.Extensions.DependencyInjection;
using MultilingualExtension.Services;

namespace MultilingualExtension.Shared.Services
{
    public class ExportService
    {
        public ExportService()
        {
        }
        public async Task<Result<Boolean>> ExportToFile(string selectedFilename, string statusToExport, EnvDTE.OutputWindowPane outputPane, ISettingsService settingsService)
        {
            try
            {
                int exportFileType = settingsService.ExtensionSettings.ExportFileType;
                //------------------------- ResW files --------------------------------------------------------------------
                var checkfileResw = RegExHelper.ValidateFileTypeIsResw(selectedFilename);
                if (checkfileResw.Success)
                {
                    var resultResw = ReswHelpers.GetBasInfo(settingsService.ExtensionSettings.MasterLanguageCode, selectedFilename);
                    if (!resultResw.WasSuccessful)
                        return new Result<bool>(resultResw.ErrorMessage);

                    foreach (var updatePath in resultResw.Model.UpdateFilepaths)
                    {
                        await ExportToFileInternal(false, resultResw.Model.MasterFilepath, updatePath, resultResw.Model.MasterFilename, statusToExport, exportFileType, outputPane);
                    }

                    if (settingsService.ExtensionSettings.ExportMasterFileOnExport)
                        await ExportToFileInternal(true, resultResw.Model.MasterFilepath, resultResw.Model.MasterFilepath, resultResw.Model.MasterFilename, statusToExport, exportFileType, outputPane);
                    return new Result<bool>(true);
                }
                // -------------------- ResX files --------------------------------------------------------
                //validate file
                var checkfile = RegExHelper.ValidateFilenameIsTargetType(selectedFilename);
                var resultResx = ResxHelpers.GetBasInfo(selectedFilename, settingsService.ExtensionSettings.MasterLanguageCode);
                if (!resultResx.WasSuccessful)
                    return new Result<bool>(resultResx.ErrorMessage);

                foreach (var updatePath in resultResx.Model.UpdateFilepaths)
                {
                    var checkfileInFolder = RegExHelper.ValidateFilenameIsTargetType(updatePath);
                    if (checkfileInFolder.Success)
                        await ExportToFileInternal(false, resultResx.Model.MasterFilepath, updatePath, resultResx.Model.MasterFilename, statusToExport, exportFileType, outputPane);
                }
                //export Master
                if (settingsService.ExtensionSettings.ExportMasterFileOnExport)
                    await ExportToFileInternal(true, resultResx.Model.MasterFilepath, resultResx.Model.MasterFilepath, resultResx.Model.MasterFilename, statusToExport, exportFileType, outputPane);

                return new Result<bool>(true);


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
               
                Console.WriteLine("ExportToFileInternal file completed");
            }

        }

        private async Task<Result<Boolean>> ExportToFileInternal(bool isMasterFile, string masterPath, string updatePath, string masterFilename, string statusToExport, int exportFileType, EnvDTE.OutputWindowPane outputPane)
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

             
            }
            var checkfile = RegExHelper.GetFilenameResx(updatePath);
           


            var services = new ServiceCollection();
            services.UseIcgNetCoreUtilitiesSpreadsheet();
            var provider = services.BuildServiceProvider();
            string exportFileName = !isMasterFile ? masterFolderPath + checkfile.Value : masterFolderPath + masterFilename;

            //Get our generator and export
            //var exportGenerator = new OpenXmlSpreadsheetGenerator();cd G  
            var exportGenerator = provider.GetService<ISpreadsheetGenerator>();
            var exportDefinition = new SpreadsheetConfiguration<TranslationsRow>
            {
                RenderTitle = false,
                DocumentTitle = "Translation export",
                RenderSubTitle = false,
                DocumentSubTitle = "translation",
                ExportData = rows,
                WorksheetName = "Transaltions"
            };
            var fileContent = exportGenerator.CreateSingleSheetSpreadsheet(exportDefinition);
            System.IO.File.WriteAllBytes(exportFileName + ".xlsx", fileContent);


            return new Result<bool>(true);
        }

        
    }

}
