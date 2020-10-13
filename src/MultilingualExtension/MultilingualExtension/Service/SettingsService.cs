using System;
namespace MultilingualExtension.Service
{
    public static class SettingsService
    {
        //Constants for saved propertys
         const string PROP_MSOFTENDPOINT = "MsoftEndpoint";
         const string PROP_MSOFTLOCATION = "MsoftLocation";
         const string PROP_MSOFTKEY = "MsoftKey";
         const string PROP_TRANSLATIONSERVICE = "TranslationServcie";
         const string PROP_MASTERLANGUAGECODE = "MasterLanguageCode";
     
        public static string TranslationService
        {
            get
            {
                //1= Google
                //2=Microsoift
                var service = MonoDevelop.Core.PropertyService.Get<string>(PROP_TRANSLATIONSERVICE);
                if (String.IsNullOrEmpty(service))
                    return "1";

                    return service;
            }
            set
            {
                if (value == "1" || value == "2")
                    MonoDevelop.Core.PropertyService.Set(PROP_TRANSLATIONSERVICE, value);
            }
        }

        public static string MasterLanguageCode
        {
            get
            {
                //Default en
                var value = MonoDevelop.Core.PropertyService.Get<string>(PROP_MASTERLANGUAGECODE);
                if (String.IsNullOrEmpty(value))
                    return "en";

                return value;
            }
            set
            {
                MonoDevelop.Core.PropertyService.Set(PROP_MASTERLANGUAGECODE, value);
            }
        }

        public static string MsoftEndpoint
        {
            get
            {
                //Default en
                var value = MonoDevelop.Core.PropertyService.Get<string>(PROP_MSOFTENDPOINT);
                return value;
            }
            set
            {
                MonoDevelop.Core.PropertyService.Set(PROP_MSOFTENDPOINT, value);
            }
        }
        public static string MsoftLocation
        {
            get
            {
                //Default en
                var value = MonoDevelop.Core.PropertyService.Get<string>(PROP_MSOFTLOCATION);
                return value;
            }
            set
            {
                MonoDevelop.Core.PropertyService.Set(PROP_MSOFTLOCATION, value);
            }
        }
        public static string MsoftKey
        {
            get
            {
                //Default en
                var value = MonoDevelop.Core.PropertyService.Get<string>(PROP_MSOFTKEY);
                return value;
            }
            set
            {
                MonoDevelop.Core.PropertyService.Set(PROP_MSOFTKEY, value);
            }
        }
    }
}
