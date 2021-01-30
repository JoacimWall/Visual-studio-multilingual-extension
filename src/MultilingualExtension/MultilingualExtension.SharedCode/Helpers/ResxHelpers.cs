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
        public static Result<Res_Files> GetBasInfo( string selectedFilename)
        {
            Res_Files resxFiles = new Res_Files();
            var checkfile = RegExHelper.ValidateFilenameIsTargetType(selectedFilename);
            if (!checkfile.Success)
            {
                string folderSeperator = Environment.OSVersion.Platform == PlatformID.Win32NT ? "\\" : "/";
                int folderindex = selectedFilename.LastIndexOf(folderSeperator);
                string masterFolderPath = selectedFilename.Substring(0, folderindex);

                
                resxFiles.MasterFilepath = selectedFilename;
                string[] fileEntries = Directory.GetFiles(masterFolderPath);
                foreach (string fileName in fileEntries)
                {
                    var checkfileInFolder = RegExHelper.ValidateFilenameIsTargetType(fileName);
                    if (checkfileInFolder.Success)
                        resxFiles.UpdateFilepaths.Add(fileName);
                }

            }
            else
            {
                resxFiles.MasterFilepath= selectedFilename.Substring(0, checkfile.Index) + ".resx";
                resxFiles.UpdateFilepaths.Add(selectedFilename);
            }
            return new Result<Res_Files>(resxFiles);
        }
    }
}
