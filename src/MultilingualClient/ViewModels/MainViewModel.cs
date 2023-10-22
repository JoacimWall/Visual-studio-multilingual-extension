namespace MultilingualClient.ViewModels;

public class MainViewModel : BaseViewModel
{
    private IFileService fileService;
    private ISettingsService settingsService;
    IStatusPadLoger statusPadLoger;
    public Label LogText;
    private string currentOpenFilePath = "";

    public MainViewModel(IFileService fileService, ISettingsService settingsService)
    {
        this.fileService = fileService;
        this.settingsService = settingsService;
        WeakReferenceMessenger.Default.Register<StartEditMessage>(this, async (r, m) =>
        {
            try
            {
                currentOpenFilePath = m.Value;
                using Stream fileStream = System.IO.File.OpenRead(m.Value);
                using StreamReader reader = new StreamReader(fileStream, System.Text.Encoding.UTF8);

                EditFileText = await reader.ReadToEndAsync();
                //text = c;
            }
            catch (Exception ex)
            {
                // Handle this as you will 
            }

        });

    }
    public ICommand OpenCommand => new Command(async () => await OpenFolder());
    public ICommand SaveCommand => new Command(async () => await Save());

    public ICommand UpdateFilesCommand => new Command(async (e) => await UpdateFiles(e));
    public ICommand TranslateFilesCommand => new Command(async (e) => await TranslateFiles(e));
    public ICommand ImportFileCommand => new Command(async (e) => await ImportFile(e));

    public ICommand GenerateAndroidFilesCommand => new Command(async (e) => await GenerateAndroidFiles(e));
    public ICommand GenerateIosFilesCommand => new Command(async (e) => await GenerateIosFiles(e));

    public ICommand ExportFileNewOrNeeReviewCommand => new Command(async (e) => await ExportFile(e, Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW));
    public ICommand ExportFileNewCommand => new Command(async (e) => await ExportFile(e, Globals.STATUS_COMMENT_NEW));
    public ICommand ExportFileNeedReviewCommand => new Command(async (e) => await ExportFile(e, Globals.STATUS_COMMENT_NEED_REVIEW));
    public ICommand ExportFileFinalCommand => new Command(async (e) => await ExportFile(e, Globals.STATUS_COMMENT_FINAL));
    public ICommand ExportFileAllCommand => new Command(async (e) => await ExportFile(e, Globals.STATUS_COMMENT_ALL_ROWS));


    private ObservableCollection<TreeViewNode> nodes;
    public ObservableCollection<TreeViewNode> Nodes
    {
        get { return nodes; }
        set { SetProperty(ref nodes, value); }
    }
    private string editFileText;
    public string EditFileText
    {
        get { return editFileText; }
        set { SetProperty(ref editFileText, value); }
    }
    private async Task UpdateFiles(object e)
    {

        try
        {
            XamlItem selectedItem = (XamlItem)e;
            SyncFileService syncFileService = new SyncFileService();
            var result = await syncFileService.SyncFile(selectedItem.FullPath, statusPadLoger, settingsService);

            if (!result.WasSuccessful)
            {
                await DialogService.ShowAlertAsync(result.ErrorMessage, "", "Ok");

            }

        }
        catch (Exception ex)
        {

            await DialogService.ShowAlertAsync(ex.Message, "", "Ok");
        }
        finally
        {

            Console.WriteLine("Sync file completed");
        }
    }
    private async Task TranslateFiles(object e)
    {

        try
        {
            XamlItem selectedItem = (XamlItem)e;

            TranslationService translationService = new TranslationService();
            var result = await translationService.TranslateFile(selectedItem.FullPath, statusPadLoger, settingsService);

            if (!result.WasSuccessful)
            {
                await DialogService.ShowAlertAsync(result.ErrorMessage, "", "Ok");

            }

        }
        catch (Exception ex)
        {

            await DialogService.ShowAlertAsync(ex.Message, "", "Ok");
        }
        finally
        {

            Console.WriteLine("Sync file completed");
        }
    }
    private async Task ImportFile(object e)
    {

        try
        {
            XamlItem selectedItem = (XamlItem)e;

            ImportService importService = new ImportService();
            var result = await importService.ImportToResx(selectedItem.FullPath, statusPadLoger, settingsService);
            if (!result.WasSuccessful)
            {
                await DialogService.ShowAlertAsync(result.ErrorMessage, "", "Ok");

            }

        }
        catch (Exception ex)
        {

            await DialogService.ShowAlertAsync(ex.Message, "", "Ok");
        }
        finally
        {

            Console.WriteLine("Sync file completed");
        }
    }
    private async Task ExportFile(object e, string type)
    {

        try
        {
            XamlItem selectedItem = (XamlItem)e;

            ExportService exportService = new ExportService();
            var result = await exportService.ExportToFile(selectedItem.FullPath, type, statusPadLoger, settingsService);
            if (!result.WasSuccessful)
            {
                await DialogService.ShowAlertAsync(result.ErrorMessage, "", "Ok");

            }

        }
        catch (Exception ex)
        {

            await DialogService.ShowAlertAsync(ex.Message, "", "Ok");
        }
        finally
        {

            Console.WriteLine("Sync file completed");
        }
    }
    private static readonly Dictionary<string, ResxConverter> Converters = new Dictionary<string, ResxConverter>(StringComparer.OrdinalIgnoreCase)
    {
        { "android", ResxConverters.Android },
        { "ios", ResxConverters.iOS },
    };

    private async Task GenerateAndroidFiles(object e)
    {

        try
        {
            XamlItem selectedItem = (XamlItem)e;

            string inputFolder = selectedItem.FullPath.Substring(0, selectedItem.FullPath.Count() - selectedItem.Name.Count());
            string outFolder;
            if (Directory.Exists(settingsService.ExtensionSettings.AndroidResourcesOutPutFolder))
                outFolder = settingsService.ExtensionSettings.AndroidResourcesOutPutFolder;
            else
            {
                statusPadLoger.WriteText(string.Format("Output folder {0} does not exist use standard", settingsService.ExtensionSettings.AndroidResourcesOutPutFolder));
                outFolder = Path.Combine(inputFolder, "Android");
            }
            ResxConverter converter;
            if (Converters.TryGetValue("android", out converter))
            {
                statusPadLoger.WriteText(string.Format("Start export all Android transaltions to the folder {0}", outFolder));
                converter.Convert(inputFolder, outFolder, statusPadLoger);
                return;
            }
           

        }
        catch (Exception ex)
        {

            await DialogService.ShowAlertAsync(ex.Message, "", "Ok");
        }
        finally
        {

            Console.WriteLine("Sync file completed");
        }
    }
    private async Task GenerateIosFiles(object e)
    {

        try
        {
            XamlItem selectedItem = (XamlItem)e;

            string inputFolder = selectedItem.FullPath.Substring(0, selectedItem.FullPath.Count() - selectedItem.Name.Count());
            string outFolder;
            if (Directory.Exists(settingsService.ExtensionSettings.IosResourcesOutPutFolder))
                outFolder = settingsService.ExtensionSettings.IosResourcesOutPutFolder;
            else
            {
                statusPadLoger.WriteText(string.Format("Output folder {0} does not exist use standard", settingsService.ExtensionSettings.AndroidResourcesOutPutFolder));
                outFolder = Path.Combine(inputFolder, "Ios");
            }
            ResxConverter converter;
            if (Converters.TryGetValue("ios", out converter))
            {
                statusPadLoger.WriteText(string.Format("Start export all Ios transaltions to the folder {0}", outFolder));
                converter.Convert(inputFolder, outFolder, statusPadLoger);
                return;
            }
            

        }
        catch (Exception ex)
        {

            await DialogService.ShowAlertAsync(ex.Message, "", "Ok");
        }
        finally
        {

            Console.WriteLine("Sync file completed");
        }
    }
    

    public override async Task OnAppearingAsync()
    {

        statusPadLoger = new StatusPadLoger(LogText);
        await base.OnAppearingAsync();
    }


    private async Task Save()
    {
        using var dlg = DialogService.GetProgress("");
        using FileStream outputStream = File.OpenWrite(currentOpenFilePath);
        using StreamWriter streamWriter = new StreamWriter(outputStream, System.Text.Encoding.UTF8);
        string output = editFileText.Replace("“", "\"").Replace("”", "\"");
        await streamWriter.WriteAsync(output);
        // await streamWriter.WriteAsync(text);
    }
    async Task OpenFolder()
    {
        var result = await FolderPicker.Default.PickAsync(MultilingualClientGlobals.CurrentRootPath, default);
        if (result.IsSuccessful)
        {
            await Toast.Make($"The folder was picked: Name - {result.Folder.Name}, Path - {result.Folder.Path}", ToastDuration.Long).Show(default);
            using var dlg = DialogService.GetProgress("");
            await Task.Delay(300);
            MultilingualClientGlobals.CurrentRootPath = result.Folder.Path;
            this.settingsService.ReInit(MultilingualClientGlobals.CurrentRootPath);
            await SetNodes();
        }
        else
        {
            await Toast.Make($"The folder was not picked with error: {result.Exception.Message}").Show(default);
        }
    }
    private async Task<bool> SetNodes()
    {
        var xamlItemGroups = await fileService.GroupData(MultilingualClientGlobals.CurrentRootPath);

        var rootNodes = new ObservableCollection<TreeViewNode>();
        var label = new Label
        {
            FontSize = 18,
            VerticalOptions = LayoutOptions.Center,
            TextColor = MultilingualClientGlobals.App.PlatformAppTheme == AppTheme.Light ? Colors.Black : Colors.White
        };
        label.SetBinding(Label.TextProperty, "Name");

        var groupTreeViewNode = CreateTreeViewNode(xamlItemGroups, label, false);

        rootNodes.Add(groupTreeViewNode);

        groupTreeViewNode.Children = ProcessXamlItemGroups(xamlItemGroups);

        foreach (var xamlItem in xamlItemGroups.XamlItems)
        {
            CreateXamlItem(groupTreeViewNode.Children, xamlItem);
        }

        foreach (var node in rootNodes)
        {
            var xamlItemGroup = (XamlItemGroup)node.BindingContext;
        }

        Nodes = rootNodes;
        return true;

    }
    private ObservableCollection<TreeViewNode> ProcessXamlItemGroups(XamlItemGroup xamlItemGroups)
    {
        var rootNodes = new ObservableCollection<TreeViewNode>();



        //children
        foreach (var xamlItemGroup in xamlItemGroups.Children.OrderBy(xig => xig.Name))
        {

            var label = new Label
            {
                FontSize = 18,
                VerticalOptions = LayoutOptions.Center,
                TextColor = MultilingualClientGlobals.App.PlatformAppTheme == AppTheme.Light ? Colors.Black : Colors.White
            };
            label.SetBinding(Label.TextProperty, "Name");

            var groupTreeViewNode = CreateTreeViewNode(xamlItemGroup, label, false);

            rootNodes.Add(groupTreeViewNode);

            groupTreeViewNode.Children = ProcessXamlItemGroups(xamlItemGroup);

            foreach (var xamlItem in xamlItemGroup.XamlItems)
            {
                CreateXamlItem(groupTreeViewNode.Children, xamlItem);
            }

        }

        return rootNodes;
    }
    private void CreateXamlItem(IList<TreeViewNode> children, XamlItem xamlItem)
    {
        var label = new Label
        {
            FontSize = 18,
            VerticalOptions = LayoutOptions.Center,
            TextColor = MultilingualClientGlobals.App.PlatformAppTheme == AppTheme.Light ? Colors.Black : Colors.White
        };
        label.SetBinding(Label.TextProperty, "Name");
        bool addMenu = false;
        MenuFlyout menuElements = new MenuFlyout();
        //Menu fileExtensions=".resx,.resw"
        if (xamlItem.Name.EndsWith(".resx") || xamlItem.Name.EndsWith(".resw"))
        {

            addMenu = true;
            var res_Info = Res_Helpers.FileInfo(ServiceHelper.GetService<ISettingsService>().ExtensionSettings.MasterLanguageCode, xamlItem.Name);

            menuElements.Add(new MenuFlyoutItem() { Text = res_Info.Model.IsMasterFile ? Globals.Synchronize_All_Files_Title : Globals.Synchronize_Seleted_File_Title, Command = UpdateFilesCommand, CommandParameter = xamlItem });
            menuElements.Add(new MenuFlyoutItem() { Text = res_Info.Model.IsMasterFile ? Globals.Translate_All_Files_Title : Globals.Translate_Seleted_File_Title, Command = UpdateFilesCommand, CommandParameter = xamlItem });
            if (res_Info.Model.IsMasterFile)
            {
                menuElements.Add(new MenuFlyoutItem() { Text = Globals.Generate_Android_Files_Title, Command = GenerateAndroidFilesCommand, CommandParameter = xamlItem });
                menuElements.Add(new MenuFlyoutItem() { Text = Globals.Generate_Ios_Files_Title, Command = GenerateIosFilesCommand, CommandParameter = xamlItem });
            }
            var flyoutSubItem = new MenuFlyoutSubItem() { Text = res_Info.Model.IsMasterFile ? Globals.Export_All_Files_Title : Globals.Export_Seleted_File_Title };
            flyoutSubItem.Add(new MenuFlyoutItem() { Text = Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW, Command = ExportFileNewOrNeeReviewCommand, CommandParameter = xamlItem });
            flyoutSubItem.Add(new MenuFlyoutItem() { Text = Globals.STATUS_COMMENT_NEW, Command = ExportFileNewCommand, CommandParameter = xamlItem });
            flyoutSubItem.Add(new MenuFlyoutItem() { Text = Globals.STATUS_COMMENT_NEED_REVIEW, Command = ExportFileNeedReviewCommand, CommandParameter = xamlItem });
            flyoutSubItem.Add(new MenuFlyoutItem() { Text = Globals.STATUS_COMMENT_FINAL, Command = ExportFileFinalCommand, CommandParameter = xamlItem });
            flyoutSubItem.Add(new MenuFlyoutItem() { Text = Globals.STATUS_COMMENT_ALL_ROWS, Command = ExportFileAllCommand, CommandParameter = xamlItem });

            menuElements.Add(flyoutSubItem);
        }
        //.csv,.xlsx
        if (xamlItem.Name.EndsWith(".csv") || xamlItem.Name.EndsWith(".xlsx"))
        {
            //validate file
            var checkfilecsv = RegExHelper.ValidateFilenameIsTargetTypeResx_Csv(xamlItem.Name);
            var checkfilecxlsx = RegExHelper.ValidateFilenameIsTargetTypeResx_Xlsx(xamlItem.Name);
            var checkfilecsv_resw = RegExHelper.ValidateFilenameIsTargetTypeResw_Csv(xamlItem.Name);
            var checkfilecxlsx_resw = RegExHelper.ValidateFilenameIsTargetTypeResw_Xlsx(xamlItem.Name);

            if (checkfilecsv.Success || checkfilecxlsx.Success || checkfilecsv_resw.Success || checkfilecxlsx_resw.Success)
            {
                addMenu = true;
                menuElements.Add(new MenuFlyoutItem() { Text = Globals.Import_Translation_Title, Command = ImportFileCommand, CommandParameter = xamlItem });
            }
        }
        if (addMenu)
        {
            FlyoutBase.SetContextFlyout(label, menuElements);
        }




        var xamlItemTreeViewNode = CreateTreeViewNode(xamlItem, label, true);
        children.Add(xamlItemTreeViewNode);
    }
    private static TreeViewNode CreateTreeViewNode(object bindingContext, Label label, bool isItem)
    {
        var node = new TreeViewNode
        {
            BindingContext = bindingContext,
            Content = new HorizontalStackLayout
            {
                Children =
                    {
                         new Label { FontFamily = "AppIconFont", FontSize = isItem ? 0 : 20,
                                    Text = isItem ? AppIconFont.Empty_plate : AppIconFont.Folder_open,
                                    TextColor = isItem ? AppColors.TransparentColor : AppColors.WhiteColor},
                        label
                    },
                Spacing = 4
            }
        };

        //set DataTemplate for expand button content
        node.ExpandButtonTemplate = new DataTemplate(() => new ExpandButtonContent { BindingContext = node });

        return node;
    }
}
public class ExpandButtonContent : ContentView
{

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        var node = (BindingContext as TreeViewNode);
        bool isLeafNode = (node.Children == null || node.Children.Count == 0);

        //empty nodes have no icon to expand unless showExpandButtonIfEmpty is et to true which will show the expand
        //icon can click and populated node on demand propably using the expand event.
        if ((isLeafNode) && !node.ShowExpandButtonIfEmpty)
        {
            Content = new Label
            {
                FontFamily = "AppIconFont",
                FontSize = 20,
                Text = isLeafNode ? AppIconFont.Empty_plate : AppIconFont.Folder_open,
                TextColor = isLeafNode ? AppColors.TransparentColor : AppColors.WhiteColor
            };


        }
        else
        {
            Content = new Label { FontFamily = "AppIconFont", FontSize = 20, Text = node.IsExpanded ? AppIconFont.Angle_down : AppIconFont.Angle_right };

        }
    }

}
