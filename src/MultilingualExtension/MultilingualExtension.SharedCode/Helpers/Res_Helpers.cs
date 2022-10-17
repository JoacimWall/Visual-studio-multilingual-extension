using System;
using System.IO;
using MultilingualExtension.Shared.Models;
using NPOI.SS.Formula.Functions;

namespace MultilingualExtension.Shared.Helpers
{
    public static class Res_Helpers
    {
        public static Result<Res_Fileinfo> FileInfo(string masterLanguageCode, string selectedFilename)
        {
            Res_Fileinfo info = new Res_Fileinfo();
            info.Filename = selectedFilename;
            string folderSeperator = Environment.OSVersion.Platform == PlatformID.Win32NT ? "\\" : "/";
            

            var result = RegExHelper.ValidateFileTypeIsResw(selectedFilename);
            //reswFile
            if (result.Success)
            {
                info.IsResW = true;
                int folderindex = selectedFilename.LastIndexOf(folderSeperator);

                string currentFolderPath = selectedFilename.Substring(0, folderindex);
                string languageFilename = selectedFilename.Substring(folderindex + 1);

                //validate if folder name is same as masterlanguage 
                if (currentFolderPath.EndsWith(masterLanguageCode))
                {
                    info.MasterFilename = languageFilename;
                    info.IsMasterFile = true;
                }
                else
                {
                    info.MasterFilename = languageFilename;
                    info.IsMasterFile = false;
                }
                int folderindexlang = selectedFilename.LastIndexOf( folderSeperator, folderindex - 1);
                string foldernameLaguage = selectedFilename.Substring(folderindexlang+1,folderindex - (folderindexlang+1));
                info.LanguageBase = foldernameLaguage.Substring(0, 2);
                if (foldernameLaguage.Length == 5)
                    info.LanguageCulture = foldernameLaguage.Substring(3, 2);
                return new Result<Res_Fileinfo>(info);
            }
            var resultIsResx = RegExHelper.ValidateFileTypeIsResx(selectedFilename);
            //resxFile
            if (resultIsResx.Success)
            {
                info.IsResx = true;
                //resx files
                var checkfile = RegExHelper.ValidateFilenameIsTargetType(selectedFilename);
                if (!checkfile.Success)
                {

                    info.IsMasterFile = true;
                    int folderindex = selectedFilename.LastIndexOf(folderSeperator);
                    string languageFilename = selectedFilename.Substring(folderindex + 1);
                    info.MasterFilename = languageFilename;
                    info.LanguageBase = masterLanguageCode.Substring(0, 2);
                    if (masterLanguageCode.Length == 5)
                        info.LanguageCulture = masterLanguageCode.Substring(3, 2);
                }
                else
                {
                    info.IsMasterFile = false;
                    string masterPath = selectedFilename.Substring(0, checkfile.Index) + ".resx";
                    int folderindex = masterPath.LastIndexOf(folderSeperator);
                    info.MasterFilename = masterPath.Substring(folderindex + 1);


                    info.LanguageBase = checkfile.Value.Substring(1, 2);
                    if (checkfile.Value.Length == 11)
                        info.LanguageCulture = checkfile.Value.Substring(4, 2);
                    var checkfileMasterTarget = RegExHelper.ValidateFilenameIsMasterTargetType(selectedFilename);
                    if (checkfileMasterTarget.Success)
                        info.IsTargetMasterFile = true;
      
                }
            }
            return new Result<Res_Fileinfo>(info);
        }
    }
}
