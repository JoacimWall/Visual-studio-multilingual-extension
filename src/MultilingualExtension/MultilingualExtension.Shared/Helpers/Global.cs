using System;
using System.Collections.Generic;

namespace MultilingualExtension.Shared.Helpers
{
    public static class Globals
    {
        //Constants for status on translation
        public const string STATUS_COMMENT_NEW = "New";
        public const string STATUS_COMMENT_NEED_REVIEW = "Need review";
        public const string STATUS_COMMENT_TRANSLATED = "Translated";
        public const string STATUS_COMMENT_FINAL = "Final";

        

    }
    public class MicrosoftTranslationResponse
    {
        public List<Translations> translations { get; set; }
    }
    public class Translations
    {
        public string text { get; set; }
        public string to { get; set; }
    }
    
}
