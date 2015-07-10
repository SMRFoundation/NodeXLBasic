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
            this.txbHashtag.Location = new System.Drawing.Point(91, 12);
            this.txbHashtag.MaxLength = 20;
            this.txbHashtag.Name = "txbHashtag";
            this.txbHashtag.Size = new System.Drawing.Size(156, 20);
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
            this.txbURL.Location = new System.Drawing.Point(91, 64);
            this.txbURL.MaxLength = 20;
            this.txbURL.Name = "txbURL";
            this.txbURL.Size = new System.Drawing.Size(156, 20);
            this.txbURL.TabIndex = 3;
            // 
            // lblHashtagMaxChars
            // 
            this.lblHashtagMaxChars.AutoSize = true;
            this.lblHashtagMaxChars.Location = new System.Drawing.Point(88, 35);
            this.lblHashtagMaxChars.Name = "lblHashtagMaxChars";
            this.lblHashtagMaxChars.Size = new System.Drawing.Size(125, 13);
            this.lblHashtagMaxChars.TabIndex = 4;
            this.lblHashtagMaxChars.Text = "(Maximum 20 characters)";
            // 
            // lblURLMaxChars
            // 
            this.lblURLMaxChars.AutoSize = true;
            this.lblURLMaxChars.Location = new System.Drawing.Point(88, 87);
            this.lblURLMaxChars.Name = "lblURLMaxChars";
            this.lblURLMaxChars.Size = new System.Drawing.Size(125, 13);
            this.lblURLMaxChars.TabIndex = 5;
            this.lblURLMaxChars.Text = "(Maximum 20 characters)";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(172, 133);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(91, 133);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ExportDataUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(259, 173);
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
    }
}