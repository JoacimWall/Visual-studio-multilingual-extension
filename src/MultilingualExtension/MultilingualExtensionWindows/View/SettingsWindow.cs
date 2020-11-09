using EnvDTE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultilingualExtensionWindows.View
{
    public partial class SettingsWindow : Form
    {
        private Services.SettingsService _settingsService;
        public SettingsWindow()
        {
            InitializeComponent();
            _settingsService = new Services.SettingsService();
            txtMasterLanguale.Text = _settingsService.MasterLanguageCode;
            if (_settingsService.TranslationService == 1)
                rbGoogle.Checked = true;
            else
                rbMicrosoft.Checked = true;

            txtMsoftEndpoint.Text = _settingsService.MsoftEndpoint;
            txtMsoftLocation.Text = _settingsService.MsoftLocation;
            txtMsoftKey.Text = _settingsService.MsoftKey;



        }

        private void btnSave_Click(object sender, EventArgs e)
        {
              _settingsService.MasterLanguageCode= txtMasterLanguale.Text;
            if (rbGoogle.Checked )
                _settingsService.TranslationService = 1;
            else
                _settingsService.TranslationService = 2;

            _settingsService.MsoftEndpoint= txtMsoftEndpoint.Text;
             _settingsService.MsoftLocation = txtMsoftLocation.Text ;
            _settingsService.MsoftKey= txtMsoftKey.Text;
            this.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
