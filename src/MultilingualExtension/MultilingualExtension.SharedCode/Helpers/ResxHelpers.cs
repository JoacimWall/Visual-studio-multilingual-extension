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
                foreach (string fileName in fileEntries)
                {
                    var checkfileInFolder = RegExHelper.ValidateFilenameIsTargetType(fileName);
                    if (checkfileInFolder.Success)
                        resxFiles.UpdateFilepaths.Add(fileName);
                }

            }
            else
            {
                resxFiles.IsMasterFile = false;
                resxFiles.MasterFilepath= selectedFilename.Substring(0, checkfile.Index) + ".resx";
                var index = resxFiles.MasterFilepath.LastIndexOf(folderSeperator);
                resxFiles.MasterFilename = resxFiles.MasterFilepath.Substring(index+1);

                resxFiles.UpdateFilepaths.Add(selectedFilename);
            }
            return new Result<Res_Files>(resxFiles);
        }
    }
}
