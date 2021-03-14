using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Services;
using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Shared.Models;

namespace MultilingualExtension
{

    class SyncAndTranslateSelectedNodeHandler : CommandHandler
    {
        public SyncAndTranslateSelectedNodeHandler()
        {


        }
        protected async override void Run()
        {
            IProgressBar progress = new Helpers.ProgressBarHelper(Globals.Synchronize_Rows_Info);

            try
            {
                UpdateStatusForTranslation Item = new  UpdateStatusForTranslation(Globals.STATUS_COMMENT_NEW);
                Item.NodeName = TextViewHelpers.TextViewGetSelectedNodeName();
                if (!string.IsNullOrEmpty(Item.NodeName))
                {
                    SyncFileService syncFileService = new SyncFileService();
                    TranslationService translationService = new TranslationService();
                    ISettingsService settingsService = new Services.SettingsService();
                    await IdeApp.Workbench.SaveAllAsync();
                    ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;

                    var resultUpdate = await syncFileService.UpdateNodeStatus(selectedItem.FilePath, Item, progress, settingsService);
                    if (!resultUpdate.WasSuccessful)
                    {
                        MessageService.GenericAlert(new GenericMessage { Text = resultUpdate.ErrorMessage });
                    }
                    else
                    {
                        var resultTrans = await translationService.TranslateNode(selectedItem.FilePath, Item, progress, settingsService);
                        if (!resultUpdate.WasSuccessful)
                            MessageService.GenericAlert(new GenericMessage { Text = resultUpdate.ErrorMessage });
                    }
                }

            }
            catch (Exception ex)
            {
                MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {
                progress.HideAll();
                progress = null;
                Console.WriteLine("Sync status  completed");
            }

        }
         protected override void Update(CommandInfo info)
        {

            //Check that we are standing on data row in resx file
            if (string.IsNullOrEmpty(TextViewHelpers.TextViewGetSelectedNodeName()))
            {
                info.Visible = false;
                return;
            }

            ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
            string selectedFilename = selectedItem.Name;
           
            //validate file
            var checkfile = RegExHelper.ValidateFilenameIsTargetType(selectedFilename);
            if (checkfile.Success)
            {
                info.Visible = false;
            }
            else
            { //child  resx fiels
                info.Visible = true;
            }
        }
    }
   

}
