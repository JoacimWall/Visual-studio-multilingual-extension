﻿using System;
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
using Microsoft.VisualStudio.RpcContracts.FileSystem;

namespace MultilingualExtension.Services
{
    public class ImportService
    {
        public ImportService()
        {
        }

        public async Task<Result<Boolean>> ImportToResx(string selectedFilename, EnvDTE.OutputWindowPane outputPane, ISettingsService settingsService)
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

                    OutputWindowHelper.WriteToOutputWindow(outputPane, string.Format("Import done for {0}-{1}: {2} rows updated", fileinfo.Model.LanguageBase, fileinfo.Model.LanguageCulture, result.Model.ToString()));
                    return new Result<bool>(result.WasSuccessful);
                }
            }

            catch (Exception ex)
            {
                throw ex;

            }

        }
        private async Task<Result<int>> ImportToResxInternal(string masterPath, string updatePath, int exportFileType, EnvDTE.OutputWindowPane outputPane)
        {
           int importCount = 0;
            XmlDocument updatedoc = new XmlDocument();
            updatedoc.Load(updatePath);
            XmlNode rootUpdate = updatedoc.DocumentElement;
            TranslationsRow[] records;
            if (exportFileType == 1)
            {
                var engine = new FileHelperEngine<TranslationsRow>();
                engine.HeaderText = engine.GetFileHeader();
                records = engine.ReadFile(masterPath);
            }
            else
            {
                var provider = new ExcelNPOIStorage(typeof(TranslationsRow))
                {
                    StartRow = 1,
                    StartColumn = 0,
                    FileName = masterPath
                };
                records = (TranslationsRow[])provider.ExtractRecords();

            }
            bool updatefilechanged = false;
            foreach (var record in records)
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
