//namespace MultilingualExtension.View
//{
//    partial class SettingsWindow
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.label1 = new System.Windows.Forms.Label();
//            this.txtMasterLanguale = new System.Windows.Forms.TextBox();
//            this.chkAddCommentToMaster = new System.Windows.Forms.CheckBox();
//            this.label2 = new System.Windows.Forms.Label();
//            this.rbGoogle = new System.Windows.Forms.RadioButton();
//            this.rbMicrosoft = new System.Windows.Forms.RadioButton();
//            this.btnSave = new System.Windows.Forms.Button();
//            this.label3 = new System.Windows.Forms.Label();
//            this.label4 = new System.Windows.Forms.Label();
//            this.label5 = new System.Windows.Forms.Label();
//            this.label6 = new System.Windows.Forms.Label();
//            this.label7 = new System.Windows.Forms.Label();
//            this.label8 = new System.Windows.Forms.Label();
//            this.txtMsoftEndpoint = new System.Windows.Forms.TextBox();
//            this.txtMsoftLocation = new System.Windows.Forms.TextBox();
//            this.txtMsoftKey = new System.Windows.Forms.TextBox();
//            this.btnClose = new System.Windows.Forms.Button();
//            this.label9 = new System.Windows.Forms.Label();
//            this.panel1 = new System.Windows.Forms.Panel();
//            this.rbCsv = new System.Windows.Forms.RadioButton();
//            this.rbExcel = new System.Windows.Forms.RadioButton();
//            this.panel1.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // label1
//            // 
//            this.label1.AutoSize = true;
//            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label1.Location = new System.Drawing.Point(41, 25);
//            this.label1.Name = "label1";
//            this.label1.Size = new System.Drawing.Size(253, 26);
//            this.label1.TabIndex = 0;
//            this.label1.Text = "Master language code:";
//            // 
//            // txtMasterLanguale
//            // 
//            this.txtMasterLanguale.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.txtMasterLanguale.Location = new System.Drawing.Point(47, 58);
//            this.txtMasterLanguale.Name = "txtMasterLanguale";
//            this.txtMasterLanguale.Size = new System.Drawing.Size(1364, 38);
//            this.txtMasterLanguale.TabIndex = 1;
//            // 
//            // chkAddCommentToMaster
//            // 
//            this.chkAddCommentToMaster.AutoSize = true;
//            this.chkAddCommentToMaster.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.chkAddCommentToMaster.Location = new System.Drawing.Point(47, 207);
//            this.chkAddCommentToMaster.Name = "chkAddCommentToMaster";
//            this.chkAddCommentToMaster.Size = new System.Drawing.Size(549, 33);
//            this.chkAddCommentToMaster.TabIndex = 2;
//            this.chkAddCommentToMaster.Text = "Add Comment node to master Resx file on sync";
//            this.chkAddCommentToMaster.UseVisualStyleBackColor = true;
//            // 
//            // label2
//            // 
//            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.label2.Location = new System.Drawing.Point(45, 255);
//            this.label2.Name = "label2";
//            this.label2.Size = new System.Drawing.Size(1366, 10);
//            this.label2.TabIndex = 3;
//            // 
//            // rbGoogle
//            // 
//            this.rbGoogle.AutoSize = true;
//            this.rbGoogle.Checked = true;
//            this.rbGoogle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.rbGoogle.Location = new System.Drawing.Point(45, 281);
//            this.rbGoogle.Name = "rbGoogle";
//            this.rbGoogle.Size = new System.Drawing.Size(481, 35);
//            this.rbGoogle.TabIndex = 4;
//            this.rbGoogle.TabStop = true;
//            this.rbGoogle.Text = "Google translate free (max 100/h)";
//            this.rbGoogle.UseVisualStyleBackColor = true;
//            // 
//            // rbMicrosoft
//            // 
//            this.rbMicrosoft.AutoSize = true;
//            this.rbMicrosoft.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.rbMicrosoft.Location = new System.Drawing.Point(45, 395);
//            this.rbMicrosoft.Name = "rbMicrosoft";
//            this.rbMicrosoft.Size = new System.Drawing.Size(319, 35);
//            this.rbMicrosoft.TabIndex = 5;
//            this.rbMicrosoft.Text = "Microsoft Translation";
//            this.rbMicrosoft.UseVisualStyleBackColor = true;
//            // 
//            // btnSave
//            // 
//            this.btnSave.Location = new System.Drawing.Point(1222, 755);
//            this.btnSave.Name = "btnSave";
//            this.btnSave.Size = new System.Drawing.Size(187, 61);
//            this.btnSave.TabIndex = 6;
//            this.btnSave.Text = "Save";
//            this.btnSave.UseVisualStyleBackColor = true;
//            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
//            // 
//            // label3
//            // 
//            this.label3.AutoSize = true;
//            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label3.Location = new System.Drawing.Point(47, 323);
//            this.label3.Name = "label3";
//            this.label3.Size = new System.Drawing.Size(865, 26);
//            this.label3.TabIndex = 7;
//            this.label3.Text = "You will only be allowed to translate about 100 words per hour using the free Goo" +
//    "gle API";
//            // 
//            // label4
//            // 
//            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.label4.Location = new System.Drawing.Point(45, 366);
//            this.label4.Name = "label4";
//            this.label4.Size = new System.Drawing.Size(1366, 10);
//            this.label4.TabIndex = 8;
//            // 
//            // label5
//            // 
//            this.label5.AutoSize = true;
//            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label5.Location = new System.Drawing.Point(40, 443);
//            this.label5.Name = "label5";
//            this.label5.Size = new System.Drawing.Size(630, 26);
//            this.label5.TabIndex = 9;
//            this.label5.Text = "You will need a Azure Cognitive Service (texttranslation service)\r\n";
//            // 
//            // label6
//            // 
//            this.label6.AutoSize = true;
//            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label6.Location = new System.Drawing.Point(45, 482);
//            this.label6.Name = "label6";
//            this.label6.Size = new System.Drawing.Size(113, 26);
//            this.label6.TabIndex = 10;
//            this.label6.Text = "Endpoint:";
//            // 
//            // label7
//            // 
//            this.label7.AutoSize = true;
//            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label7.Location = new System.Drawing.Point(46, 557);
//            this.label7.Name = "label7";
//            this.label7.Size = new System.Drawing.Size(109, 26);
//            this.label7.TabIndex = 11;
//            this.label7.Text = "Location:";
//            // 
//            // label8
//            // 
//            this.label8.AutoSize = true;
//            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label8.Location = new System.Drawing.Point(47, 637);
//            this.label8.Name = "label8";
//            this.label8.Size = new System.Drawing.Size(60, 26);
//            this.label8.TabIndex = 12;
//            this.label8.Text = "Key:";
//            // 
//            // txtMsoftEndpoint
//            // 
//            this.txtMsoftEndpoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.txtMsoftEndpoint.Location = new System.Drawing.Point(47, 512);
//            this.txtMsoftEndpoint.Name = "txtMsoftEndpoint";
//            this.txtMsoftEndpoint.Size = new System.Drawing.Size(1364, 38);
//            this.txtMsoftEndpoint.TabIndex = 13;
//            // 
//            // txtMsoftLocation
//            // 
//            this.txtMsoftLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.txtMsoftLocation.Location = new System.Drawing.Point(45, 584);
//            this.txtMsoftLocation.Name = "txtMsoftLocation";
//            this.txtMsoftLocation.Size = new System.Drawing.Size(1364, 38);
//            this.txtMsoftLocation.TabIndex = 14;
//            // 
//            // txtMsoftKey
//            // 
//            this.txtMsoftKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.txtMsoftKey.Location = new System.Drawing.Point(47, 668);
//            this.txtMsoftKey.Name = "txtMsoftKey";
//            this.txtMsoftKey.PasswordChar = '*';
//            this.txtMsoftKey.Size = new System.Drawing.Size(1364, 38);
//            this.txtMsoftKey.TabIndex = 15;
//            // 
//            // btnClose
//            // 
//            this.btnClose.Location = new System.Drawing.Point(1027, 755);
//            this.btnClose.Name = "btnClose";
//            this.btnClose.Size = new System.Drawing.Size(168, 61);
//            this.btnClose.TabIndex = 16;
//            this.btnClose.Text = "Close";
//            this.btnClose.UseVisualStyleBackColor = true;
//            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
//            // 
//            // label9
//            // 
//            this.label9.AutoSize = true;
//            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label9.Location = new System.Drawing.Point(46, 109);
//            this.label9.Name = "label9";
//            this.label9.Size = new System.Drawing.Size(179, 26);
//            this.label9.TabIndex = 17;
//            this.label9.Text = "Export file type:";
//            // 
//            // panel1
//            // 
//            this.panel1.Controls.Add(this.rbExcel);
//            this.panel1.Controls.Add(this.rbCsv);
//            this.panel1.Location = new System.Drawing.Point(52, 143);
//            this.panel1.Name = "panel1";
//            this.panel1.Size = new System.Drawing.Size(342, 58);
//            this.panel1.TabIndex = 18;
//            // 
//            // rbCsv
//            // 
//            this.rbCsv.AutoSize = true;
//            this.rbCsv.Checked = true;
//            this.rbCsv.Location = new System.Drawing.Point(17, 15);
//            this.rbCsv.Name = "rbCsv";
//            this.rbCsv.Size = new System.Drawing.Size(120, 29);
//            this.rbCsv.TabIndex = 0;
//            this.rbCsv.TabStop = true;
//            this.rbCsv.Text = "CSV file";
//            this.rbCsv.UseVisualStyleBackColor = true;
//            // 
//            // rbExcel
//            // 
//            this.rbExcel.AutoSize = true;
//            this.rbExcel.Location = new System.Drawing.Point(171, 15);
//            this.rbExcel.Name = "rbExcel";
//            this.rbExcel.Size = new System.Drawing.Size(130, 29);
//            this.rbExcel.TabIndex = 1;
//            this.rbExcel.Text = "Excel file";
//            this.rbExcel.UseVisualStyleBackColor = true;
//            // 
//            // SettingsWindow
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(1430, 829);
//            this.Controls.Add(this.panel1);
//            this.Controls.Add(this.label9);
//            this.Controls.Add(this.btnClose);
//            this.Controls.Add(this.txtMsoftKey);
//            this.Controls.Add(this.txtMsoftLocation);
//            this.Controls.Add(this.txtMsoftEndpoint);
//            this.Controls.Add(this.label8);
//            this.Controls.Add(this.label7);
//            this.Controls.Add(this.label6);
//            this.Controls.Add(this.label5);
//            this.Controls.Add(this.label4);
//            this.Controls.Add(this.label3);
//            this.Controls.Add(this.btnSave);
//            this.Controls.Add(this.rbMicrosoft);
//            this.Controls.Add(this.rbGoogle);
//            this.Controls.Add(this.label2);
//            this.Controls.Add(this.chkAddCommentToMaster);
//            this.Controls.Add(this.txtMasterLanguale);
//            this.Controls.Add(this.label1);
//            this.MaximizeBox = false;
//            this.MinimizeBox = false;
//            this.Name = "SettingsWindow";
//            this.Text = "Multilingual Settings";
//            this.Load += new System.EventHandler(this.SettingsWindow_Load);
//            this.panel1.ResumeLayout(false);
//            this.panel1.PerformLayout();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private System.Windows.Forms.Label label1;
//        private System.Windows.Forms.TextBox txtMasterLanguale;
//        private System.Windows.Forms.CheckBox chkAddCommentToMaster;
//        private System.Windows.Forms.Label label2;
//        private System.Windows.Forms.RadioButton rbGoogle;
//        private System.Windows.Forms.RadioButton rbMicrosoft;
//        private System.Windows.Forms.Button btnSave;
//        private System.Windows.Forms.Label label3;
//        private System.Windows.Forms.Label label4;
//        private System.Windows.Forms.Label label5;
//        private System.Windows.Forms.Label label6;
//        private System.Windows.Forms.Label label7;
//        private System.Windows.Forms.Label label8;
//        private System.Windows.Forms.TextBox txtMsoftEndpoint;
//        private System.Windows.Forms.TextBox txtMsoftLocation;
//        private System.Windows.Forms.TextBox txtMsoftKey;
//        private System.Windows.Forms.Button btnClose;
//        private System.Windows.Forms.Label label9;
//        private System.Windows.Forms.Panel panel1;
//        private System.Windows.Forms.RadioButton rbExcel;
//        private System.Windows.Forms.RadioButton rbCsv;
//    }
//}