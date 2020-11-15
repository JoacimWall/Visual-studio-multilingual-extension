using System;
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
                string line = textView.Caret.Position.BufferPosition.GetContainingLine().Extent.ToString();
                var result = RexExHelper.LineContainsDataName(line);

                if (result.Success)
                {
                    int pos = line.IndexOf("\"", result.Index + 13);
                    return line.Substring(result.Index + 12, pos - result.Index - 12);

                }

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return string.Empty;
        }
    }
}
