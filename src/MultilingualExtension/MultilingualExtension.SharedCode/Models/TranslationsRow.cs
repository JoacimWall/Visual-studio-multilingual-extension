﻿using FileHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultilingualExtension.Shared.Models
{
    /// <summary>
    /// Layout for a file delimited by ,
    /// </summary>
    [DelimitedRecord(";")]
    public class TranslationsRow
    {


        public string Name;


        public string SourceLanguage;

        public string TargetLanguage;

        public string Status;
    }
}
