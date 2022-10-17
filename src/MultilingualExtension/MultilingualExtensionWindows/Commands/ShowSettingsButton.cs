//using System;
//using System.ComponentModel.Design;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using EnvDTE;
//using EnvDTE80;
//using Microsoft.VisualStudio.Shell;
//using Microsoft.VisualStudio.Shell.Interop;


//namespace MultilingualExtension
//{
//    internal sealed class ShowSettingsButton
//    {
//        // CommandId must match the MyButtonId specified in the .vsct file
//        public const int CommandId = 0x0100;

//        // Guid must match the guidMyButtonPackageCmdSet specified in the .vsct file
//        public static readonly Guid CommandSet = new Guid("e86c9f19-293d-49ab-8b5b-f4039e5193db");

//        private readonly AsyncPackage _package;
//        private ShowSettingsButton(AsyncPackage package, IMenuCommandService commandService)
//        {
//            _package = package ?? throw new ArgumentNullException(nameof(package));
//            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

//            var cmdID = new CommandID(CommandSet, CommandId);
//            var command = new OleMenuCommand(Execute, cmdID)
//            {
//                // This defers the visibility logic back to the VisibilityConstraints in the .vsct file
//                Supported = false
//            };

//            // The MyQueryStatus method makes the exact same check as the ProvideUIContextRule attribute
//            // does on the MyPackage class. When that is the case, there is no need to specify
//            // a QueryStatus method and we can set command.Supported=false to defer the logic back 
//            // to the VisibilityConstraint in the .vsct file.
//            //command.BeforeQueryStatus += MyQueryStatus;

//            commandService.AddCommand(command);



//        }
//        public static ShowSettingsButton Instance
//        {
//            get;
//            private set;
//        }

//        private IServiceProvider ServiceProvider
//        {
//            get { return _package; }
//        }

//        public static void Initialize(AsyncPackage package, IMenuCommandService commandService)
//        {
//            ThreadHelper.ThrowIfNotOnUIThread();

//            Instance = new ShowSettingsButton(package, commandService);
//        }



//        private void Execute(object sender, EventArgs e)
//        {
//            //SettingsWindow frmSettings = new SettingsWindow();
//            //frmSettings.ShowDialog();
//        }
//    }
//}
