using System;
using System.Collections.Generic;
using System.Text;

namespace MultilingualExtension.Shared.Models
{
    public class Res_Files
    {
        public string MasterFilename { get; set; }
        public string MasterFilepath { get; set; }
        public List<String> UpdateFilepaths { get; set; }
        public Res_Files()
        {
            UpdateFilepaths = new List<string>();

        }
    }
}
