using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using MultilingualExtension;
using Task = System.Threading.Tasks.Task;

namespace MultilingualExtension
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(MultilingualExtensionPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideUIContextRule(_uiContextSupporResxFiles,
        name: "Support resx",
        expression: "Resource",
        termNames: new[] { "Resource" },
        termValues: new[] { "HierSingleSelectionName:.resx$" })]
    [ProvideUIContextRule(_uiContextSupporCsvXlsxFiles,
        name: "Support csv and xlsx",
        expression: "Csv | Xlsx",
        termNames: new[] { "Csv", "Excel" },
        termValues: new[] { "HierSingleSelectionName:.csv$", "HierSingleSelectionName:.xlsx$" })]
    public sealed class MultilingualExtensionPackage : AsyncPackage
    {
        private const string _uiContextSupporResxFiles = "24551deb-f034-43e9-a279-0e541241687e"; // Must match guid in VsCommandTable.vsct
        private const string _uiContextSupporCsvXlsxFiles = "24551deb-f034-43e9-a279-0e541241687f"; // Must match guid in VsCommandTable.vsct

        /// <summary>
        /// MultilingualExtensionPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "a976f7e6-3c4b-4234-80aa-f8f5400443c0";

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {

            // Request any services while on the background thread
            var commandService = await GetServiceAsync((typeof(IMenuCommandService))) as IMenuCommandService;

            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            ShowSettingsButton.Initialize(this, commandService);
            SyncFilesButton.Initialize(this, commandService);
            TranslateButton.Initialize(this, commandService);
            ExportButton.Initialize(this, commandService);
            ImportButton.Initialize(this, commandService);
        }

        #endregion
    }
}
