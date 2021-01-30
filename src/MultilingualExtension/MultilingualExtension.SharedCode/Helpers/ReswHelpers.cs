using MultilingualExtension.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MultilingualExtension.Shared.Helpers
{
    public static class ReswHelpers
    {
        public static Result<Res_Files> GetBasInfo(string masterLanguageCode,string selectedFilename )
        {
            string folderSeperator = Environment.OSVersion.Platform == PlatformID.Win32NT ? "\\" : "/";

            //Validate that Masterlanguage format "en-US" so we can match masterfile from folder name below 
            if (masterLanguageCode.Length != 5)
                return new Result<Res_Files>("When you work with Resw files the master language code need to be in the format of 'en-US'");

            int folderindex = selectedFilename.LastIndexOf(folderSeperator);

            string currentFolderPath = selectedFilename.Substring(0, folderindex);
            string languageFilename = selectedFilename.Substring(folderindex + 1);

            folderindex = currentFolderPath.LastIndexOf(folderSeperator);

            string folderStringsPath = currentFolderPath.Substring(0, folderindex);

            Res_Files reswFiles = new Res_Files();
            reswFiles.MasterFilename = languageFilename;
            //validate if folder name is same as masterlanguage 
            if (currentFolderPath.EndsWith(masterLanguageCode))
            {
                reswFiles.MasterFilepath = selectedFilename;

                string[] dirEntries = Directory.GetDirectories(folderStringsPath);
                //Check that the masterfile eexist as folder in StringsFolder
                var resultMasterLanguageCodeFolderExist = dirEntries.Any(x => x.EndsWith(masterLanguageCode));
                if (!resultMasterLanguageCodeFolderExist)
                    return new Result<Res_Files>("Your Master language from 'Tools/Multilingual settings' needs to exist as a folder under Strings folder.");

                foreach (string dir in dirEntries)
                {

                    if (!dir.EndsWith(masterLanguageCode))
                        reswFiles.UpdateFilepaths.Add(dir + folderSeperator + reswFiles.MasterFilename);
                }
            }
            else
            {
                reswFiles.MasterFilepath = folderStringsPath + folderSeperator + masterLanguageCode + folderSeperator + reswFiles.MasterFilename;
                reswFiles.UpdateFilepaths.Add(selectedFilename);
            }
            return new Result<Res_Files>(reswFiles);
        }
    }
    
    
}
