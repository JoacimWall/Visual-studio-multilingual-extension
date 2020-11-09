using System;
using MultilingualExtension.Shared.Helpers;
namespace MultilingualExtension.Service
{
    public  class SettingsService : MultilingualExtension.Shared.Interface.ISettingsService
    {
        

        public  bool AddCommentNodeMasterResx
        {
            get
            {
                //1= Google
                //2=Microsoift
                var addCommenttoMaster = MonoDevelop.Core.PropertyService.Get<string>(Globals.PROP_ADDCOMMENTNODEMASTERRESX);
                if (String.IsNullOrEmpty(addCommenttoMaster) || addCommenttoMaster == "1")
                    return true;

                return false;
            }
            set
            {
                if (value)
                    MonoDevelop.Core.PropertyService.Set(Globals.PROP_ADDCOMMENTNODEMASTERRESX, "1");
                else
                    MonoDevelop.Core.PropertyService.Set(Globals.PROP_ADDCOMMENTNODEMASTERRESX, "0");
            }
        }
        public int TranslationService
        {
            get
            {
                //1= Google
                //2=Microsoift
                var service = MonoDevelop.Core.PropertyService.Get<string>(Globals.PROP_TRANSLATIONSERVICE);
                if (String.IsNullOrEmpty(service))
                    return 1;

                    return int.Parse(service);
            }
            set
            {
                if (value == 1 || value == 2)
                    MonoDevelop.Core.PropertyService.Set(Globals.PROP_TRANSLATIONSERVICE, value.ToString());
            }
        }

        public  string MasterLanguageCode
        {
            get
            {
                //Default en
                var value = MonoDevelop.Core.PropertyService.Get<string>(Globals.PROP_MASTERLANGUAGECODE);
                if (String.IsNullOrEmpty(value))
                    return "en";

                return value;
            }
            set
            {
                MonoDevelop.Core.PropertyService.Set(Globals.PROP_MASTERLANGUAGECODE, value);
            }
        }

        public  string MsoftEndpoint
        {
            get
            {
                //Default en
                var value = MonoDevelop.Core.PropertyService.Get<string>(Globals.PROP_MSOFTENDPOINT);
                return value;
            }
            set
            {
                MonoDevelop.Core.PropertyService.Set(Globals.PROP_MSOFTENDPOINT, value);
            }
        }
        public  string MsoftLocation
        {
            get
            {
                //Default en
                var value = MonoDevelop.Core.PropertyService.Get<string>(Globals.PROP_MSOFTLOCATION);
                return value;
            }
            set
            {
                MonoDevelop.Core.PropertyService.Set(Globals.PROP_MSOFTLOCATION, value);
            }
        }
        public  string MsoftKey
        {
            get
            {
                //Default en
                var value = MonoDevelop.Core.PropertyService.Get<string>(Globals.PROP_MSOFTKEY);
                return value;
            }
            set
            {
                MonoDevelop.Core.PropertyService.Set(Globals.PROP_MSOFTKEY, value);
            }
        }
    }
}
