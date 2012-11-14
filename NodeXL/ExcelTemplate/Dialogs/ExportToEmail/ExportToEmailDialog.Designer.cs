

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class ExportToEmailDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.usrExportedFiles = new Smrf.NodeXL.ExcelTemplate.ExportedFilesControl();
            this.lblDialogDescription = new System.Windows.Forms.Label();
            this.usrExportedFilesDescription = new Smrf.NodeXL.ExcelTemplate.ExportedFilesDescriptionControl();
            this.label2 = new System.Windows.Forms.Label();
            this.txbToAddresses = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txbFromAddress = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.txbSmtpHost = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txbSmtpPort = new System.Windows.Forms.TextBox();
            this.chkUseSslForSmtp = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txbSmtpPassword = new System.Windows.Forms.TextBox();
            this.txbSmtpUserName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(485, 523);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(399, 523);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // usrExportedFiles
            // 
            this.usrExportedFiles.ExportGraphML = false;
            this.usrExportedFiles.ExportWorkbookAndSettings = false;
            this.usrExportedFiles.Location = new System.Drawing.Point(12, 385);
            this.usrExportedFiles.Name = "usrExportedFiles";
            this.usrExportedFiles.Size = new System.Drawing.Size(553, 133);
            this.usrExportedFiles.TabIndex = 7;
            this.usrExportedFiles.UseFixedAspectRatio = false;
            // 
            // lblDialogDescription
            // 
            this.lblDialogDescription.AutoSize = true;
            this.lblDialogDescription.Location = new System.Drawing.Point(12, 9);
            this.lblDialogDescription.Name = "lblDialogDescription";
            this.lblDialogDescription.Size = new System.Drawing.Size(322, 13);
            this.lblDialogDescription.TabIndex = 0;
            this.lblDialogDescription.Text = "This exports an image of the graph to one or more email addresses.";
            // 
            // usrExportedFilesDescription
            // 
            this.usrExportedFilesDescription.Description = "";
            this.usrExportedFilesDescription.DescriptionLabel = "&Message";
            this.usrExportedFilesDescription.Location = new System.Drawing.Point(12, 120);
            this.usrExportedFilesDescription.Name = "usrExportedFilesDescription";
            this.usrExportedFilesDescription.Size = new System.Drawing.Size(553, 100);
            this.usrExportedFilesDescription.TabIndex = 5;
            this.usrExportedFilesDescription.Title = "";
            this.usrExportedFilesDescription.TitleLabel = "S&ubject:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "&To:";
            this.toolTip1.SetToolTip(this.label2, "Separate email addresses with spaces, commas or returns.  Sample: john@yahoo.com " +
                    "mary@hotmail.com");
            // 
            // txbToAddresses
            // 
            this.txbToAddresses.AcceptsReturn = true;
            this.txbToAddresses.Location = new System.Drawing.Point(170, 38);
            this.txbToAddresses.MaxLength = 1000;
            this.txbToAddresses.Multiline = true;
            this.txbToAddresses.Name = "txbToAddresses";
            this.txbToAddresses.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbToAddresses.Size = new System.Drawing.Size(395, 46);
            this.txbToAddresses.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "&From:";
            this.toolTip1.SetToolTip(this.label3, "Sample: bill@gmail.com");
            // 
            // txbFromAddress
            // 
            this.txbFromAddress.Location = new System.Drawing.Point(170, 92);
            this.txbFromAddress.MaxLength = 200;
            this.txbFromAddress.Name = "txbFromAddress";
            this.txbFromAddress.Size = new System.Drawing.Size(395, 20);
            this.txbFromAddress.TabIndex = 4;
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(14, 21);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(70, 13);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Server &name:";
            this.toolTip1.SetToolTip(this.lblHost, "Sample: smtp.gmail.com");
            // 
            // txbSmtpHost
            // 
            this.txbSmtpHost.Location = new System.Drawing.Point(94, 21);
            this.txbSmtpHost.MaxLength = 200;
            this.txbSmtpHost.Name = "txbSmtpHost";
            this.txbSmtpHost.Size = new System.Drawing.Size(209, 20);
            this.txbSmtpHost.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "&Port:";
            this.toolTip1.SetToolTip(this.label4, "Sample: 587");
            // 
            // txbSmtpPort
            // 
            this.txbSmtpPort.Location = new System.Drawing.Point(94, 49);
            this.txbSmtpPort.MaxLength = 5;
            this.txbSmtpPort.Name = "txbSmtpPort";
            this.txbSmtpPort.Size = new System.Drawing.Size(87, 20);
            this.txbSmtpPort.TabIndex = 3;
            // 
            // chkUseSslForSmtp
            // 
            this.chkUseSslForSmtp.AutoSize = true;
            this.chkUseSslForSmtp.Location = new System.Drawing.Point(200, 51);
            this.chkUseSslForSmtp.Name = "chkUseSslForSmtp";
            this.chkUseSslForSmtp.Size = new System.Drawing.Size(68, 17);
            this.chkUseSslForSmtp.TabIndex = 4;
            this.chkUseSslForSmtp.Text = "Use SS&L";
            this.toolTip1.SetToolTip(this.chkUseSslForSmtp, "For Gmail, for example, this should be checked");
            this.chkUseSslForSmtp.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Us&er name:";
            this.toolTip1.SetToolTip(this.label5, "Sample: bill@gmail.com");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txbSmtpPassword);
            this.groupBox1.Controls.Add(this.txbSmtpUserName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txbSmtpHost);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblHost);
            this.groupBox1.Controls.Add(this.chkUseSslForSmtp);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txbSmtpPort);
            this.groupBox1.Location = new System.Drawing.Point(12, 232);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(553, 138);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SMTP server";
            // 
            // txbSmtpPassword
            // 
            this.txbSmtpPassword.Location = new System.Drawing.Point(94, 106);
            this.txbSmtpPassword.MaxLength = 200;
            this.txbSmtpPassword.Name = "txbSmtpPassword";
            this.txbSmtpPassword.Size = new System.Drawing.Size(209, 20);
            this.txbSmtpPassword.TabIndex = 8;
            this.txbSmtpPassword.UseSystemPasswordChar = true;
            // 
            // txbSmtpUserName
            // 
            this.txbSmtpUserName.Location = new System.Drawing.Point(94, 77);
            this.txbSmtpUserName.MaxLength = 200;
            this.txbSmtpUserName.Name = "txbSmtpUserName";
            this.txbSmtpUserName.Size = new System.Drawing.Size(209, 20);
            this.txbSmtpUserName.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "P&assword:";
            // 
            // ExportToEmailDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(580, 561);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txbFromAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txbToAddresses);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.usrExportedFilesDescription);
            this.Controls.Add(this.lblDialogDescription);
            this.Controls.Add(this.usrExportedFiles);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportToEmailDialog";
            this.Text = "Export to Email";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private ExportedFilesControl usrExportedFiles;
        private System.Windows.Forms.Label lblDialogDescription;
        private ExportedFilesDescriptionControl usrExportedFilesDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbToAddresses;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbFromAddress;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txbSmtpHost;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbSmtpPort;
        private System.Windows.Forms.CheckBox chkUseSslForSmtp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txbSmtpPassword;
        private System.Windows.Forms.TextBox txbSmtpUserName;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
