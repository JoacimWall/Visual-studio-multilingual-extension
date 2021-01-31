using System;
using MultilingualExtension.Shared.Models;

namespace MultilingualExtension.Shared.Helpers
{
    public static class Res_Helpers
    {
        public static Result<Res_Fileinfo> FileInfo(string masterLanguageCode, string selectedFilename)
        {
            Res_Fileinfo info = new Res_Fileinfo();
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
                return new Result<Res_Fileinfo>(info);
            }
            var resultIsResx = RegExHelper.ValidateFileTypeIsResx(selectedFilename);
            //reswFile
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
                }
                else
                {
                    info.IsMasterFile = false;
                    string masterPath = selectedFilename.Substring(0, checkfile.Index) + ".resx";
                    int folderindex = masterPath.LastIndexOf(folderSeperator);

                    string currentFolderPath = masterPath.Substring(0, folderindex);
                    string languageFilename = masterPath.Substring(folderindex + 1);
                    info.MasterFilename = languageFilename;

                }
            }
            return new Result<Res_Fileinfo>(info);
        }
    }
}
