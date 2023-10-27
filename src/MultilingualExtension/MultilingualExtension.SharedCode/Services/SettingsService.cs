using System.Text.Json;
using MultilingualExtension.Shared.Interfaces;

namespace MultilingualExtension.Services
{
    public class ExtensionSettings
    {
        public string MasterLanguageCode { get; set; } = "en-US";
        public int TranslationService { get; set; } = 1;
        public int ExportFileType { get; set; } = 2;
        public bool ExportMasterFileOnExport { get; set; } = true;
        public string TranslationServiceMsoftEndpoint { get; set; } = "";
        public string TranslationServiceMsoftLocation { get; set; } = "";
        public string TranslationServiceMsoftKey { get; set; } = "";
        public string AndroidResourcesOutPutFolder { get; set; } = "Android";
        public string IosResourcesOutPutFolder { get; set; } = "Ios";
        public string TranslationEnumNamespace { get; set; } = "";
    }
    public class SettingsService : ISettingsService
    {
        private string filename = "MultiLingualExtensionSettings.json";
        public ExtensionSettings ExtensionSettings { get; set; }
       
        public SettingsService()
        {
        }
        public SettingsService(string pathProj)
        {

            ReInit(pathProj);

        }
        public bool ReInit(string pathProj)
        {
            try
            {

              
                //var path = IdeApp.Workspace.CurrentSelectedProject.BaseDirectory.FullPath;
                if (File.Exists(Path.Combine(pathProj, filename)))
                {
                    ExtensionSettings defaultsettings = new ExtensionSettings();
                    this.ExtensionSettings = JsonSerializer.Deserialize<ExtensionSettings>(File.ReadAllText(Path.Combine(pathProj, filename)));
                    
                }
                else
                {
                    this.ExtensionSettings = new ExtensionSettings();
                    string jsonString = JsonSerializer.Serialize(this.ExtensionSettings);
                    File.WriteAllText(Path.Combine(pathProj, filename), jsonString);

                }
               
            }
            catch (Exception ex)
            {

            }
            return true;
        }
        
    }
}
