//using EnvDTE;
//using MultilingualExtension.Shared.Services;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace MultilingualExtension.View
//{
//    public partial class SettingsWindow : Form
//    {
//        private SettingsService _settingsService;
//        public SettingsWindow()
//        {
//            InitializeComponent();
//            _settingsService = new SettingsService("");
//            txtMasterLanguale.Text = _settingsService.MasterLanguageCode;
//            if (_settingsService.ExportFileType == 1)
//                rbCsv.Checked = true;
//            else
//               rbExcel.Checked= true;

//            if (_settingsService.TranslationService == 1)
//                rbGoogle.Checked = true;
//            else
//                rbMicrosoft.Checked = true;
//            if (_settingsService.AddCommentNodeMasterResx)
//                chkAddCommentToMaster.Checked = true;
//            else
//                chkAddCommentToMaster.Checked = false;

//            txtMsoftEndpoint.Text = _settingsService.MsoftEndpoint;
//            txtMsoftLocation.Text = _settingsService.MsoftLocation;
//            txtMsoftKey.Text = _settingsService.MsoftKey;



//        }

//        private void btnSave_Click(object sender, EventArgs e)
//        {
//              _settingsService.MasterLanguageCode= txtMasterLanguale.Text;

//            if (rbCsv.Checked)
//                _settingsService.ExportFileType = 1;
//            else
//                _settingsService.ExportFileType = 2;

//            if (rbGoogle.Checked )
//                _settingsService.TranslationService = 1;
//            else
//                _settingsService.TranslationService = 2;

//            if (chkAddCommentToMaster.Checked)
//                _settingsService.AddCommentNodeMasterResx = true;
//            else
//                _settingsService.AddCommentNodeMasterResx = false;

//            _settingsService.MsoftEndpoint= txtMsoftEndpoint.Text;
//             _settingsService.MsoftLocation = txtMsoftLocation.Text ;
//            _settingsService.MsoftKey= txtMsoftKey.Text;
//            this.Hide();
//        }

//        private void btnClose_Click(object sender, EventArgs e)
//        {
//            this.Hide();
//        }

//        private void SettingsWindow_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}
