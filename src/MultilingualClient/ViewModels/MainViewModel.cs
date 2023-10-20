using System;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using Microsoft.Maui;
using Microsoft.Maui.Graphics.Text;
using MultilingualClient.Controls.TreeView;
using MultilingualClient.Controls.TreeView.Model;
using MultilingualClient.Messages;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Shared.Models;
using MultilingualExtension.Shared.Services;

namespace MultilingualClient.ViewModels;

public class MainViewModel : BaseViewModel
{
    private IFileService fileService;
    private ISettingsService settingsService;
    IStatusPadLoger statusPadLoger;
    public Label LogText;
    private string currentOpenFilePath = "";
    private string text;
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
                using StreamReader reader = new StreamReader(fileStream,System.Text.Encoding.Default);
               
                var c = await reader.ReadToEndAsync();
              
               EditFileText = c;
                //text = c;
            }
            catch (Exception ex)
            {
                // Handle this as you will 
            }

        });

    }

    public ICommand UpdateFilesCommand => new Microsoft.Maui.Controls.Command(async (e) => await UpdateFiles(e));
    public ICommand TranslateFilesCommand => new Microsoft.Maui.Controls.Command(async (e) => await TranslateFiles(e));
    public ICommand ImportFileCommand => new Microsoft.Maui.Controls.Command(async (e) => await ImportFile(e));
    public ICommand SaveCommnd => new Microsoft.Maui.Controls.Command(async () => await Save());

    private async Task Save()
    {
        using var dlg = DialogService.GetProgress("");
        using FileStream outputStream = System.IO.File.OpenWrite(currentOpenFilePath);
        using StreamWriter streamWriter = new StreamWriter(outputStream,System.Text.Encoding.Default);
        await streamWriter.WriteAsync(editFileText);
       // await streamWriter.WriteAsync(text);
    }

    //commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW), Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW);
    //commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEW), Globals.STATUS_COMMENT_NEW);
    //commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEED_REVIEW), Globals.STATUS_COMMENT_NEED_REVIEW);
    //commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_FINAL), Globals.STATUS_COMMENT_FINAL);
    //commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_ALL_ROWS), Globals.STATUS_COMMENT_ALL_ROWS);
    public ICommand ExportFileNewOrNeeReviewCommand => new Microsoft.Maui.Controls.Command(async (e) => await ExportFile(e, Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW));
    public ICommand ExportFileNewCommand => new Microsoft.Maui.Controls.Command(async (e) => await ExportFile(e, Globals.STATUS_COMMENT_NEW));
    public ICommand ExportFileNeedReviewCommand => new Microsoft.Maui.Controls.Command(async (e) => await ExportFile(e, Globals.STATUS_COMMENT_NEED_REVIEW));
    public ICommand ExportFileFinalCommand => new Microsoft.Maui.Controls.Command(async (e) => await ExportFile(e, Globals.STATUS_COMMENT_FINAL));
    public ICommand ExportFileAllCommand => new Microsoft.Maui.Controls.Command(async (e) => await ExportFile(e, Globals.STATUS_COMMENT_ALL_ROWS));


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
            //MultilingualExtension.StatusPad.Instance.FocusPad();
            SyncFileService syncFileService = new SyncFileService();
            //var dte = ServiceProvider.GetService(typeof(DTE)) as DTE2;
            //var path = IdeApp.Workspace.CurrentSelectedSolution.FileName;
            //var projPath = System.IO.Path.GetDirectoryName(path);
            //ISettingsService settingsService = new Services.SettingsService(projPath);
            //await IdeApp.Workbench.SaveAllAsync();

            //ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            //Dummy for mac 

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
    public override async Task OnAppearingAsync()
    {
        using var dlg = DialogService.GetProgress("");
        await Task.Delay(300);
        await SetNodes();
        statusPadLoger = new StatusPadLoger(LogText);
        await base.OnAppearingAsync();
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
                        //new ResourceImage
                        //{
                        //    Resource = isItem? "MultilingualClient.Resources.EmbeddedImages.Item.png" :"MultilingualClient.Resources.EmbeddedImages.FolderOpen.png" ,
                        //    HeightRequest= 20,
                        //    WidthRequest = 20
                        //},
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

            //Content = new ResourceImage
            //{
            //    Resource = isLeafNode ? "MultilingualClient.Resources.EmbeddedImages.Blank.png" : "MultilingualClient.Resources.EmbeddedImages.FolderOpen.png",
            //    HeightRequest = 20,
            //    WidthRequest = 20
            //};
        }
        else
        {
            Content = new Label { FontFamily = "AppIconFont", FontSize = 20, Text = node.IsExpanded ? AppIconFont.Angle_down : AppIconFont.Angle_right };
            //Content = new ResourceImage
            //{
            //    Resource = node.IsExpanded ? "MultilingualClient.Resources.EmbeddedImages.OpenGlyph.png" : "MultilingualClient.Resources.EmbeddedImages.CollpsedGlyph.png",
            //    HeightRequest = 20,
            //    WidthRequest = 20
            //};
        }
    }

}
