using System;
using Gtk;
using MultilingualExtension; 
namespace MultilingualExtension
{
    public class SettingsWindow : Gtk.Window
    {

        RadioButton radiobuttonGoogle;
        RadioButton radiobuttonMicrosoft;
        Entry entryMsoftEndpoint;
        Entry entryMsoftLocation;
        Entry entryMsoftKey;
        //static GLib.SList group = null;

        public SettingsWindow() : base ("Multilingual Settings")
        {
            SetDefaultSize(590, 300);
            SetPosition(Gtk.WindowPosition.Center);
            DeleteEvent += delegate { Application.Quit(); };

            VBox vbox = new VBox(false, 10);
            vbox.BorderWidth = 10;
            //box1.PackStart(box2, true, true, 0);
            vbox.Show();


           
            radiobuttonGoogle = new RadioButton(null, "Google translate free (max 100/h)");
            radiobuttonGoogle.Active = true;
           
            Label labelInfoGoogle = new Label("You will only be allowed to translate about 100 words per hour using the free Google API");
           
            Separator separator = new Gtk.HSeparator();
            separator.SetSizeRequest(580, -1);
            
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
            fix.Put(radiobuttonGoogle, xleft, 10);
            fix.Put(labelInfoGoogle, xleftLab, 40);
            fix.Put(separator, xleft, 70);
            fix.Put(radiobuttonMicrosoft, xleft, 80);
            fix.Put(labelInfoMsoft, xleftLab, 110);

            fix.Put(labelEndpointMsoft, xleftLab, 140);
            fix.Put(entryMsoftEndpoint, xleft, 155);

            fix.Put(labelLocationMsoft, xleftLab, 185);
            fix.Put(entryMsoftLocation, xleft, 200);

            fix.Put(labelKeyMsoft, xleftLab, 230);
            fix.Put(entryMsoftKey, xleft, 245);

            fix.Put(save, 435, 280);
            fix.Put(close, 510, 280);

            Add(fix);


            //get settings Google is default value 1 
            if (MonoDevelop.Core.PropertyService.Get<string>(Globals.PROP_TRANSLATIONSERVICE) == "2")
            {
                radiobuttonMicrosoft.Active = true;
            }
           

            entryMsoftEndpoint.Text = MonoDevelop.Core.PropertyService.Get<string>(Globals.PROP_MSOFTENDPOINT); 
            entryMsoftLocation.Text = MonoDevelop.Core.PropertyService.Get<string>(Globals.PROP_MSOFTLOCATION);
            entryMsoftKey.Text = MonoDevelop.Core.PropertyService.Get<string>(Globals.PROP_MSOFTKEY);

           
            ShowAll();

        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            HideAll();
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            //TODO: Save settings
            if (radiobuttonGoogle.Active)
            {
                MonoDevelop.Core.PropertyService.Set(Globals.PROP_TRANSLATIONSERVICE, "1");
            }
            else
            {
                MonoDevelop.Core.PropertyService.Set(Globals.PROP_TRANSLATIONSERVICE, "2");
            }

            MonoDevelop.Core.PropertyService.Set(Globals.PROP_MSOFTENDPOINT, entryMsoftEndpoint.Text);
            MonoDevelop.Core.PropertyService.Set(Globals.PROP_MSOFTLOCATION, entryMsoftLocation.Text);
            MonoDevelop.Core.PropertyService.Set(Globals.PROP_MSOFTKEY, entryMsoftKey.Text);
            HideAll();
        }

        void OnClicked(object sender, EventArgs args)
        {

           
        }
    }
}
