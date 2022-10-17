using System;
using Gdk;
using Gtk;
using MonoDevelop.Ide;
using MultilingualExtension;
using MultilingualExtension.Services;
using MultilingualExtension.Shared.Helpers;

namespace MultilingualExtension
{
    public class SettingsWindow : Gtk.Window
    {
        Entry entryMasterLanguageCode;
        RadioButton radioExportFiletypeCsv;
        RadioButton radioExportFiletypeExcel;
        RadioButton radiobuttonGoogle;
        RadioButton radiobuttonMicrosoft;
        Entry entryMsoftEndpoint;
        Entry entryMsoftLocation;
        Entry entryMsoftKey;
        CheckButton checkButtonAddComment;
        Services.SettingsService SettingsService;
        //static GLib.SList group = null;

        public SettingsWindow() : base(Globals.Multilingual_Extension_Title)
        {
            var path = IdeApp.Workspace.CurrentSelectedSolution.FileName;
            var projPath = System.IO.Path.GetDirectoryName(path);
            
            SettingsService = new Services.SettingsService(projPath);
            this.DeleteEvent += delegate { HideAll(); };
            SetDefaultSize(590, 300);
            SetPosition(Gtk.WindowPosition.Center);
            DeleteEvent += delegate { Application.Quit(); };

            VBox vbox = new VBox(false, 10);
            vbox.BorderWidth = 10;
            //box1.PackStart(box2, true, true, 0);
            vbox.Show();

            //Master language code
            Label labelMasterLanguageCode = new Label(Globals.Master_Language_Code_Title);
            entryMasterLanguageCode = new Entry();
            entryMasterLanguageCode.SetSizeRequest(580, -1);

            //Add Status/translation to master file
             checkButtonAddComment = new CheckButton(Globals.Add_Comment_code_To_Master_Title);
            checkButtonAddComment.Active = true;

            //Export file type
            Label labelExportFileType = new Label(Globals.Export_File_Type_Title);
            radioExportFiletypeCsv = new RadioButton(null, Globals.CSV_File);
            radioExportFiletypeCsv.Active = true;
            radioExportFiletypeExcel = new RadioButton(radioExportFiletypeCsv, Globals.Excel_File);

            Separator separator1 = new Gtk.HSeparator();
            separator1.SetSizeRequest(580, -1);

            radiobuttonGoogle = new RadioButton(null, Globals.Google_Translate_Free);
            radiobuttonGoogle.Active = true;

            Label labelInfoGoogle = new Label(Globals.Google_Translate_Free_Info);

            Separator separator2 = new Gtk.HSeparator();
            separator2.SetSizeRequest(580, -1);

            radiobuttonMicrosoft = new RadioButton(radiobuttonGoogle, Globals.Microsoft_Translation);

            Label labelInfoMsoft = new Label(Globals.Microsoft_Translation_Info);

            //Endpoint
            Label labelEndpointMsoft = new Label(Globals.Endpoint_Title);
            entryMsoftEndpoint = new Entry();
            entryMsoftEndpoint.SetSizeRequest(580, -1);

            //location
            Label labelLocationMsoft = new Label(Globals.Location_Title);
            entryMsoftLocation = new Entry();
            entryMsoftLocation.SetSizeRequest(580, -1);

            //Key
            Label labelKeyMsoft = new Label(Globals.Key_Title);
            entryMsoftKey = new Entry();
            entryMsoftKey.SetSizeRequest(580, -1);
            entryMsoftKey.InvisibleChar = '*';
            entryMsoftKey.Visibility = false;

            Button save = new Button(Globals.Save);
            save.SetSizeRequest(70, 30);
            save.Clicked += Save_Clicked;

            Button close = new Button(Globals.Close);
            close.SetSizeRequest(70, 30);
            close.Clicked += Close_Clicked;

            Fixed fix = new Fixed();


            int xleft = 5;
            int xleftLab = 10;
            int topValue = 10;
            int rowHeightEntry = 30;
            int rowHeightLabel = 15;
            int rowHeightSeparator = 10;
            int rowHeightRadioButton = 35;

            fix.Put(labelMasterLanguageCode, xleftLab, topValue);
            fix.Put(entryMasterLanguageCode, xleft, topValue += rowHeightLabel);

            
            fix.Put(labelExportFileType, xleftLab, topValue += rowHeightEntry);
            fix.Put(radioExportFiletypeCsv, xleftLab, topValue + rowHeightLabel);
            fix.Put(radioExportFiletypeExcel, xleftLab + 100, topValue += rowHeightLabel);

            fix.Put(checkButtonAddComment, xleft, topValue += rowHeightRadioButton);

            fix.Put(separator1, xleft, topValue += rowHeightEntry);

            fix.Put(radiobuttonGoogle, xleft, topValue += rowHeightSeparator);
            fix.Put(labelInfoGoogle, xleftLab, topValue += rowHeightRadioButton);
            fix.Put(separator2, xleft, topValue += rowHeightLabel + 10);
            fix.Put(radiobuttonMicrosoft, xleft, topValue += rowHeightSeparator);
            fix.Put(labelInfoMsoft, xleftLab, topValue += rowHeightRadioButton);

            fix.Put(labelEndpointMsoft, xleftLab, topValue += rowHeightLabel + 10);
            fix.Put(entryMsoftEndpoint, xleft, topValue += rowHeightLabel);

            fix.Put(labelLocationMsoft, xleftLab, topValue += rowHeightEntry);
            fix.Put(entryMsoftLocation, xleft, topValue += rowHeightLabel);

            fix.Put(labelKeyMsoft, xleftLab, topValue += rowHeightEntry);
            fix.Put(entryMsoftKey, xleft, topValue += rowHeightLabel);

            fix.Put(save, 435, topValue += 30);
            fix.Put(close, 510, topValue);

            Add(fix);

            entryMasterLanguageCode.Text = SettingsService.ExtensionSettings.MasterLanguageCode;
            //get settings Google is default value 1 
            if (SettingsService.ExtensionSettings.TranslationService == 2)
            {
                radiobuttonMicrosoft.Active = true;
            }
            //get settings csv is default value 1 
            if (SettingsService.ExtensionSettings.ExportFileType == 2)
            {
                radioExportFiletypeExcel.Active = true;
            }

            entryMsoftEndpoint.Text = SettingsService.ExtensionSettings.TranslationServiceMsoftEndpoint;
            entryMsoftLocation.Text = SettingsService.ExtensionSettings.TranslationServiceMsoftLocation;
            entryMsoftKey.Text = SettingsService.ExtensionSettings.TranslationServiceMsoftKey;
            checkButtonAddComment.Active = SettingsService.ExtensionSettings.ExportMasterFileOnExport;
            
            ShowAll();

        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            HideAll();
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            SettingsService.ExtensionSettings.MasterLanguageCode = entryMasterLanguageCode.Text;
            if (radioExportFiletypeCsv.Active)
            {
                SettingsService.ExtensionSettings.ExportFileType = 1;
            }
            else
            {
                SettingsService.ExtensionSettings.ExportFileType = 2;
            }

            SettingsService.ExtensionSettings.ExportMasterFileOnExport = checkButtonAddComment.Active;
           
            if (radiobuttonGoogle.Active)
            {
                SettingsService.ExtensionSettings.TranslationService = 1;
            }
            else
            {
                SettingsService.ExtensionSettings.TranslationService = 2;
            }

            SettingsService.ExtensionSettings.TranslationServiceMsoftEndpoint = entryMsoftEndpoint.Text;
            SettingsService.ExtensionSettings.TranslationServiceMsoftLocation = entryMsoftLocation.Text;
            SettingsService.ExtensionSettings.TranslationServiceMsoftKey = entryMsoftKey.Text;
            HideAll();
        }
        
        }
}
