# Visual Studio For Mac/Windows Multilingual Extension
#### This extension is for handling multilingual support in visual studio.
#### For it to work, it requires that you start from a Resx file that is the master. Then you name all the other files according to the masterfile name .xx-xx.resx. For example AppResources.resx for master and AppResources.fr-FR.resx for French.

<img width=“100” height="200" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/SolutionExample.png">

### It has five main functions.

* Synchronization of Resx files
* Translation of texts by Google translate or Microsoft translate
* Export of translations into csv or xlsx file for final translation by external stakeholder
* Import of final translations from external stakholder
* List all unused translations to keep out resources files clean.

## Road map 
* Posibility to export all rows not only the one that need translations.
* Cleaning of the Resx files (find translations that are not used anymore anywhere in the code)

## Install for Mac
The extension publish function is closed for new users so you need to download the latest release from this page then got to visual studio mac and click "install from file" in the Exstension Mananger.

## Install for Windows 
Go Exstension Manager and search for Multilingual Extension or Wlltec and you will find it. 

 ## Explanation of functions
 ### Synchronization of Resx files
 There are two ways to sync the resx files. 
 Right-click the master file and select "Sync all .xx-x.resx files with this". Then all files that are in the same folder and follow the naming standard .xx-xx.resx  will be synchronized with the master file. 

<img width=“100” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/SyncAllFilePopUpDialog.png">
 
Right-click one of the language files (AppResources.fr-FR.resx) and select "Sync this .xx-x.resx file". This file will then be synchronized with the master file.

<img width=“100” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/SyncFilePopUpDialog.png">

### Translation of texts
By default, this extension uses Google free translation. This is limited to only 100 translations per hour. To use the Microsoft translation service, select tools / multilangual settings in the visual studio and fill in the information from Microsoft. In this dialog you can also change the master language that is 'en' by default.
If you also want to handle the status for the texts in the master resx file, check the 'Add Comment node to master Resx file on sync' in settings dialog.

<img width=“150” height="300" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/MultilingualSettingsDialog.png">

### Export of texts to Excel or csv file
There are two ways to export the resx files to csv or xlsx. 
Right-click the master file and select "Export all .xx-xx.resx files". Then all files that are in the same folder and follow the naming standard .xx-xx.resx will be processed and all rows with status 'New' or 'Need review' will be exported. You can change the preferd export file type in the settings dialog click tools/multilangual settings and select the preferd 'Export file type' and click save.  
 
<img  width=“100” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/ExportAllFilePopUpDialog.png">

Right-click one of the language files (AppResources.fr-FR.resx) and select "Export this .xx-x.resx file". Then all rows with status 'New' or 'Need review' will be exported to the csv file.

<img  width=“100” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/ExportFilePopUpDialog.png">

### Import of texts
Right-click one of the csv or xlsx language files (AppResources.fr-FR.resx.csv) and select "Import translation". Then all rows with status 'Final' will be imported to the resx file(AppResources.fr-FR.resx) that are in the same folder. 

<img  width=“100” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/ImportFilePopUpDialog.png">

### List unused translation
Right-click the master(AppResources.resx) file and select "List unused translations". The extension will search trow all .cs and .xaml files and check if the translations in the master file is used. When done it will report alla translations that are not used in the app to search result windows.

<img  width=“400” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/ListUnusedTransaltions.png">


### Update status for a previously translated node 
If you change the text in an existing node in the Master resx file and want to change the translation status. Right click somewhere on the <data name = 'xxxxx' node and select "Change Translation Status ..." and chose the status you would like to change to. This will change the status in all language files ((AppResources.fr-FR.resx) to the bew status. If you changed to 'New' then you can right click the master file and select translate to make new translations.    

<img  width=“100” height="200" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/ChangeStatusOnExisting.png">

### Retranslate of a previously translated node 
If you change the text in an existing node or add a new node in the Master resx file and want to sync and translate this to all the language files. Right click somewhere on the <data name = 'xxxxx' node and select "Sync and translate this node in all .xx-xx.resx files". This will translate this node in all language files ((AppResources.fr-FR.resx) and change status to need review.

<img  width=“100” height="200" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/SyncAndTranslateThisNode.png">

## How the extension works 
This extension use the comment field in the target resx files(AppResources.fr-FR.resx) to keep track of the status of row/translation. It has three different statuses that it can have in it's comment field.
* New
* Need review
* Final

New: The row will get this status after first synchronization. If the data row not exist in the target file,
if it already exists then it gets the status "final"

Need review: This status will it get after translation from one of the translation services.

Final: This status will it get after import from csv file. You can also set this status in the target file if you don't want the row to be exported to external stakholder for translation.

In this first version all files need to be in the same folder, export and import (.csv) should not be renamed if the should work when we will import translations.

## Converting from windows multilangual toolkit
if you have used the windows multilangual toolkit before you can use this. The first time you sync the master file with the target files (AppResources.fr-FR.resx) all existings rows in the target file will get status "Final". 
There is a Windows version of this extension also so you can work i projects where user are on both Mac and Windows.  
