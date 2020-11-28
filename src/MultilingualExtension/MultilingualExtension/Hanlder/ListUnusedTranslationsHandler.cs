using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Services;
using MultilingualExtension.Shared.Interfaces;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.Text;
using MonoDevelop.Core;
using MonoDevelop.Core.Text;

using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MonoDevelop.Ide.FindInFiles;
using System.Xml;

namespace MultilingualExtension
{
    public class FileProvider
    {
        public string FileName
        {
            get;
            set;
        }

        public Project Project
        {
            get;
            set;
        }

        public int SelectionStartPosition
        {
            get;
            set;
        }

        public int SelectionEndPosition
        {
            get;
            set;
        }

        public Encoding CurrentEncoding { get; private set; }

        public FileProvider(string fileName) : this(fileName, null)
        {
        }

        public FileProvider(string fileName, Project project) : this(fileName, project, -1, -1)
        {
        }

        public FileProvider(string fileName, Project project, int selectionStartPostion, int selectionEndPosition)
        {
            FileName = fileName;
            Project = project;
            SelectionStartPosition = selectionStartPostion;
            SelectionEndPosition = selectionEndPosition;
        }
        public bool SourceLoade
        {
            get;
            set;
        }
        public string Source
        {
            get;
            set;
        }
       

        internal bool GetReaderForFileName(bool readBinaryFiles = false)
        {
            try
            {
                if (!File.Exists(FileName))
                    return false;
                if (!readBinaryFiles && TextFileUtility.IsBinary(FileName))
                    return false;
                var sr = TextFileUtility.OpenStream(FileName);
                CurrentEncoding = sr.CurrentEncoding;
                Source = sr.ReadToEnd();
                SourceLoade = true;
                return true;
                
            }
            catch (Exception e)
            {
                LoggingService.LogError("Error while opening " + FileName, e);
                return false;
            }
        }
    }
    class ListUnusedTranslationsHandler : CommandHandler
    {
        public List<FileProvider> GetFiles(ProgressMonitor monitor)
        {
            List<FileProvider> returnlist = new List<FileProvider>(); 

            foreach (var project in IdeApp.Workspace.GetAllProjects())
            {
                var conf = project.DefaultConfiguration?.Selector;

            foreach (ProjectFile file in project.GetSourceFilesAsync(conf).Result)
                {
                    if ((file.Flags & ProjectItemFlags.Hidden) == ProjectItemFlags.Hidden)
                        continue;
                    if (!IdeServices.DesktopService.GetFileIsText(file.FilePath))
                        continue;
                    if (file.Subtype == Subtype.Designer)
                        continue;
                    if (!RegExHelper.IsTranslateUserFiletype(file.FilePath))
                        continue;


                    returnlist.Add(new FileProvider(file.FilePath, project as Project));
                    Console.WriteLine(file.FilePath);
                   
                }
            }
            return returnlist;
           
        }

        public static CancellationTokenSource searchTokenSource = new CancellationTokenSource();
        static Task currentTask;
        protected async override void Run()
        {

            try
            {
               
               SyncFileService syncFileService = new SyncFileService();
                ISettingsService settingsService = new Services.SettingsService();
                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                var cancelSource = new CancellationTokenSource();
                searchTokenSource = cancelSource;
                var token = cancelSource.Token;

                XmlDocument masterdoc = new XmlDocument();
                masterdoc.Load(selectedItem.FilePath);
                XmlNode rootMaster = masterdoc.DocumentElement;
                FileProvider fileProviderMaster = new FileProvider(selectedItem.FilePath);
                
                // get all names
                XmlNodeList nodeListMaster = rootMaster.SelectNodes("//data");
                List<string> dataValues = new List<string>();
                List<string> unuseddataValues = new List<string>();
                foreach (XmlNode dataMaster in nodeListMaster)
                {
                    dataValues.Add(dataMaster.Attributes.GetNamedItem("name").Value);
                }

                 currentTask = Task.Run(delegate
                {
                    using (SearchProgressMonitor searchMonitor = IdeApp.Workbench.ProgressMonitors.GetSearchProgressMonitor(true))
                    {
                        //searchMonitor.
                        var files = GetFiles(searchMonitor);
                        var results = new List<SearchResult>();
                        foreach (var transname in dataValues)
                        {
                            bool found = false;
                            foreach (var item in files)
                            {
                                if (!item.SourceLoade)
                                     item.GetReaderForFileName(false);// .GetContent<ITextBuffer>();

                                
                                var result = RegExHelper.TranslationNameExistInCode(transname, item.Source);

                                if (result.Success)
                                {
                                    found = true;
                                    break;
                                }
                        
                            }
                            if (!found)
                            {
                                unuseddataValues.Add(transname);
                                //results.Add(SearchResult.Create(transname, 1, 1));
                            }
                        }
                        fileProviderMaster.GetReaderForFileName(false);
                        foreach (var item in unuseddataValues)
                       {
                          var result =  RegExHelper.LineContainsDataName(item, fileProviderMaster.Source);
                            if (result.Success)
                            {
                                results.Add(SearchResult.Create(selectedItem.FilePath, result.Index, result.Length));
                            }
                        }
                        searchMonitor.ReportResults(results);

                    }
                });
            }
            catch (Exception ex)
            {
                MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {
                
                Console.WriteLine("Check files completed");
            }

        }
        protected override void Update(CommandInfo info)
        {

            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            string selectedFilename = selectedItem.Name;

            //validate file
            var checkfile = RegExHelper.ValidateFilenameIsTargetType(selectedFilename);
            if (!checkfile.Success)
            {
                info.Visible = true;
                info.Text = Globals.List_Unused_Translatations;
            }
            else
            {
                info.Visible = false;

            }
        }
    }


}
