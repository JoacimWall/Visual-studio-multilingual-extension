using MonoDevelop.Components.Commands;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using MonoDevelop.Ide;
using MultilingualExtension.Shared.Helpers;
using MonoDevelop.Projects;
using MultilingualExtension.Shared.Services;
using MultilingualExtension.Shared.Interfaces;
using System;
using MultilingualExtension.Shared.Models;

namespace MultilingualExtension
{

    public class TranslateActionHandler : CommandHandler
    {

        private string TextViewGetSelectedNodeName()
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
        protected async override void Run(object dataItem)
        {
            IProgressBar progress = new Helpers.ProgressBarHelper(Globals.Synchronize_Rows_Info);

            try
            {
                UpdateStatusForTranslation Item = dataItem as UpdateStatusForTranslation;
                Item.NodeName = TextViewGetSelectedNodeName();
                if (!string.IsNullOrEmpty(Item.NodeName))
                {
                    SyncFileService syncFileService = new SyncFileService();
                    ISettingsService settingsService = new Services.SettingsService();
                    ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;

                    var resultUpdate = await syncFileService.UpdateStatus(selectedItem.FilePath, Item, progress, settingsService);

                }


            }
            catch (Exception ex)
            {
                MonoDevelop.Ide.MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {
                progress.HideAll();
                progress = null;
                Console.WriteLine("Sync status  completed");
            }
           
        }
         protected override void Update(CommandArrayInfo info)
        {
            //Check that we are standing on data row in resx file
            if (string.IsNullOrEmpty(TextViewGetSelectedNodeName()))
               return;
            

            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            string selectedFilename = selectedItem.Name;
            var commandSet = new CommandInfoSet();
            //validate file
            var checkfile = RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
            if (checkfile.Success)
            { //this is master resx
                commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEW),  new UpdateStatusForTranslation(Globals.STATUS_COMMENT_NEW));
                commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEED_REVIEW), new UpdateStatusForTranslation(Globals.STATUS_COMMENT_NEED_REVIEW));
                commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_FINAL), new UpdateStatusForTranslation(Globals.STATUS_COMMENT_FINAL));
            }
            else
            { //child  resx fiels
                commandSet.CommandInfos.Add(new CommandInfo("All to " + Globals.STATUS_COMMENT_NEW), new UpdateStatusForTranslation(Globals.STATUS_COMMENT_NEW));
                commandSet.CommandInfos.Add(new CommandInfo("All to " + Globals.STATUS_COMMENT_TRANSLATED), new UpdateStatusForTranslation(Globals.STATUS_COMMENT_TRANSLATED));
                commandSet.CommandInfos.Add(new CommandInfo("All to " + Globals.STATUS_COMMENT_FINAL), new UpdateStatusForTranslation(Globals.STATUS_COMMENT_FINAL));
            }

            info.Add(commandSet);

        }
    }
    

}
