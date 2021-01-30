using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Shared.Models;
namespace MultilingualExtension.Services
{
    public class SettingsService : ISettingsService
    {
        
        private  RegistryKey UserPrefs;
        public SettingsService()
        {
            UserPrefs = Registry.CurrentUser.OpenSubKey("Walltec\\MultilingualExtension", true);

          
            if (UserPrefs == null)
            {
                // Value does not already exist so create it
                //RegistryKey newKey = Registry.CurrentUser.OpenSubKey("Walltec", true);
                UserPrefs = Registry.CurrentUser.CreateSubKey("Walltec\\MultilingualExtension");
                //Set default
                AddCommentNodeMasterResx = false;
                ExportFileType = 2;
                TranslationService = 1;
                MasterLanguageCode = "en-US";
                MsoftEndpoint = "";
                MsoftLocation = "";
                MsoftKey = "";
            }
        }
        public  bool AddCommentNodeMasterResx
        {
            get
            {
               return bool.Parse(UserPrefs.GetValue(Globals.PROP_ADDCOMMENTNODEMASTERRESX).ToString());
            }
            set
            {
                UserPrefs.SetValue(Globals.PROP_ADDCOMMENTNODEMASTERRESX, value);
                

            }
        }
        public  int TranslationService
        {
            get
            {
                //1= Google
                //2=Microsoift
                return int.Parse(UserPrefs.GetValue(Globals.PROP_TRANSLATIONSERVICE).ToString());

            }
            set
            {
                UserPrefs.SetValue(Globals.PROP_TRANSLATIONSERVICE, value);
            }
        }

        public  string MasterLanguageCode
        {
            get
            {
                //Default en
               return UserPrefs.GetValue(Globals.PROP_MASTERLANGUAGECODE).ToString();

            }
            set
            {
                UserPrefs.SetValue(Globals.PROP_MASTERLANGUAGECODE, value);
            }
        }

        public  string MsoftEndpoint
        {
            get
            {
                
                return UserPrefs.GetValue(Globals.PROP_MSOFTENDPOINT).ToString();

            }
            set
            {
                UserPrefs.SetValue(Globals.PROP_MSOFTENDPOINT, value);
            }
        }
        public  string MsoftLocation
        {
            get
            {
                return UserPrefs.GetValue(Globals.PROP_MSOFTLOCATION).ToString();
            }
            set
            {
                UserPrefs.SetValue(Globals.PROP_MSOFTLOCATION, value);
            }
        }
        public  string MsoftKey
        {
            get
            {
                return UserPrefs.GetValue(Globals.PROP_MSOFTKEY).ToString();
            }
            set
            {
                UserPrefs.SetValue(Globals.PROP_MSOFTKEY, value);
            }
        }

        public int ExportFileType
        {
            get
            {
                //1= Google
                //2=Microsoift
                return int.Parse(UserPrefs.GetValue(Globals.PROP_EXPORTFILETYPEX).ToString());

            }
            set
            {
                UserPrefs.SetValue(Globals.PROP_EXPORTFILETYPEX, value);
            }
        }

    }
}
