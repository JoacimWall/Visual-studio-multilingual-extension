using ICG.NetCore.Utilities.Spreadsheet;

namespace MultilingualExtension.Shared.Models
{
    /// <summary>
    /// Layout for a file delimited by ,
    /// </summary>
    //[DelimitedRecord(";")]
    public class TranslationsRow
    {

        [SpreadsheetImportColumn(1)]
        public string Name { get; set; }

        [SpreadsheetImportColumn(2)]
        public string SourceLanguage { get; set; }

        [SpreadsheetImportColumn(3)]
        public string TargetLanguage { get; set; }

        [SpreadsheetImportColumn(4)]
        public string Status { get; set; }
    }
}
