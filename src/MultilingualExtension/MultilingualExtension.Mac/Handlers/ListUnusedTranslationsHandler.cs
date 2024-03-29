﻿using EnvDTE;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Core.Text;
using MonoDevelop.Ide;
using MonoDevelop.Ide.FindInFiles;
using MonoDevelop.Projects;
using MultilingualExtension.Services;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Shared.Models;
using MultilingualExtension.Shared.Services;
using System.Text;
using System.Xml;


namespace MultilingualExtension;

public class ListUnusedTranslationsHandler : CommandHandler
{
    public List<FileProvider> GetFiles() //ProgressMonitor monitor
    {
        List<FileProvider> returnlist = new List<FileProvider>();

        foreach (var project in IdeApp.Workspace.GetAllProjects())
        {
            //var conf = project.DefaultConfiguration?.Selector;
            //var files = project.GetSourceFilesAsync(conf).Result; //monitor
            foreach (var file in project.Files)
            {
                Console.WriteLine(file.FilePath);
                if (file.Include == null)
                    continue;
                if ((file.Flags & ProjectItemFlags.Hidden) == ProjectItemFlags.Hidden)
                    continue;
                if (!IdeServices.DesktopService.GetFileIsText(file.FilePath))
                    continue;
                if (file.Subtype == Subtype.Designer)
                    continue;
                if (!RegExHelper.IsTranslateUserFiletype(file.FilePath))
                    continue;


                returnlist.Add(new FileProvider(file.FilePath, project));
                if (file.FilePath.FileName.Contains(".xaml.cs"))
                {
                    
                    var pathxaml = file.FilePath.ToString().Substring(0,file.FilePath.ToString().Length - 3);
                    returnlist.Add(new FileProvider(pathxaml, project));
                    Console.WriteLine(pathxaml);
                }

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
            IStatusPadLoger statusPadLoger = new StatusPadLoger();
            SyncFileService syncFileService = new SyncFileService();
            var path = IdeApp.Workspace.CurrentSelectedSolution.FileName;
            var projPath = System.IO.Path.GetDirectoryName(path);
            ISettingsService settingsService = new Services.SettingsService(projPath);
            await IdeApp.Workbench.SaveAllAsync();
            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            var cancelSource = new CancellationTokenSource();
            searchTokenSource = cancelSource;
            var token = cancelSource.Token;

            var res_Info = Res_Helpers.FileInfo(settingsService.ExtensionSettings.MasterLanguageCode, selectedItem.Name);


            XmlDocument masterdoc = new XmlDocument();
            masterdoc.Load(selectedItem.FilePath);
            XmlNode rootMaster = masterdoc.DocumentElement;




            string namspacenameMaster = res_Info.Model.MasterFilename.Substring(0, res_Info.Model.MasterFilename.Length - 4);
            // get all names
            XmlNodeList nodeListMaster = rootMaster.SelectNodes("//data");
            List<string> dataValues = new List<string>();
            currentTask = Task.Run(delegate
            {
                {
                    //using (SearchProgressMonitor searchMonitor = IdeApp.Workbench.ProgressMonitors.GetSearchProgressMonitor(true))
                    //{
                    foreach (XmlNode dataMaster in nodeListMaster)
                    {
                        dataValues.Add(dataMaster.Attributes.GetNamedItem("name").Value);
                    }
                    statusPadLoger.WriteText("Check for unused translations:");
                    var result = AnalyzeFiles(dataValues, namspacenameMaster, selectedItem); //, searchMonitor
                    if (result.Count > 0)
                    {
                        string questionMess = "You have " + dataValues.Count.ToString() + " translations strings and " + result.Count.ToString() +
                        " of them are not used in any .cs or .xaml file." + Environment.NewLine + Environment.NewLine + "To remove them from the master resx file press 'Yes'." + Environment.NewLine + "Select 'No' to just show them in the 'Multlingual Extension log'" + Environment.NewLine +
                        "Remember to select 'sync all Language ...' translation files to remove unused translations from other language files.";
                        var question = new QuestionMessage(questionMess);
                        question.Buttons.Add(new AlertButton("Yes"));
                        question.Buttons.Add(new AlertButton("No"));
                        var questionResult = MessageService.AskQuestion(question);

                        if (questionResult.Label == "Yes")
                        {
                            //Select all rows data in Updatefile to remove row that not exist in master 
                            foreach (var removeItem in result)
                            {
                                XmlNode exist = rootMaster.SelectSingleNode("//data[@name='" + removeItem + "']");
                                if (exist != null)
                                {
                                    rootMaster.RemoveChild(exist);
                                    statusPadLoger.WriteText("Removed translation: " + removeItem);
                                }
                            }
                            masterdoc.Save(selectedItem.FilePath);
                        }
                        else
                        {
                            foreach (var removeItem in result)
                                statusPadLoger.WriteText(removeItem);

                            
                        }
                    }
                    else
                    {
                        statusPadLoger.WriteText("Did not find any unused translations.");
                        MessageService.GenericAlert(new GenericMessage { Text = "Did not find any unused translations." });
                    }
                }
            }).ContinueWith((t) =>
            {
                if (t.IsFaulted)
                    MessageService.GenericAlert(new GenericMessage { Text = t.Exception.Message });
                //if (t.IsCompleted) //optionally do some work);
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
    private List<string> AnalyzeFiles(List<string> dataValues, string namspacenameMaster, ProjectFile selectedItem) //, SearchProgressMonitor searchMonitor
    {


        //searchMonitor.
        var files = GetFiles(); //searchMonitor
        var results = new List<SearchResult>();
        List<string> unuseddataValues = new List<string>();
        FileProvider fileProviderMaster = new FileProvider(selectedItem.FilePath);
        foreach (var transname in dataValues)
        {
            bool found = false;
            foreach (var item in files)
            {
                if (!item.SourceLoade)
                    item.GetReaderForFileName(false);// .GetContent<ITextBuffer>();


                var result = RegExHelper.TranslationNameExistInCode(namspacenameMaster + transname, item.Source);

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
            var result = RegExHelper.LineContainsDataName(item, fileProviderMaster.Source);
            if (result.Success)
            {
                results.Add(SearchResult.Create(selectedItem.FilePath, result.Index, result.Length));
            }
        }
        //searchMonitor.ReportResults(results);
        return unuseddataValues;








    }
    protected override void Update(CommandInfo info)
    {

        ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
        string selectedFilename = selectedItem.Name;
        var path = IdeApp.Workspace.CurrentSelectedSolution.FileName;
        var projPath = System.IO.Path.GetDirectoryName(path);
        ISettingsService settingsService = new Services.SettingsService(projPath);
        var res_Info = Res_Helpers.FileInfo(settingsService.ExtensionSettings.MasterLanguageCode, selectedFilename);

        if (res_Info.Model.IsMasterFile)
        {
            info.Visible = true;
            info.Text = Shared.Helpers.Globals.List_Unused_Translatations;
        }
        else
        {
            info.Visible = false;

        }
    }
}
public class FileProvider
{
    public string FileName
    {
        get;
        set;
    }

    public MonoDevelop.Projects.Project Project
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

    public FileProvider(string fileName, MonoDevelop.Projects.Project project) : this(fileName, project, -1, -1)
    {
    }

    public FileProvider(string fileName, MonoDevelop.Projects.Project project, int selectionStartPostion, int selectionEndPosition)
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
