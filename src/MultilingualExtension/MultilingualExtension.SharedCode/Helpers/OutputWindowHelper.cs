using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultilingualExtension.Shared.Helpers
{
    public static class OutputWindowHelper
    {
        static string PaiGuid = "a976f7e6-3c4b-4234-80aa-f8f5400443c0";

        public static void WriteToOutputWindow(EnvDTE.OutputWindowPane outputPane, string text)
        {
            if (outputPane == null)
                return;

            outputPane.Activate();

            outputPane.OutputString(text + Environment.NewLine);

        }
        public static EnvDTE.OutputWindowPane GetOutputWindow(DTE2 dte)
        {
           


            EnvDTE.OutputWindowPanes panes =
                dte.ToolWindows.OutputWindow.OutputWindowPanes;
            foreach (EnvDTE.OutputWindowPane pane in panes)
            {
                if (pane.Name.Contains("Multilingual extension"))
                {

                    return pane;
                }
            }
            return null;

        }
        //Dummy for mac 
        public static EnvDTE.OutputWindowPane GetOutputWindow()
        {



            //EnvDTE.OutputWindowPanes panes =
            //    dte.ToolWindows.OutputWindow.OutputWindowPanes;
            //foreach (EnvDTE.OutputWindowPane pane in panes)
            //{
            //    if (pane.Name.Contains("Multilingual extension"))
            //    {

            //        return pane;
            //    }
            //}
            return null;

        }
    }
}
