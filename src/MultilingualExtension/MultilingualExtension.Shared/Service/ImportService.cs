using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using FileHelpers;
using MultilingualExtension.Shared.Helpers;
namespace MultilingualExtension.Shared.Service
{
    public class ImportService
    {
        public ImportService()
        {
        }

        public async Task<Result<Boolean>> ImportCsvToResx(string selectedFilename, Interface.IProgressBar progress)
        {

            try
            {

                //validate file
                var checkfile = RexExHelper.ValidateFilenameIsTargetTypeCsv(selectedFilename);
                if (!checkfile.Success)
                {
                    //TODO: Show message you select file have the format .sv-SE.resx
                    return new Result<bool>("Not valid file");

                }
                else
                {
                    string updatePath = selectedFilename.Substring(0, selectedFilename.Length - 4);
                    var reslut = await ImportCsvToResxInternal(selectedFilename, updatePath, progress);
                    return new Result<bool>(true);
                }

            }

            catch (Exception ex)
            {
                throw ex;

            }

        }

        private async Task<Result<Boolean>> ImportCsvToResxInternal(string masterPath, string updatePath, Interface.IProgressBar progress)
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
                if (record.Status == Shared.Helpers.Globals.STATUS_COMMENT_FINAL)
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
                            dataNodeComment.InnerText = Shared.Helpers.Globals.STATUS_COMMENT_FINAL;
                        }

                    }

                }
                progress.Pulse();

            }

            if (updatefilechanged)
                updatedoc.Save(updatePath);

            return new Result<bool>(true);

        }

    }

}
