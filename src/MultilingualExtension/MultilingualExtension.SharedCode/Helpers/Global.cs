using System;
using System.Collections.Generic;


namespace MultilingualExtension.Shared.Helpers
{
    public static class Globals
    {
        //Constants for status on translation
        public const string STATUS_COMMENT_NEW = "New";
        public const string STATUS_COMMENT_NEED_REVIEW = "Need review";
        public const string STATUS_COMMENT_NO_TRANSLATION = "No transaltion";
        public const string STATUS_COMMENT_FINAL = "Final";

        //Constants for saved propertys
        public const string PROP_MSOFTENDPOINT = "MsoftEndpoint";
        public const string PROP_MSOFTLOCATION = "MsoftLocation";
        public const string PROP_MSOFTKEY = "MsoftKey";
        public const string PROP_TRANSLATIONSERVICE = "TranslationServcie";
        public const string PROP_MASTERLANGUAGECODE = "MasterLanguageCode";
        public const string PROP_ADDCOMMENTNODEMASTERRESX = "AddCommentNodeMasterResx";
        public const string PROP_EXPORTFILETYPEX = "ExportFileType";

        //App texts
        public const string Multilingual_Extension_Title = "Multilingual Extension";
        public const string Master_Language_Code_Title = "Multilingual Extension:";
        public const string Add_Comment_code_To_Master_Title = "Add comment node to master Resx file on sync";
        public const string Export_File_Type_Title = "Export file type:";
        public const string CSV_File = "CSV file";
        public const string Excel_File = "Excel file";
        public const string Google_Translate_Free = "Google translate free (max 100/h)";
        public const string Google_Translate_Free_Info = "You will only be allowed to translate about 100 words per hour using the free Google API";
        public const string Microsoft_Translation = "Microsoft Translation";
        public const string Microsoft_Translation_Info = "You will need a Azure Cognitive Service (texttranslation service)";
        public const string Endpoint_Title = "Endpoint:";
        public const string Location_Title = "Location:";
        public const string Key_Title = "Key:";
        public const string Save = "Save";
        public const string Close = "Close";

        public const string Export_Rows_Info = "Export rows where comment is 'New' or 'Need review'";
        public const string Export_All_Files_Title = "Export language files";
        public const string Export_Seleted_File_Title = "Export this language file";

        public const string Import_Rows_Info = "Import rows where comment is 'Final'";
        public const string Import_Translation_Title = "Import translations";

        public const string Show_Setting_Title = "Multilingual Settings";

        public const string Translate_Rows_Info = "Translate rows where comment has value 'New'";
        public const string Translate_All_Files_Title = "Translate all language files";
        public const string Translate_Seleted_File_Title = "Translate this language file";

        public const string Synchronize_Rows_Info = "Synchronizes lines from master Resx file.";
        public const string Synchronize_All_Files_Title = "Sync all language files with this";
        public const string Synchronize_Seleted_File_Title = "Sync this language file";

        public const string List_Unused_Translatations = "List unused translations";

        //resw text
        public const string Resw_Masterlang_Need_Exist_As_Folder = "Your Master language from 'Tools/Multilingual settings' needs to exist as a folder under Strings folder.";
        public const string Resw_Masterlang_Need_Be_Format_xx_XX = "When you work with Resw files the master language code need to be in the format of 'en-US'";
    }


}
