using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Interface;
using MultilingualExtension.Shared.Service;
using Task = System.Threading.Tasks.Task;

namespace MultilingualExtensionWindows
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Command
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandIdUpdateFiles = 0x0100;
        public const int CommandIdTranslateFiles = 0x0101;
        public const int CommandIdShowSettings = 0x0102;
        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("e86c9f19-293d-49ab-8b5b-f4039e5193db");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;


        private OleMenuCommand menuItemUpdateFiles;
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private Command(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            //ShowSettings
            var menuCommandIDShowSettings = new CommandID(CommandSet, CommandIdShowSettings);
            var menuItemShowSettings = new OleMenuCommand(this.ExecuteShowSettings, menuCommandIDShowSettings);
            commandService.AddCommand(menuItemShowSettings);

            //Update Files
            var menuCommandIDUpdateFiles = new CommandID(CommandSet, CommandIdUpdateFiles);
            menuItemUpdateFiles = new OleMenuCommand(this.ExecuteUpdateFiles, menuCommandIDUpdateFiles);
            menuItemUpdateFiles.BeforeQueryStatus += MenuItemUpdateFiles_BeforeQueryStatus;
            commandService.AddCommand(menuItemUpdateFiles);

            //Translate
           var menuCommandIDTranslateFiles = new CommandID(CommandSet, CommandIdTranslateFiles);
            var menuItemTranslateFiles = new OleMenuCommand(this.ExecuteTranslateFiles, menuCommandIDTranslateFiles);
            commandService.AddCommand(menuItemTranslateFiles);
        }

        private void MenuItemUpdateFiles_BeforeQueryStatus(object sender, EventArgs e)
        {
            // get the menu that fired the event
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand != null)
            {
                // Get the file path
                var selectedFilename = Helpers.DevfileHelper.GetSelectedFile();
                if (String.IsNullOrEmpty(selectedFilename)) return;

                var checkfile = RexExHelper.ValidateFileTypeIsResx(selectedFilename);
                if (!checkfile.Success)
                {
                    menuCommand.Visible = false;
                    menuCommand.Enabled = false;
                    return;
                }
                else
                {
                    menuCommand.Visible = true;
                    menuCommand.Enabled = true;
                }

                // Get the file path
                //validate file
                checkfile = RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
                if (!checkfile.Success)
                {
                    menuCommand.Text = "Sync all .xx-xx.resx files with this";
                }
                else
                {
                    menuCommand.Text = "Sync this .xx-xx.resx file";
                }

            }
            menuCommand.Visible = true;
            menuCommand.Enabled = true;
        }
       
        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static Command Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in Command's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new Command(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void ExecuteUpdateFiles(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IProgressBar progress = new Helpers.ProgressBarHelper();
            Services.SettingsService settingsService = new Services.SettingsService();
            try
            {
                // Get the file path
                var selectedFilename = Helpers.DevfileHelper.GetSelectedFile();
                if (String.IsNullOrEmpty(selectedFilename)) return;
               
               
                //MultilingualExtension.Shared
                SyncFileService syncFileService = new SyncFileService();
                bool addCommentNodeToMasterResx = settingsService.AddCommentNodeMasterResx; // Service.SettingsService.AddCommentNodeMasterResx;

                
                //validate file
                var checkfile = RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
                if (!checkfile.Success)
                {
                    //TODO: Show message you have selected master .resx file we will update all other resx files in this folder that have the format .sv-SE.resx
                    int folderindex = selectedFilename.LastIndexOf("\\");
                    string masterFolderPath = selectedFilename.Substring(0, folderindex);

                    string[] fileEntries = Directory.GetFiles(masterFolderPath);
                    foreach (string fileName in fileEntries)
                    {
                        var checkfileInFolder = RexExHelper.ValidateFilenameIsTargetType(fileName);
                        if (checkfileInFolder.Success)
                            syncFileService.SyncFile(selectedFilename, fileName, addCommentNodeToMasterResx, progress);

                    }

                }
                else
                {
                    string masterPath = selectedFilename.Substring(0, checkfile.Index) + ".resx";
                    syncFileService.SyncFile(masterPath, selectedFilename, addCommentNodeToMasterResx, progress);
                }


            }
            catch (Exception ex)
            {
                VsShellUtilities.ShowMessageBox(
                    this.package,
                    ex.Message,
                    "Multilangiual Extension",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

            }
            finally
            {
                progress.HideAll();
                progress = null;
                Console.WriteLine("Sync file completed");
            }

            //string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            //string title = "Command";

            //// Show a message box to prove we were here
            //VsShellUtilities.ShowMessageBox(
            //    this.package,
            //    message,
            //    title,
            //    OLEMSGICON.OLEMSGICON_INFO,
            //    OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
        private void ExecuteTranslateFiles(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            string title = "Command";

            // Show a message box to prove we were here
            VsShellUtilities.ShowMessageBox(
                this.package,
                message,
                title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
        private void ExecuteShowSettings(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            View.SettingsWindow frmSettings = new View.SettingsWindow();
            frmSettings.ShowDialog();
        }
       
    }
}
