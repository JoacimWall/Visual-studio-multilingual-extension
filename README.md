# Visual Studio For Mac Multilingual Extension
#### This extension is for handling multilingual support in visual studio for Mac.
#### For it to work, it requires that you start from a Resx file that is master. Then you name all the other files according to the masterfile name .xx-xx.resx. For example AppResources.resx for master and AppResources.fr-FR.resx for French.

### It has four main functions.

* Synchronization of Resx files
* Translation of texts by Google translate or Microsoft translate
* Export of translations into csv file for final translation by external stakeholder
* Import of final translations from external stakholder (this is not implemented yet) 

## Roadmapp 
* Import of csv file
* posibility to export all rows not only the one that need translations.
* Cleaning of the Resx files (find translations that are not used anymore anywhere in the code)

 ## Explanation of functions
 ### Synchronization of Resx files
 There are two ways to sync the resx files. 
 Right-click the master file and select "Sync all .xx-x.resx files with this". Then all files that are in the same folder and follow the naming standard .xx-xx.resx  will be synchronized with the master file. 
 <img width=“100” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/SyncAllFilePopUpDialog.png">
 
Right-click one of the language files (AppResources.fr-FR.resx) and select "Sync this .xx-x.resx file". This file will then be synchronized with the master file.

<img width=“100” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/SyncFilePopUpDialog.png">

### Translation of texts
By default, this extension uses Google free translation. This is limited to only 100 translations per hour. To use the Microsoft translation service, select tools / multilangual settings in the visual studio and fill in the information from Microsoft. in this dialog you can also change the master language that is en by default.

## How the extension works 
This extension use the comment field in the target resx files(AppResources.fr-FR.resx) to keep track of the status of row/translation. It has three different statuses that it can have in its comment field.
* New
* Need review
* Final

New: This status will the row get after first synchronization. if the data row not exist in the target file,
if it already exists then it gets the status "final"

Need review: This status will it get after translation from one of the translation services.

Final: This status will it get after import from csv file. You can also set this status in the target file if you don't want the row to be exported to external stakholder for translation.

## Converting from windows multilangual toolkit
if you have used the windows multilangual toolkit before you can use this. The first time you sync the master file with the target files (AppResources.fr-FR.resx) all existings rows in the target file will get status "Final"  
