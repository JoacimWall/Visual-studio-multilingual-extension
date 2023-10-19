using System;
using MultilingualClient.Controls.TreeView;
using MultilingualClient.Controls.TreeView.Model;

namespace MultilingualClient.ViewModels;

	public class MainViewModel : BaseViewModel
{
		private IFileService fileService;
		public MainViewModel(IFileService fileService)
		{
			this.fileService = fileService;
		}

    public ICommand TestCommand => new Command(async () => await Test());
    private ObservableCollection<TreeViewNode> nodes;
    public ObservableCollection<TreeViewNode> Nodes
    {
        get { return nodes; }
        set { SetProperty(ref nodes, value); }
    }
    private async Task Test()
    {
			var test = await this.fileService.GroupData();
    }
    public override async Task OnAppearingAsync()
    {
        var xamlItemGroups = await fileService.GroupData();

        var rootNodes = ProcessXamlItemGroups(xamlItemGroups);

        foreach (var node in rootNodes)
        {
            var xamlItemGroup = (XamlItemGroup)node.BindingContext;
        }

        Nodes = rootNodes;

        await base.OnAppearingAsync();
    }
    private static ObservableCollection<TreeViewNode> ProcessXamlItemGroups(XamlItemGroup xamlItemGroups)
    {
        var rootNodes = new ObservableCollection<TreeViewNode>();

        foreach (var xamlItemGroup in xamlItemGroups.Children.OrderBy(xig => xig.Name))
        {

            var label = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.Black
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
    private static void CreateXamlItem(IList<TreeViewNode> children, XamlItem xamlItem)
    {
        var label = new Label
        {
            VerticalOptions = LayoutOptions.Center,
            TextColor = Colors.Black
        };
        label.SetBinding(Label.TextProperty, "Name");

        MenuFlyout menuElements = new MenuFlyout();
        MenuFlyoutItem item1 = new MenuFlyoutItem() { Text = "menu1" };
        MenuFlyoutItem item2 = new MenuFlyoutItem() { Text = "menu2" };
        menuElements.Add(item1);
        menuElements.Add(item2);
        FlyoutBase.SetContextFlyout(label, menuElements);

        var xamlItemTreeViewNode = CreateTreeViewNode(xamlItem, label, true);
        children.Add(xamlItemTreeViewNode);
    }
    private static TreeViewNode CreateTreeViewNode(object bindingContext, Label label, bool isItem)
    {
        var node = new TreeViewNode
        {
            BindingContext = bindingContext,
            Content = new StackLayout
            {
                Children =
                    {
                        new ResourceImage
                        {
                            Resource = isItem? "MultilingualClient.Resources.EmbeddedImages.Item.png" :"MultilingualClient.Resources.EmbeddedImages.FolderOpen.png" ,
                            HeightRequest= 20,
                            WidthRequest = 20
                        },
                        label
                    },
                Orientation = StackOrientation.Horizontal
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
            Content = new ResourceImage
            {
                Resource = isLeafNode ? "MultilingualClient.Resources.EmbeddedImages.Blank.png" : "MultilingualClient.Resources.EmbeddedImages.FolderOpen.png",
                HeightRequest = 20,
                WidthRequest = 20
            };
        }
        else
        {
            Content = new ResourceImage
            {
                Resource = node.IsExpanded ? "MultilingualClient.Resources.EmbeddedImages.OpenGlyph.png" : "MultilingualClient.Resources.EmbeddedImages.CollpsedGlyph.png",
                HeightRequest = 20,
                WidthRequest = 20
            };
        }
    }

}
