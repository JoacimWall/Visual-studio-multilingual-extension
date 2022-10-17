# Visual Studio For Mac/Windows Multilingual Extension
#### This extension is for handling multilingual support in visual studio.
#### For it to work, it requires that you start from a Resx or Resw file that is the master.
#### Resx files use the filename for language recognize. You name all the other files according to the masterfile name .xx-xx.resx or .xx.resx. For example AppResources.resx for master and AppResources.fr.resx for shared translations for French. And AppResources.fr-FR.resx,AppResources.fr-CA.resx if you would like to have different translation for France or Canadian culture. 

#### If you have a row in master file (AppResources.resx) that you want to be public and not translate you can add a comment value of 'No translation'.

#### Resw use the name of the folders for language recognize. If you have a masterfile in folder strings\en-US\Resources.resw then you shold put french file in strings\fr-FR\Resources.resw  

### View introduction video or download the sample app
#### This video is for Mac but the windows and mac are very similar. What changed after this was done is that you now configure by changing values in the file MultiLingualExtensionSettings.json which is created in the same folder as the solution file.
[![YouTube](https://i9.ytimg.com/vi/4OAhHpUpXmA/mq2.jpg?sqp=CLDSof8F&rs=AOn4CLAexsPpQzbUEi8Ud_7YgThQFXFwPg)](https://youtu.be/4OAhHpUpXmA)


Under src/sample you will find a Xamarin forms sample and a UWP sample. In the xamarin sample you will see how you can implement multilingual support for Xamarin and how to access the translation from code and in xaml. This is a standard implementation of multilangual suport. If you install the extension you can test the translation features of the extension.

### It has five main functions.

* Synchronization of language files
* Translation of texts by Google translate or Microsoft translate
* Export of translations into csv or xlsx file for final translation by external stakeholder
* Import of final translations from external stakholder
* List all unused translations to keep your resource files clean.

<img width=“100” height="200" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/SolutionExample.png">
<img width=“100” height="200" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/SolutionExample_win.png">

## Install for Mac
The Visual studio for Mac team has closed the signup for new developers to publish extensions. So you need to download the latest release from this page then go to visual studio mac and click "install from file" in the Exstension Mananger. Remember to restart you visual studio after install for the extension to load.  
[Latest release](https://github.com/JoacimWall/Visual-studio-multilingual-extension/releases)

## Install for Windows 
Go to Extension Manager and search for Multilingual Extension or Walltec and you will find it or use the link below.   
[Multilingual extension](https://marketplace.visualstudio.com/items?itemName=Walltec.MultilingualExtension)

## Naming convention of language files
You need to include a resx or resw file for each culture you wish to support. Resx files are added to the project in the same location as the master resx file, and must follow a strict naming convention.

The name of the resx file needs to include both the country code and language, for example:

* Resources.fr-FR.resx for French (in France)
* Resources.fr-CA.resx for French (in Canada)
* Resources.es-ES.resx for Spanish (in Spain)
* Resources.es-MX.resx for Spanish (in Mexico)

A list of culture codes
https://docs.microsoft.com/en-us/previous-versions/commerce-server/ee825488(v=cs.20)?redirectedfrom=MSDN

For resw files you use the folder structure to specify the languages. for example:  
Strings\fr-FR\Resources.resw for French (in France)  
Strings\fr-CA\Resources.resw for French (in Canada)

Remember to set your master language in the menu Tools/multilinugal settings to you preferd master language for example en-US. 


 ## Explanation of functions
 ### Setting the preferred property's for the extension
 You can change settings for the plugin in the MultiLingualExtensionSettings.json file located in the root of the solution folder. This file will be created the first time you run any function in the extension if it does not already exist. 
 <img width=“150” height="300" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/MultilingualSettingsDialog.png">
#### MasterLanguageCode
Master language that is 'en-US' by default. This is the language code for the MasterFile and is used in the trnaslation service to specify the input language to be translated.
#### TranslationService
1 = Googele (Default)

2 = Microsoft translation service

Google's translation. This is limited to only 100 translations per hour. To use the Microsoft translation service
you need fill in the information from Microsoft in the fields (TranslationServiceMsoftEndpoint,TranslationServiceMsoftLocation,TranslationServiceMsoftKey).
#### ExportFileType
your preferred export format, csv or xslt.

1 = Csv

2 = Excel (Default)

#### ExportMasterFileOnExport
If you also want to handle the status for the texts in the master resx file then sett this to true.
This will add Comment node to master Resx file on sync and also export the master Resx on export.


 ### Synchronization of Resx or Resw files
 There are two ways to sync the language files. 
 Right-click the master file and select "Sync all .xx-x.resx files with this". Then all files that are in the same folder and follow the naming standard .xx-xx.resx  will be synchronized with the master file. 

<img width=“100” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/SyncAllFilePopUpDialog.png">
 
Right-click one of the language files (AppResources.fr-FR.resx) and select "Sync this .xx-x.resx file". This file will then be synchronized with the master file.

<img width=“100” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/SyncFilePopUpDialog.png">

the synchronization will not only add translations to the language files it will also remove translation that are not present in the master file.

## Translate of Resx or Resw Files
There are two ways to translate the resx files. 
Right-click the master file and select "Translate all .xx-x.resx files".  
Then all files that are in the same folder and follow the naming standard .xx-xx.resx  will be Translated. The extension will only translate values that has comment value set to "New".  

<img width=“100” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/TranslateAllFilePopUpDialog.png">
 
Right-click one of the language files (AppResources.fr-FR.resx) and select "Translate this .xx-x.resx file".   
This file will then be Translate with the master file. The extension will only translate values that has comment value set to "New".

<img width=“100” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/TranslateFilePopUpDialog.png">


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
Right-click the master(AppResources.resx) file and select "List unused translations". The extension will search trow all .cs and .xaml files and check if the translations in the master file is used. When done it will report all translations that are not used in the app to the search result windows and prompt you with dialog asking if you want to remove all unused translations.

<img  width=“400” height="400" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/ListUnusedTransaltions.png">


### Update status for a previously translated node 
If you change the text in an existing node in the Master resx file and want to change the translation status. Right click somewhere on the <data name = 'xxxxx' node and select "Change Translation Status ..." and chose the status you would like to change to. This will change the status in all language files ((AppResources.fr-FR.resx) to the bew status. If you changed to 'New' then you can right click the master file and select translate to make new translations.    
This feature is only iplemented on mac in current version.

<img  width=“100” height="200" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/ChangeStatusOnExisting.png">

### Retranslate of a previously translated node 
If you change the text in an existing node or add a new node in the Master resx file and want to sync and translate this to all the language files. Right click somewhere on the <data name = 'xxxxx' node and select "Sync and translate this node in all .xx-xx.resx files". This will translate this node in all language files ((AppResources.fr-FR.resx) and change status to need review.
This feature is only implemented on mac in current version.

<img  width=“100” height="200" src="https://github.com/JoacimWall/Visual-studio-multilingual-extension/blob/main/Images/SyncAndTranslateThisNode.png">

## How the extension works 
This extension use the comment field in the target resx files(AppResources.fr-FR.resx) to keep track of the status of row/translation. It has three different status that it can have in it's comment field.
* New
* Need review
* Final   

New: The row will get this status after first synchronization. If the data row not exist in the target file.
if the translation already exists then it gets the status "final" this is so you can start using this extension if you alredy have translation files that are translated.

Need review: This status will it get after translation from one of the translation services.

Final: This status will it get after import from csv/xslt file where the status column bin changed to final. You can also set this status in the target file if you don't want the row to be exported to external stakeholder for translation.

To make a transaltion row public for all language you can add the value 'No translation' in the comment filed in the master resx file.  
* No translation

In this first version all files need to be in the same folder, export and import (.csv) should not be renamed if it should work when we will import translations.

## Converting from windows multilangual toolkit
if you have used the windows multilangual toolkit before you can use this. The first time you sync the master file with the target files (AppResources.fr-FR.resx) all existings rows in the target file will get status "Final". 
There is a Windows version of this extension also so you can work i projects where users are on both Mac and Windows.  
