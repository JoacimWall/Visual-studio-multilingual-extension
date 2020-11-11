using System;
using Gdk;
using Gtk;
using MultilingualExtension;
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

        public SettingsWindow() : base("Multilingual Settings")
        {
            SettingsService = new Services.SettingsService();
            this.DeleteEvent += delegate { HideAll(); };
            SetDefaultSize(590, 300);
            SetPosition(Gtk.WindowPosition.Center);
            DeleteEvent += delegate { Application.Quit(); };

            VBox vbox = new VBox(false, 10);
            vbox.BorderWidth = 10;
            //box1.PackStart(box2, true, true, 0);
            vbox.Show();

            //Master language code
            Label labelMasterLanguageCode = new Label("Master language code:");
            entryMasterLanguageCode = new Entry();
            entryMasterLanguageCode.SetSizeRequest(580, -1);

            //Add Status/translation to master file
             checkButtonAddComment = new CheckButton("Add Comment node to master Resx file on sync");
            checkButtonAddComment.Active = true;

            //Export file type
            Label labelExportFileType = new Label("Export file type:");
            radioExportFiletypeCsv = new RadioButton(null, "CSV file");
            radioExportFiletypeCsv.Active = true;
            radioExportFiletypeExcel = new RadioButton(radioExportFiletypeCsv, "Excel file");

            Separator separator1 = new Gtk.HSeparator();
            separator1.SetSizeRequest(580, -1);

            radiobuttonGoogle = new RadioButton(null, "Google translate free (max 100/h)");
            radiobuttonGoogle.Active = true;

            Label labelInfoGoogle = new Label("You will only be allowed to translate about 100 words per hour using the free Google API");

            Separator separator2 = new Gtk.HSeparator();
            separator2.SetSizeRequest(580, -1);

            radiobuttonMicrosoft = new RadioButton(radiobuttonGoogle, "Microsoft Translation");

            Label labelInfoMsoft = new Label("You will need a Azure Cognitive Service (texttranslation service)");

            //Endpoint
            Label labelEndpointMsoft = new Label("Endpoint:");
            entryMsoftEndpoint = new Entry();
            entryMsoftEndpoint.SetSizeRequest(580, -1);

            //location
            Label labelLocationMsoft = new Label("Location:");
            entryMsoftLocation = new Entry();
            entryMsoftLocation.SetSizeRequest(580, -1);

            //Key
            Label labelKeyMsoft = new Label("Key:");
            entryMsoftKey = new Entry();
            entryMsoftKey.SetSizeRequest(580, -1);
            entryMsoftKey.InvisibleChar = '*';
            entryMsoftKey.Visibility = false;

            Button save = new Button("Save");
            save.SetSizeRequest(70, 30);
            save.Clicked += Save_Clicked;

            Button close = new Button("Close");
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

            entryMasterLanguageCode.Text = SettingsService.MasterLanguageCode;
            //get settings Google is default value 1 
            if (SettingsService.TranslationService == 2)
            {
                radiobuttonMicrosoft.Active = true;
            }
            //get settings csv is default value 1 
            if (SettingsService.ExportFileType == 2)
            {
                radioExportFiletypeExcel.Active = true;
            }

            entryMsoftEndpoint.Text = SettingsService.MsoftEndpoint;
            entryMsoftLocation.Text = SettingsService.MsoftLocation;
            entryMsoftKey.Text = SettingsService.MsoftKey;
            checkButtonAddComment.Active = SettingsService.AddCommentNodeMasterResx;
            
            ShowAll();

        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            HideAll();
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            SettingsService.MasterLanguageCode = entryMasterLanguageCode.Text;
            if (radioExportFiletypeCsv.Active)
            {
                SettingsService.ExportFileType = 1;
            }
            else
            {
                SettingsService.ExportFileType = 2;
            }

            SettingsService.AddCommentNodeMasterResx = checkButtonAddComment.Active;
           
            if (radiobuttonGoogle.Active)
            {
                SettingsService.TranslationService = 1;
            }
            else
            {
                SettingsService.TranslationService = 2;
            }

            SettingsService.MsoftEndpoint = entryMsoftEndpoint.Text;
            SettingsService.MsoftLocation = entryMsoftLocation.Text;
            SettingsService.MsoftKey = entryMsoftKey.Text;
            HideAll();
        }
        
        }
}
