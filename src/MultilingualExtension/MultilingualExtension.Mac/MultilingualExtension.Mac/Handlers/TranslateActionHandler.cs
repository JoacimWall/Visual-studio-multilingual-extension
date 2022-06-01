//using MonoDevelop.Components.Commands;
//using Microsoft.VisualStudio.Text;
//using Microsoft.VisualStudio.Text.Editor;
//using MonoDevelop.Ide;
//using MultilingualExtension.Shared.Helpers;
//using MonoDevelop.Projects;
//using MultilingualExtension.Shared.Services;
//using MultilingualExtension.Shared.Interfaces;
//using System;
//using MultilingualExtension.Shared.Models;

//namespace MultilingualExtension
//{

//    public class TranslateActionHandler : CommandHandler
//    {

        
//        protected async override void Run(object dataItem)
//        {
//            IProgressBar progress = new Helpers.ProgressBarHelper(Globals.Synchronize_Rows_Info);

//            try
//            {
//                UpdateStatusForTranslation Item = dataItem as UpdateStatusForTranslation;
//                Item.NodeName = TextViewHelpers.TextViewGetSelectedNodeName();
//                if (!string.IsNullOrEmpty(Item.NodeName))
//                {
//                    SyncFileService syncFileService = new SyncFileService();
//                    ISettingsService settingsService = new Services.SettingsService();
//                    await IdeApp.Workbench.SaveAllAsync();
//                    ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;

//                    var resultUpdate = await syncFileService.UpdateNodeStatus(selectedItem.FilePath, Item, progress, settingsService);

//                }


//            }
//            catch (Exception ex)
//            {
//                MonoDevelop.Ide.MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

//            }
//            finally
//            {
//                progress.HideAll();
//                progress = null;
//                Console.WriteLine("Sync status  completed");
//            }
           
//        }
//         protected override void Update(CommandArrayInfo info)
//        {
//            //Check that we are standing on data row in resx file
//            if (string.IsNullOrEmpty(TextViewHelpers.TextViewGetSelectedNodeName()))
//               return;
            

//            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
//            string selectedFilename = selectedItem.Name;
//            var commandSet = new CommandInfoSet();
//            //validate file
//            var checkfile = RegExHelper.ValidateFilenameIsTargetType(selectedFilename);
//            if (checkfile.Success)
//            { //this is master resx
//                commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEW),  new UpdateStatusForTranslation(Globals.STATUS_COMMENT_NEW));
//                commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_NEED_REVIEW), new UpdateStatusForTranslation(Globals.STATUS_COMMENT_NEED_REVIEW));
//                commandSet.CommandInfos.Add(new CommandInfo(Globals.STATUS_COMMENT_FINAL), new UpdateStatusForTranslation(Globals.STATUS_COMMENT_FINAL));
//            }
//            else
//            { //child  resx fiels
//                commandSet.CommandInfos.Add(new CommandInfo("All to " + Globals.STATUS_COMMENT_NEW), new UpdateStatusForTranslation(Globals.STATUS_COMMENT_NEW));
//                commandSet.CommandInfos.Add(new CommandInfo("All to " + Globals.STATUS_COMMENT_NEED_REVIEW), new UpdateStatusForTranslation(Globals.STATUS_COMMENT_NEED_REVIEW));
//                commandSet.CommandInfos.Add(new CommandInfo("All to " + Globals.STATUS_COMMENT_FINAL), new UpdateStatusForTranslation(Globals.STATUS_COMMENT_FINAL));
//            }

//            info.Add(commandSet);

//        }
//    }
    

//}
