namespace MultilingualClient.Controls.TreeView;

public class TreeView : ScrollView
{
    #region Fields
    private readonly StackLayout _StackLayout = new StackLayout { Orientation = StackOrientation.Vertical };

    //TODO: This initialises the list, but there is nothing listening to INotifyCollectionChanged so no nodes will get rendered
    private ObservableCollection<TreeViewNode> _itemSource = new ObservableCollection<TreeViewNode>();
    private TreeViewNode _SelectedItem;
    #endregion

    #region Public Properties
    public static BindableProperty RootNodesProperty = BindableProperty.CreateAttached(
        propertyName: nameof(RootNodesProperty),
        returnType: typeof(object),
        declaringType: typeof(TreeView),
        defaultValue: null,
        defaultBindingMode: BindingMode.TwoWay, propertyChanged: RootNodesPropertyChanged
    );

    private static void RootNodesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TreeView targetView;

        targetView = (TreeView)bindable;
        if (targetView != null && newValue != null)
            targetView.RootNodes = ((ObservableCollection<TreeViewNode>)newValue);
        
    }

    /// <summary>
    /// The item that is selected in the tree
    /// TODO: Make this two way - and maybe eventually a bindable property
    /// </summary>
    public TreeViewNode SelectedItem
    {
        get => _SelectedItem;

        set
        {
            if (_SelectedItem == value)
            {
                return;
            }

            if (_SelectedItem != null)
            {
                _SelectedItem.IsSelected = false;
            }

            _SelectedItem = value;

            SelectedItemChanged?.Invoke(this, new EventArgs());
        }
    }

  
    public ObservableCollection<TreeViewNode> RootNodes
    {
        get => (ObservableCollection<TreeViewNode>)GetValue(RootNodesProperty); 
        set
        {
            _itemSource = value;

            if (value is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += (s, e) =>
                {
                    RenderNodes(_itemSource, _StackLayout, e, null);
                };
            }

            RenderNodes(_itemSource, _StackLayout, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), null);
        }
    }

    #endregion

    #region Events
    /// <summary>
    /// Occurs when the user selects a TreeViewItem
    /// </summary>
    public event EventHandler SelectedItemChanged;


    #endregion

    #region Constructor
    public TreeView()
    {
        Content = _StackLayout;
    }
    #endregion

    #region Private Methods
    private void RemoveSelectionRecursive(IEnumerable<TreeViewNode> nodes)
    {
        foreach (var treeViewItem in nodes)
        {
            if (treeViewItem != SelectedItem)
            {
                treeViewItem.IsSelected = false;
            }

            RemoveSelectionRecursive(treeViewItem.Children);
        }
    }
    #endregion

    #region Private Static Methods
    private static void AddItems(IEnumerable<TreeViewNode> childTreeViewItems, StackLayout parent, TreeViewNode parentTreeViewItem)
    {
        foreach (var childTreeNode in childTreeViewItems)
        {
            if (!parent.Children.Contains(childTreeNode))
            {
                parent.Children.Add(childTreeNode);
            }

            childTreeNode.ParentTreeViewItem = parentTreeViewItem;
        }
    }
    #endregion

    #region Internal Methods
    /// <summary>
    /// TODO: A bit stinky but better than bubbling an event up...
    /// </summary>
    internal void ChildSelected(TreeViewNode child)
    {
        SelectedItem = child;
        child.IsSelected = true;
        child.SelectionBoxView.Color = child.SelectedBackgroundColor;
        child.SelectionBoxView.Opacity = child.SelectedBackgroundOpacity;
        RemoveSelectionRecursive(RootNodes);
    }
    #endregion

    #region Internal Static Methods
    internal static void RenderNodes(IEnumerable<TreeViewNode> childTreeViewItems, StackLayout parent, NotifyCollectionChangedEventArgs e, TreeViewNode parentTreeViewItem)
    {
        if (e.Action != NotifyCollectionChangedAction.Add)
        {
            //TODO: Reintate this...
            //parent.Children.Clear();
            AddItems(childTreeViewItems, parent, parentTreeViewItem);
        }
        else
        {
            AddItems(e.NewItems.Cast<TreeViewNode>(), parent, parentTreeViewItem);
        }
    }
    #endregion
}