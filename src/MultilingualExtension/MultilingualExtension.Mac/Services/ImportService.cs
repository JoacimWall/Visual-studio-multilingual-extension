using System.Xml;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Models;
using ICG.NetCore.Utilities.Spreadsheet;
using MultilingualExtension.Shared.Interfaces;

namespace MultilingualExtension.Shared.Services
{
    public class ImportService
    {
        public ImportService()
        {
        }

        public async Task<Result<Boolean>> ImportToResx(string selectedFilename, IStatusPadLoger outputPane, ISettingsService settingsService)
        {

            try
            {

                //validate file
                var checkfileResxCsv = RegExHelper.ValidateFilenameIsTargetTypeResx_Csv(selectedFilename);
                var checkfileResxXlsx = RegExHelper.ValidateFilenameIsTargetTypeResx_Xlsx(selectedFilename);
                var checkfileReswCsv = RegExHelper.ValidateFilenameIsTargetTypeResw_Csv(selectedFilename);
                var checkfileReswXlsx = RegExHelper.ValidateFilenameIsTargetTypeResw_Xlsx(selectedFilename);
                if (!checkfileResxCsv.Success && !checkfileResxXlsx.Success && !checkfileReswCsv.Success && !checkfileReswXlsx.Success)
                {
                    //TODO: Show message you select file have the format .sv-SE.resx.csv or .sv-SE.resx.xlsx
                    return new Result<bool>("Not valid file");

                }
                else
                {
                    //Fix for .csv or .xlsx filetype
                    int minus = checkfileResxCsv.Success || checkfileReswCsv.Success ? 4 : 5;
                    int filetype = checkfileResxCsv.Success || checkfileReswCsv.Success ? 1 : 2;
                    string updatePath = selectedFilename.Substring(0, selectedFilename.Length - minus);
                    var result = await ImportToResxInternal(selectedFilename, updatePath, filetype, outputPane);
                    var fileinfo = Res_Helpers.FileInfo(settingsService.ExtensionSettings.MasterLanguageCode, updatePath);

                    outputPane.WriteText(string.Format("Import done for {0}-{1}: {2} rows updated", fileinfo.Model.LanguageBase, fileinfo.Model.LanguageCulture, result.Model.ToString()));

                    return new Result<bool>(true);
                }

            }

            catch (Exception ex)
            {
                throw ex;

            }

        }
        private async Task<Result<int>> ImportToResxInternal(string masterPath, string updatePath, int exportFileType, IStatusPadLoger outputPane)
        {
            int importCount = 0;
            XmlDocument updatedoc = new XmlDocument();
            updatedoc.Load(updatePath);
            XmlNode rootUpdate = updatedoc.DocumentElement;
            
            var spreadsheetParser = new OpenXmlSpreadsheetParser();
            var result = spreadsheetParser.ParseDocument<TranslationsRow>(File.OpenRead(masterPath), 1, true);
            bool updatefilechanged = false;
            foreach (var record in result)
            {
                if (record.Status == Shared.Helpers.Globals.STATUS_COMMENT_FINAL)
                {
                    var dataNode = rootUpdate.SelectSingleNode("//data[@name='" + record.Name + "']");
                    if (dataNode != null)
                    {
                        updatefilechanged = true;
                        importCount++;
                        var dataNodeValue = dataNode.SelectSingleNode("value");
                        var dataNodeComment = dataNode.SelectSingleNode("comment");
                        if (dataNodeValue != null && dataNodeComment != null)
                        {
                            dataNodeValue.InnerText = record.TargetLanguage;
                            dataNodeComment.InnerText = Globals.STATUS_COMMENT_FINAL;
                        }

                    }

                }
              

            }

            if (updatefilechanged)
                updatedoc.Save(updatePath);

            return new Result<int>(importCount);

        }

    }
}
