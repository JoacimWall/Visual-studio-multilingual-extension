using System;
using MultilingualExtension.Services;

namespace MultilingualExtension.Shared.Interfaces
{
    public interface ISettingsService
    {

        ExtensionSettings ExtensionSettings { get; set; }
       //int TranslationService { get; set; }
       //string MasterLanguageCode { get; set; }
       //string MsoftEndpoint { get; set; }
       //string MsoftLocation { get; set; }
       //string MsoftKey { get; set; }
       //int ExportFileType { get; set; }
    }
}
