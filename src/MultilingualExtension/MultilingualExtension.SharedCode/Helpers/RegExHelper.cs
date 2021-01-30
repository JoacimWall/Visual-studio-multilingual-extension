using System;
using System.Text.RegularExpressions;

namespace MultilingualExtension.Shared.Helpers
{
    public static class RegExHelper
    {
        public static Match ValidateFileTypeIsResw(string path)
        {
            Regex regex = new Regex(@".resw$");
            return regex.Match(path);

        }//.resx
        public static Match ValidateFileTypeIsResx(string path)
        {
            Regex regex = new Regex(@".resx$");
            return regex.Match(path);

        }//AppResources.se-SE.resx
        public static Match ValidateFilenameIsTargetType(string path)
        {
            Regex regex = new Regex(@".[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx$");
            return regex.Match(path);

        }
        public static Match ValidatePathReswIsTargetType(string path)
        {
            Regex regex = new Regex(@".[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z]$");
            return regex.Match(path);

        }//AppResources.se-SE.resx.csv
        public static Match ValidateFilenameIsTargetTypeResx_Csv(string path)
        {
            Regex regex = new Regex(@"\w+.[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx.csv$");
            return regex.Match(path);

        } //Resources.resw.csv
        public static Match ValidateFilenameIsTargetTypeResw_Csv(string path)
        {
            Regex regex = new Regex(@"\w+.resw.csv$");
            return regex.Match(path);

        }//AppResources.se-SE.resx.slsx
        public static Match ValidateFilenameIsTargetTypeResx_Xlsx(string path)
        {
            Regex regex = new Regex(@"\w+.[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx.xlsx$");
            return regex.Match(path);

        }//Resources.resw.slsx
        public static Match ValidateFilenameIsTargetTypeResw_Xlsx(string path)
        {
            Regex regex = new Regex(@"\w+.resw.xlsx$");
            return regex.Match(path);

        }
        public static Match GetFilenameResx(string path)
        {
            Regex regex = new Regex(@"\w+.[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx$");
            return regex.Match(path);

        }
        public static Match GetFilenameMasterResx(string path)
        {
            Regex regex = new Regex(@"\w+.resx$");
            return regex.Match(path);

        }
        public static Match LineContainsDataName(string line)
        { //Regex(@"[<]\bdata name\b[=][""]");
            Regex regex = new Regex(@"<data name=""");
            return regex.Match(line);

        }
        public static Match LineContainsDataName(string name, string line)
        {
            //<data name="How_Old_Are_You"
            Regex regex = new Regex(@"<data name=""" + name + "\"");
            return regex.Match(line);

        }
        //validate if file is of type that use translations
        public static bool IsTranslateUserFiletype(string line)
        {
            //TODO: Remove debug and relese result also
            //check if its a desigenr file then we skip this  /Debug/
            Regex regex = new Regex(@"\w+.Designer.cs$|\w+.designer.cs$");
            var resultDesging = regex.Match(line);
            if (resultDesging.Success)
                return false;

            Regex regexOkFile = new Regex(@"\w+.cs$|\w+.xaml$");
            return regexOkFile.Match(line).Success;

        }
        public static Match TranslationNameExistInCode(string name, string line)
        {
            //Xaml
            //:AppResources.Email}
            //Code
            //AppResources.Enter_Email; ) ' '
            //Regex regex Regex(@"([.]\b" + name + "\\b[;])|([.]\b" + name + "\\b[.])|([.]\\b" + name + "\\b[)])|([.]\\b" + name + "\\b[\\s][)])|([.]\\b" + name + "\\b[\\s])|([.]\\b" + name + "\\b[}])|([.]\\b" + name + "\\b[\\s][}])|([.]\\b" + name + "\\b[,])|([.]\\b" + name + "\\b[\\s][,])");
            string format = "({0}[;])|({0}[.])|({0}[+])|({0}[)])|({0}[,])|({0}[\\s])";
            format = string.Format(format, name);
            format = format + "|(" + name + "[}])"; //format can't handle [}]
            Regex regex = new Regex(format);
            return regex.Match(line);

        }
    }
}
