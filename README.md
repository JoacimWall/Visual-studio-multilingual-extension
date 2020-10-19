# Visual Studio For Mac Multilingual Extension
### This extension is for handling multilingual support in visual studio for Mac.
### For it to work, it requires that you start from a Resx file that is master. Then you name all the other files according to the masterfile name .xx-xx.resx. For example AppResources.resx for master and AppResources.fr-FR.resx for French.

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
 
Right-click one of the language files (AppResources.fr-FR.resx) and select "Sync this .xx-x.resx file". This file will then be synchronized with the master file.
### Translation of texts
By default, this extension uses Google free translation. This is limited to only 100 translations per hour. To use the Microsoft translation service, select tools / multilangual settings in the visual studio and fill in the information from Microsoft. in this dialog you can also change the master language that is en by default.
