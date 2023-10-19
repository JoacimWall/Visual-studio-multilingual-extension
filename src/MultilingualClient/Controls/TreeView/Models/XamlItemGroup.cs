namespace MultilingualClient.Controls.TreeView.Model;

[Serializable]
public class XamlItemGroup
{
    public List<XamlItemGroup> Children { get; } = new List<XamlItemGroup>();
    public List<XamlItem> XamlItems { get; } = new List<XamlItem>();

    public string Name { get; set; }
    public string FullPath { get; set; }
}
