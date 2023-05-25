using EnvDTE;
using EnvDTE80;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Interfaces;

namespace MultilingualExtension.Services
{
    public class StatusPadLoger : IStatusPadLoger
    {
        OutputWindowPane _pane;
        public StatusPadLoger(DTE2 dte)
        {
            
             _pane = OutputWindowHelper.GetOutputWindow(dte);
        }

        public void WriteText(string logtext)
        {
            OutputWindowHelper.WriteToOutputWindow(_pane,logtext);
        }
    }
}
