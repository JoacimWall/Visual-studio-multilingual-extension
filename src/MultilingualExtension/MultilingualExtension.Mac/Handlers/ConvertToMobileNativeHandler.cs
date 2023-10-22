using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MultilingualExtension.Services;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Services;
using MultilingualExtension.Shared.Services.FileConverters;

namespace MultilingualExtension
{
    class ConvertToMobileNativeHandler : CommandHandler
    {
        private static readonly Dictionary<string, ResxConverter> Converters = new Dictionary<string, ResxConverter>(StringComparer.OrdinalIgnoreCase)
        {
            { "android", ResxConverters.Android },
            { "ios", ResxConverters.iOS },
        };
        protected async override void Run(object dataItem)
        {


            try
            {
                var test = IdeApp.Workbench.Pads;
                string nativeTypeToExport = dataItem as string;
                ExportService exportService = new ExportService();
                await IdeApp.Workbench.SaveAllAsync();
                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                var path = IdeApp.Workspace.CurrentSelectedSolution.FileName;
                var projPath = System.IO.Path.GetDirectoryName(path);
                MultilingualExtension.Shared.Interfaces.ISettingsService settingsService = new MultilingualExtension.Services.SettingsService(projPath);
                string selectedFilename = selectedItem.Name;

                //Dummy for mac 
                var outputPane = new StatusPadLoger();
                ResxConverter converter;
                var res_Info = Res_Helpers.FileInfo(settingsService.ExtensionSettings.MasterLanguageCode, selectedFilename);

                string inputFolder = selectedItem.FilePath.ToString().Substring(0, selectedItem.FilePath.ToString().Count() - res_Info.Model.MasterFilename.Count());
                string outFolder;
                if (nativeTypeToExport == Globals.Generate_Android_Files_Title)
                {
                    if (Directory.Exists(settingsService.ExtensionSettings.AndroidResourcesOutPutFolder))
                        outFolder = settingsService.ExtensionSettings.AndroidResourcesOutPutFolder;
                    else
                    {
                        outputPane.WriteText(string.Format("Output folder {0} does not exist use standard", settingsService.ExtensionSettings.AndroidResourcesOutPutFolder));
                        outFolder = Path.Combine(inputFolder, "Android");
                    }
                    
                    if (Converters.TryGetValue("android", out converter))
                    {
                        outputPane.WriteText(string.Format("Start export all Android transaltions to the folder {0}", outFolder));
                        converter.Convert(inputFolder, outFolder, outputPane);
                        return;
                    }
                }
                else
                {
                    if (Directory.Exists(settingsService.ExtensionSettings.AndroidResourcesOutPutFolder))
                        outFolder = settingsService.ExtensionSettings.AndroidResourcesOutPutFolder;
                    else
                    {
                        outputPane.WriteText(string.Format("Output folder {0} does not exist use standard", settingsService.ExtensionSettings.AndroidResourcesOutPutFolder));
                        outFolder = Path.Combine(inputFolder, "Ios");
                    }
                   
                    if (Converters.TryGetValue("ios", out converter))
                    {
                        outputPane.WriteText(string.Format("Start export all iOS transaltions to the folder {0}", outFolder));
                        converter.Convert(inputFolder, outFolder, outputPane);
                        return;
                    }

                }
                


            }

            catch (Exception ex)
            {
                MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {

                Console.WriteLine("Export file completed");
            }

        }
        protected override void Update(CommandArrayInfo info)
        {

            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            string selectedFilename = selectedItem.Name;
            var commandSet = new CommandInfoSet();
            var path = IdeApp.Workspace.CurrentSelectedSolution.FileName;
            var projPath = System.IO.Path.GetDirectoryName(path);
            MultilingualExtension.Shared.Interfaces.ISettingsService settingsService = new MultilingualExtension.Services.SettingsService(projPath);

            var res_Info = Res_Helpers.FileInfo(settingsService.ExtensionSettings.MasterLanguageCode, selectedFilename);


            if (res_Info.Model.IsMasterFile)
            {
                commandSet.CommandInfos.Add(new CommandInfo(Globals.Generate_Android_Files_Title), Globals.Generate_Android_Files_Title);
                commandSet.CommandInfos.Add(new CommandInfo(Globals.Generate_Ios_Files_Title), Globals.Generate_Ios_Files_Title);

                commandSet.Text = Globals.Export_All_Files_To_Native_Title;
                info.Add(commandSet);
            }


        }

    }
}

