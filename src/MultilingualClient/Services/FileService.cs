using System;
using DocumentFormat.OpenXml.Wordprocessing;
using MultilingualClient.Controls.TreeView.Model;

namespace MultilingualClient.Services;
public interface IFileService
{
    Task<XamlItemGroup> GroupData(string projectpath);
}

public class FileService : IFileService
{
    public FileService()
    {
    }

    public async Task<XamlItemGroup> GroupData(string projectpath)
    {
        
        var dir = new DirectoryInfo(projectpath);

        // Check if the source directory exists
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        var rootDir = new XamlItemGroup();
        rootDir.Name = dir.Name;
        rootDir.FullPath = projectpath;
        AddCildren(dir, rootDir);


        //var test = new XamlItemGroup {  Name="Solution" };
        //test.Children.Add(rootDir);
        return rootDir;
    }
    private void AddCildren(DirectoryInfo dir, XamlItemGroup parent)
    {
        if (FolderSkip(dir.Name))
            return;

        

        //SubDirectorys
        foreach (DirectoryInfo subDir in dir.GetDirectories())
        {
            if (FolderSkip(dir.Name))
                continue;
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
            if (FileSkip(file.Name))
                continue;

            XamlItem xamlItem = new XamlItem();

            xamlItem.Name = file.Name;
            xamlItem.FullPath = file.FullName;
            parent.XamlItems.Add(xamlItem);
        }
    }
    private bool FolderSkip(string name)
    {
        if (name == "bin" || name == "obj" || name == ".vs" || name == ".vscode" || name == ".idea")
            return true;

        return false;
    }
    private bool FileSkip(string name)
    {
        if (name == ".DS_Store")
            return true;

        return false;
    }
}

