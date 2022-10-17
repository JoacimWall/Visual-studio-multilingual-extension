using System;
namespace MultilingualExtension.Shared.Models
{
    public class Res_Fileinfo
    {
        public bool IsResx { get; set; }
        public bool IsResW { get; set; }
        public bool IsMasterFile { get; set; } // true if it's AppResources.resx
        public bool IsTargetMasterFile { get; set; } // true if it's AppResources.fr.resx master for all frence files like fr-FR and fr-CA
        public string MasterFilename { get; set; } //AppResources.resx
        public string Filename { get; set; } //AppResources.fr-CA.resx or AppResources.fr.resx or AppResources.resx
        public string LanguageBase { get; set; } //AppResources.fr-CA.resx returns fr
        public string LanguageCulture { get; set; } //AppResources.fr-CA.resx returns CA

       // public string FilenameDisplay { get; set; } //for print i log
    }
}
