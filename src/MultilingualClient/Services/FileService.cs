using System;
using MultilingualClient.Controls.TreeView.Model;

namespace MultilingualClient.Services;
public interface IFileService
{
    Task<XamlItemGroup> GroupData();
}

public class FileService : IFileService
{
    public FileService()
    {
    }

    public async Task<XamlItemGroup> GroupData()
    {
        // Get information about the source directory
        string path = "/Users/joacimwall/GitRepos/NbcMaui/LiberoClub/Resources";
        var dir = new DirectoryInfo(path);

        // Check if the source directory exists
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        var rootDir = new XamlItemGroup();
        rootDir.Name = dir.Name;
        rootDir.FullPath = path;
        AddCildren(dir, rootDir);


        return rootDir;
    }
    private void AddCildren(DirectoryInfo dir, XamlItemGroup parent)
    {

        //Directorys
        foreach (DirectoryInfo subDir in dir.GetDirectories())
        {
            var itemGroup = new XamlItemGroup();
            itemGroup.Name = subDir.Name;
            itemGroup.FullPath = Path.Combine(dir.FullName, subDir.Name);
            parent.Children.Add(itemGroup);
            //Recursive
            AddCildren(subDir, itemGroup);
        }
        //Files
        foreach (FileInfo file in dir.GetFiles())
        {
             XamlItem xamlItem = new XamlItem();
            xamlItem.Name = file.Name;
            xamlItem.FullPath = file.FullName;
            parent.XamlItems.Add(xamlItem);
        }

    }

}

