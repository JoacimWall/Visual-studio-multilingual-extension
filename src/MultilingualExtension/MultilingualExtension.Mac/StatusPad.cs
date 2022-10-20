using System;
using AppKit;
using MonoDevelop.Components;
using MonoDevelop.Components.Declarative;
using MonoDevelop.Components.Docking;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components.LogView;

namespace MultilingualExtension
{
    public class StatusPad : PadContent
    {
        static StatusPad instance;
        static NSView logView;
        static LogViewProgressMonitor progressMonitor;
        static LogViewController logViewController;
        ToolbarButtonItem clearButton;

        public StatusPad()
        {
            instance = this;

            CreateLogView();
        }

        static void CreateLogView()
        {
            if (logViewController != null)
            {
                return;
            }

            logViewController = new LogViewController("LogMonitor");
            logView = logViewController.Control.GetNativeWidget<NSView>();

            // Need to create a progress monitor to avoid a null reference exception
            // when LogViewController.WriteText is called.
            progressMonitor = (LogViewProgressMonitor)logViewController.GetProgressMonitor();
        }

        public static StatusPad Instance
        {
            get { return instance; }
        }

        protected override void Initialize(IPadWindow window)
        {
            var toolbar = new Toolbar();

            clearButton = new ToolbarButtonItem(toolbar.Properties, nameof(clearButton));
            clearButton.Icon = Stock.Broom;
            clearButton.Clicked += ButtonClearClick;
            clearButton.Tooltip = GettextCatalog.GetString("Clear");
            toolbar.AddItem(clearButton);

            window.SetToolbar(toolbar, DockPositionType.Right);
        }

        public override Control Control
        {
            get { return logView; }
        }

        public static LogViewController LogView
        {
            get { return logViewController; }
        }

        void ButtonClearClick(object sender, EventArgs e)
        {
            Clear();
        }

        public static void Clear()
        {
            if (logViewController != null)
            {
                Runtime.RunInMainThread(() => {
                    logViewController.Clear();
                });
            }
        }

        public static void WriteText(string message)
        {
            Runtime.RunInMainThread(() => {
                CreateLogView();
                logViewController.WriteText(progressMonitor, message + Environment.NewLine);
            });
        }

        public static void WriteError(string message)
        {
            Runtime.RunInMainThread(() => {
                CreateLogView();
                logViewController.WriteError(progressMonitor, message + Environment.NewLine);
            });
        }
    }
}

