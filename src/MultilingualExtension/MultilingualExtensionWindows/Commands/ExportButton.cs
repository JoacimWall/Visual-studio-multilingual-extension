using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MultilingualExtension.Helpers;
using MultilingualExtension.Services;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Shared.Services;

namespace MultilingualExtension
{
    internal sealed class ExportButton
    {
        // CommandId must match the MyButtonId specified in the .vsct file
        public const int CommandId = 0x0103;

        // Guid must match the guidMyButtonPackageCmdSet specified in the .vsct file
        public static readonly Guid CommandSet = new Guid("e86c9f19-293d-49ab-8b5b-f4039e5193db");

        private readonly AsyncPackage _package;
        private ExportButton(AsyncPackage package, IMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var cmdID = new CommandID(CommandSet, CommandId);
            var command = new OleMenuCommand(Execute, cmdID)
            {
                // This defers the visibility logic back to the VisibilityConstraints in the .vsct file
                Supported = false
            };

            // The MyQueryStatus method makes the exact same check as the ProvideUIContextRule attribute
            // does on the MyPackage class. When that is the case, there is no need to specify
            // a QueryStatus method and we can set command.Supported=false to defer the logic back 
            // to the VisibilityConstraint in the .vsct file.
            //command.BeforeQueryStatus += MyQueryStatus;

            commandService.AddCommand(command);



        }
        public static ExportButton Instance
        {
            get;
            private set;
        }

        private IServiceProvider ServiceProvider
        {
            get { return _package; }
        }

        public static void Initialize(AsyncPackage package, IMenuCommandService commandService)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Instance = new ExportButton(package, commandService);
        }
      
        private void MyQueryStatus(object sender, EventArgs e)
        {
            var button = (OleMenuCommand)sender;

            // Make the button invisible by default
            button.Visible = false;

            var dte = ServiceProvider.GetService(typeof(DTE)) as DTE2;
            ProjectItem item = dte.SelectedItems.Item(1)?.ProjectItem;

            if (item != null)
            {
                string fileExtension = Path.GetExtension(item.Name).ToLowerInvariant();
                string[] supportedFiles = new[] { ".resx" };

                // Show the button only if a supported file is selected
                button.Visible = supportedFiles.Contains(fileExtension);
                //button.Text = "Hej";
            }
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();


            try
            {
                // Get the file path
                var selectedFilename = Helpers.DevfileHelper.GetSelectedFile();
                if (String.IsNullOrEmpty(selectedFilename)) return;


                //MultilingualExtension.Shared
                ExportService exportFileService = new ExportService();
                var dte = ServiceProvider.GetService(typeof(DTE)) as DTE2;
                var projPath = System.IO.Path.GetDirectoryName(dte.Solution.FullName);

                ISettingsService settingsService = new SettingsService(projPath);
                var outputPane = OutputWindowHelper.GetOutputWindow(dte);

                exportFileService.ExportToFile(selectedFilename, Shared.Helpers.Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW, outputPane, settingsService).GetAwaiter().GetResult();




            }
            catch (Exception ex)
            {
                VsShellUtilities.ShowMessageBox(
                    _package,
                    ex.Message,
                    "Multilangiual Extension",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

            }
            finally
            {
               
                Console.WriteLine("Export file completed");
            }
        }
    }
}
