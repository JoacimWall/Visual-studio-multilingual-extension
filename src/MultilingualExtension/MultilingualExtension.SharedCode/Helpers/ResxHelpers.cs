using MultilingualExtension.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MultilingualExtension.Shared.Helpers
{
    public static class ResxHelpers
    {
        public static Result<Res_Files> GetBasInfo( string selectedFilename, string masterLanguageCode)
        {
            string folderSeperator = Environment.OSVersion.Platform == PlatformID.Win32NT ? "\\" : "/";
            int folderindex = selectedFilename.LastIndexOf(folderSeperator);
            string masterFolderPath = selectedFilename.Substring(0, folderindex);
           
            Res_Files resxFiles = new Res_Files();
            var checkfile = RegExHelper.ValidateFilenameIsTargetType(selectedFilename);
            if (!checkfile.Success)
            {
                resxFiles.IsMasterFile = true;
                resxFiles.MasterFilepath = selectedFilename;
                var index = resxFiles.MasterFilepath.LastIndexOf(folderSeperator);
                resxFiles.MasterFilename = resxFiles.MasterFilepath.Substring(index + 1);

                string[] fileEntries = Directory.GetFiles(masterFolderPath);
                List<Res_Fileinfo> resx_files = new List<Res_Fileinfo>();
                foreach (string fileName in fileEntries)
                {
                    var checkfileInFolder = RegExHelper.ValidateFilenameIsTargetType(fileName);
                    if (checkfileInFolder.Success)
                    {
                        var result = Res_Helpers.FileInfo(masterLanguageCode, fileName);
                        if (result.WasSuccessful)
                            resx_files.Add(result.Model);

                    }
                }
                foreach (Res_Fileinfo info in resx_files)
                {
                    if (info.IsTargetMasterFile)
                    {
                        resxFiles.UpdateFilepaths.Add(info.Filename);
                    }
                    else
                    {   //only add culture files if there are no masterTarget files with the same language
                        var masterTargetExist = resx_files.FirstOrDefault(x => x.IsTargetMasterFile && x.LanguageBase.ToLower() == info.LanguageBase.ToLower());
                        if (masterTargetExist == null &&  info.LanguageBase.ToLower() != masterLanguageCode.Substring(0, 2).ToLower())
                            resxFiles.UpdateFilepaths.Add(info.Filename);
                    }

                }
            }
            else
            {
                resxFiles.IsMasterFile = false;
                resxFiles.MasterFilepath = selectedFilename.Substring(0, checkfile.Index) + ".resx";
                var index = resxFiles.MasterFilepath.LastIndexOf(folderSeperator);
                resxFiles.MasterFilename = resxFiles.MasterFilepath.Substring(index + 1);

                resxFiles.UpdateFilepaths.Add(selectedFilename);
            }
            return new Result<Res_Files>(resxFiles);
        }
    }
}
