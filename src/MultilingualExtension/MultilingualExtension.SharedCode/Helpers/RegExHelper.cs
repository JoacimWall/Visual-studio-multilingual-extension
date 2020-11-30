using System;
using System.Text.RegularExpressions;

namespace MultilingualExtension.Shared.Helpers
{
    public static class RegExHelper
    {
        //.resx
        public static Match ValidateFileTypeIsResx(string path)
        {
            Regex regex = new Regex(@".resx$");
            return regex.Match(path);

        }//AppResources.se-SE.resx
        public static Match ValidateFilenameIsTargetType(string path)
        {
            Regex regex = new Regex(@".[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx$");
            return regex.Match(path);

        }//AppResources.se-SE.resx.csv
        public static Match ValidateFilenameIsTargetTypeCsv(string path)
        {
            Regex regex = new Regex(@"\w+.[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx.csv$");
            return regex.Match(path);

        }//AppResources.se-SE.resx.slsx
        public static Match ValidateFilenameIsTargetTypeXlsx(string path)
        {
            Regex regex = new Regex(@"\w+.[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx.xlsx$");
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
            //Regex regex = new Regex(@"([.]\b" + name + "\\b[;])|([.]\b" + name + "\\b[.])|([.]\\b" + name + "\\b[)])|([.]\\b" + name + "\\b[\\s][)])|([.]\\b" + name + "\\b[\\s])|([.]\\b" + name + "\\b[}])|([.]\\b" + name + "\\b[\\s][}])|([.]\\b" + name + "\\b[,])|([.]\\b" + name + "\\b[\\s][,])");

            Regex regex = new Regex(@"(\b"+ name + "\\b[;])|(" + name + "[.])|(" + name + "[)])|(" + name + "b[\\s][)])|(" + name + "[\\s])|(" + name + "[}])|(" + name + "[\\s][}])|(" + name + "[,])|(" + name + "[\\s][,])");
            return regex.Match(line);

        }
    }
}
