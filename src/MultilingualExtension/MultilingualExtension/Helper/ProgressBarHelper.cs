
using GLib;
using Gtk;
using System;

namespace MultilingualExtension.Helper
{
    public class ProgressData
    {
        public Gtk.Window window;
        public Gtk.ProgressBar pbar;
        public uint timer;
        public bool activity_mode;
    }
    public class ProgressBarHelper
    {
        public ProgressData pdata;
        Gtk.VBox vbox;
        Gtk.HSeparator separator;
        Gtk.Table table;
        Gtk.Label label;
        public ProgressBarHelper(string InfoText)
        {

            pdata = new ProgressData();
            pdata.activity_mode = false;
            pdata.window = new Gtk.Window(Gtk.WindowType.Toplevel);
            pdata.window.Resizable = true;

            //pdata.window.DeleteEvent += destroy_progress;
            pdata.window.Title = "Multilingual Extension";
            pdata.window.BorderWidth = 2;
            pdata.window.DefaultSize = new Gdk.Size(500, 100);

            vbox = new Gtk.VBox(false, 5);
            vbox.BorderWidth = 10;
            pdata.window.Add(vbox);
            vbox.Show();


            /* Create a centering alignment object */
            //Gtk.Alignment align = new Gtk.Alignment(1, 1, 0, 0);
            // vbox.PackStart(pdata.pbar, true, true, 5);
            //align.Show();

            /* Create the GtkProgressBar */
            pdata.pbar = new Gtk.ProgressBar();
            pdata.pbar.Text = "";
            pdata.pbar.PulseStep = 0.1;
            pdata.pbar.Pulse();
            pdata.pbar.Show();
            vbox.PackStart(pdata.pbar, false, true, 5);

            separator = new Gtk.HSeparator();
            vbox.PackStart(separator, false, false, 0);
            separator.Show();

            /* rows, columns, homogeneous */
            table = new Gtk.Table(2, 3, false);
            vbox.PackStart(table, false, true, 5);
            table.Show();

            /* Add a label button to select displaying of the trough text*/
            label = new Gtk.Label(InfoText);
            table.Attach(label, 0, 1, 0, 1,
                    Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill,
                    Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill,
                    5, 5);

            label.Show();

            pdata.window.ShowAll();



        }

/* Update the value of the progress bar so that we get
* some movement */
bool progress_timeout()
{
    double new_val;

    if (pdata.activity_mode)
        pdata.pbar.Pulse();
    else
    {
        /* Calculate the value of the progress bar using the
         * value range set in the adjustment object */
        new_val = pdata.pbar.Fraction + 0.01;
        if (new_val > 1.0)
            new_val = 0.0;

        /* Set the new value */
        pdata.pbar.Fraction = new_val;
    }

    /* As this is a timeout function, return TRUE so that it
     * continues to get called */

    return true;
}
/* Callback that toggles the text display within the progress bar trough */
void toggle_show_text(object obj, EventArgs args)
{
    if (pdata.pbar.Text == "")
        pdata.pbar.Text = "some text";
    else
        pdata.pbar.Text = "";
}

/* Callback that toggles the activity mode of the progress bar */
void toggle_activity_mode(object obj, EventArgs args)
{
    pdata.activity_mode = !pdata.activity_mode;
    if (pdata.activity_mode)
        pdata.pbar.Pulse();
    else
        pdata.pbar.Fraction = 0.0;
}

/* Callback that toggles the orientation of the progress bar */
void toggle_orientation(object obj, EventArgs args)
{
    switch (pdata.pbar.Orientation)
    {
        case Gtk.ProgressBarOrientation.LeftToRight:
            pdata.pbar.Orientation = Gtk.ProgressBarOrientation.RightToLeft;
            break;
        case Gtk.ProgressBarOrientation.RightToLeft:
            pdata.pbar.Orientation = Gtk.ProgressBarOrientation.LeftToRight;
            break;
    }
}


    }
}
