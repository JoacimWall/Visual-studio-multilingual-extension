using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using MonoDevelop.Ide;

namespace MultilingualExtension.Shared.Helpers
{
    public static class TextViewHelpers
    {
        public static string TextViewGetSelectedNodeName()
        {
            try
            {
                var textView = IdeApp.Workbench.ActiveDocument.GetContent<ITextView>();
                ITextSnapshotLine line = textView.Caret.Position.BufferPosition.GetContainingLine();
                string lineText = line.GetText();

                var result = RegExHelper.LineContainsDataName(lineText);

                if (result.Success)
                {
                    int pos = lineText.IndexOf("\"", result.Index + 13);
                    return lineText.Substring(result.Index + 12, pos - result.Index - 12);

                }

            }
            catch 
            {

            }
            finally
            {

            }
            return string.Empty;
        }
        
    }
}

