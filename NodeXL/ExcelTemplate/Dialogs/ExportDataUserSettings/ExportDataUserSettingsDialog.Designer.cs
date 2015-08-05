namespace Smrf.NodeXL.ExcelTemplate
{
    partial class ExportDataUserSettingsDialog
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
            this.lblHashtag = new System.Windows.Forms.Label();
            this.txbHashtag = new System.Windows.Forms.TextBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.txbURL = new System.Windows.Forms.TextBox();
            this.lblHashtagMaxChars = new System.Windows.Forms.Label();
            this.lblURLMaxChars = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblLink = new System.Windows.Forms.Label();
            this.lblAction = new System.Windows.Forms.Label();
            this.lblActionURL = new System.Windows.Forms.Label();
            this.txtBrandURL = new System.Windows.Forms.TextBox();
            this.txtActionLabel = new System.Windows.Forms.TextBox();
            this.txtActionURL = new System.Windows.Forms.TextBox();
            this.ofdBrandLogo = new System.Windows.Forms.OpenFileDialog();
            this.txtBrandLogo = new System.Windows.Forms.TextBox();
            this.btnBrandLogo = new System.Windows.Forms.Button();
            this.lblMaxFileSize = new System.Windows.Forms.Label();
            this.hllAbout = new Smrf.AppLib.HelpLinkLabel();
            this.SuspendLayout();
            // 
            // lblHashtag
            // 
            this.lblHashtag.AutoSize = true;
            this.lblHashtag.Location = new System.Drawing.Point(12, 15);
            this.lblHashtag.Name = "lblHashtag";
            this.lblHashtag.Size = new System.Drawing.Size(73, 13);
            this.lblHashtag.TabIndex = 0;
            this.lblHashtag.Text = "Your hashtag:";
            // 
            // txbHashtag
            // 
            this.txbHashtag.Location = new System.Drawing.Point(109, 12);
            this.txbHashtag.MaxLength = 20;
            this.txbHashtag.Name = "txbHashtag";
            this.txbHashtag.Size = new System.Drawing.Size(274, 20);
            this.txbHashtag.TabIndex = 1;
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(12, 67);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(57, 13);
            this.lblURL.TabIndex = 2;
            this.lblURL.Text = "Your URL:";
            // 
            // txbURL
            // 
            this.txbURL.Location = new System.Drawing.Point(109, 64);
            this.txbURL.MaxLength = 20;
            this.txbURL.Name = "txbURL";
            this.txbURL.Size = new System.Drawing.Size(274, 20);
            this.txbURL.TabIndex = 3;
            // 
            // lblHashtagMaxChars
            // 
            this.lblHashtagMaxChars.AutoSize = true;
            this.lblHashtagMaxChars.Location = new System.Drawing.Point(106, 35);
            this.lblHashtagMaxChars.Name = "lblHashtagMaxChars";
            this.lblHashtagMaxChars.Size = new System.Drawing.Size(125, 13);
            this.lblHashtagMaxChars.TabIndex = 4;
            this.lblHashtagMaxChars.Text = "(Maximum 20 characters)";
            // 
            // lblURLMaxChars
            // 
            this.lblURLMaxChars.AutoSize = true;
            this.lblURLMaxChars.Location = new System.Drawing.Point(106, 87);
            this.lblURLMaxChars.Name = "lblURLMaxChars";
            this.lblURLMaxChars.Size = new System.Drawing.Size(125, 13);
            this.lblURLMaxChars.TabIndex = 5;
            this.lblURLMaxChars.Text = "(Maximum 20 characters)";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(308, 300);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(227, 300);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Location = new System.Drawing.Point(12, 114);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(64, 13);
            this.lblLogo.TabIndex = 8;
            this.lblLogo.Text = "Author logo:";
            // 
            // lblLink
            // 
            this.lblLink.AutoSize = true;
            this.lblLink.Location = new System.Drawing.Point(12, 167);
            this.lblLink.Name = "lblLink";
            this.lblLink.Size = new System.Drawing.Size(66, 13);
            this.lblLink.TabIndex = 9;
            this.lblLink.Text = "Author URL:";
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new System.Drawing.Point(12, 205);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(65, 13);
            this.lblAction.TabIndex = 10;
            this.lblAction.Text = "Action label:";
            // 
            // lblActionURL
            // 
            this.lblActionURL.AutoSize = true;
            this.lblActionURL.Location = new System.Drawing.Point(12, 241);
            this.lblActionURL.Name = "lblActionURL";
            this.lblActionURL.Size = new System.Drawing.Size(65, 13);
            this.lblActionURL.TabIndex = 11;
            this.lblActionURL.Text = "Action URL:";
            // 
            // txtBrandURL
            // 
            this.txtBrandURL.Location = new System.Drawing.Point(109, 164);
            this.txtBrandURL.Name = "txtBrandURL";
            this.txtBrandURL.Size = new System.Drawing.Size(274, 20);
            this.txtBrandURL.TabIndex = 13;
            // 
            // txtActionLabel
            // 
            this.txtActionLabel.Location = new System.Drawing.Point(109, 202);
            this.txtActionLabel.Name = "txtActionLabel";
            this.txtActionLabel.Size = new System.Drawing.Size(274, 20);
            this.txtActionLabel.TabIndex = 14;
            // 
            // txtActionURL
            // 
            this.txtActionURL.Location = new System.Drawing.Point(109, 238);
            this.txtActionURL.Name = "txtActionURL";
            this.txtActionURL.Size = new System.Drawing.Size(274, 20);
            this.txtActionURL.TabIndex = 15;
            // 
            // ofdBrandLogo
            // 
            this.ofdBrandLogo.Filter = "JPEG (*.jpeg)|*.jpeg|PNG (*.png)|*.png|JPG (*.jpg)|*.jpg|GIF (*.gif)|*.gif|BMP (*" +
    ".bmp)|*.bmp";
            // 
            // txtBrandLogo
            // 
            this.txtBrandLogo.Location = new System.Drawing.Point(109, 111);
            this.txtBrandLogo.Name = "txtBrandLogo";
            this.txtBrandLogo.Size = new System.Drawing.Size(193, 20);
            this.txtBrandLogo.TabIndex = 16;
            // 
            // btnBrandLogo
            // 
            this.btnBrandLogo.Location = new System.Drawing.Point(308, 109);
            this.btnBrandLogo.Name = "btnBrandLogo";
            this.btnBrandLogo.Size = new System.Drawing.Size(75, 23);
            this.btnBrandLogo.TabIndex = 17;
            this.btnBrandLogo.Text = "Browse...";
            this.btnBrandLogo.UseVisualStyleBackColor = true;
            this.btnBrandLogo.Click += new System.EventHandler(this.btnBrandLogo_Click);
            // 
            // lblMaxFileSize
            // 
            this.lblMaxFileSize.AutoSize = true;
            this.lblMaxFileSize.Location = new System.Drawing.Point(106, 134);
            this.lblMaxFileSize.Name = "lblMaxFileSize";
            this.lblMaxFileSize.Size = new System.Drawing.Size(262, 13);
            this.lblMaxFileSize.TabIndex = 18;
            this.lblMaxFileSize.Text = "(Maximum file size: 1MB Maximum resolution: 900,200)";
            // 
            // hllAbout
            // 
            this.hllAbout.AutoSize = true;
            this.hllAbout.Location = new System.Drawing.Point(12, 276);
            this.hllAbout.Name = "hllAbout";
            this.hllAbout.Size = new System.Drawing.Size(104, 13);
            this.hllAbout.TabIndex = 19;
            this.hllAbout.TabStop = true;
            this.hllAbout.Text = "About export options";
            // 
            // ExportDataUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(395, 335);
            this.Controls.Add(this.hllAbout);
            this.Controls.Add(this.lblMaxFileSize);
            this.Controls.Add(this.btnBrandLogo);
            this.Controls.Add(this.txtBrandLogo);
            this.Controls.Add(this.txtActionURL);
            this.Controls.Add(this.txtActionLabel);
            this.Controls.Add(this.txtBrandURL);
            this.Controls.Add(this.lblActionURL);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.lblLink);
            this.Controls.Add(this.lblLogo);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblURLMaxChars);
            this.Controls.Add(this.lblHashtagMaxChars);
            this.Controls.Add(this.txbURL);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.txbHashtag);
            this.Controls.Add(this.lblHashtag);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDataUserSettingsDialog";
            this.Text = "Export Data Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHashtag;
        private System.Windows.Forms.TextBox txbHashtag;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox txbURL;
        private System.Windows.Forms.Label lblHashtagMaxChars;
        private System.Windows.Forms.Label lblURLMaxChars;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblLink;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.Label lblActionURL;
        private System.Windows.Forms.TextBox txtBrandURL;
        private System.Windows.Forms.TextBox txtActionLabel;
        private System.Windows.Forms.TextBox txtActionURL;
        private System.Windows.Forms.OpenFileDialog ofdBrandLogo;
        private System.Windows.Forms.TextBox txtBrandLogo;
        private System.Windows.Forms.Button btnBrandLogo;
        private System.Windows.Forms.Label lblMaxFileSize;
        private AppLib.HelpLinkLabel hllAbout;
    }
}