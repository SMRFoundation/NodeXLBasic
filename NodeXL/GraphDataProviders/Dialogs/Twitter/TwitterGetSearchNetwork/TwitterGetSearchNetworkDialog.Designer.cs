

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
    partial class TwitterGetSearchNetworkDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlUserInputs = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.hllNetworkTypeBasic = new Smrf.AppLib.HelpLinkLabel();
            this.hllNetworkTypeBasicPlusFollows = new Smrf.AppLib.HelpLinkLabel();
            this.picNetworkPreview = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.radIncludeFriendEdges = new System.Windows.Forms.RadioButton();
            this.radNoFriendEdges = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudMaximumStatuses = new System.Windows.Forms.NumericUpDown();
            this.chkExpandStatusUrls = new System.Windows.Forms.CheckBox();
            this.usrTwitterAuthorization = new Smrf.NodeXL.GraphDataProviders.Twitter.TwitterAuthorizationControl();
            this.linkLabel1 = new Smrf.AppLib.StartProcessLinkLabel();
            this.txbSearchTerm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.usrTwitterRateLimits = new Smrf.NodeXL.GraphDataProviders.Twitter.TwitterRateLimitsControl();
            this.pnlUserInputs.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNetworkPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumStatuses)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(479, 420);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(565, 420);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.groupBox2);
            this.pnlUserInputs.Controls.Add(this.label4);
            this.pnlUserInputs.Controls.Add(this.label3);
            this.pnlUserInputs.Controls.Add(this.nudMaximumStatuses);
            this.pnlUserInputs.Controls.Add(this.chkExpandStatusUrls);
            this.pnlUserInputs.Controls.Add(this.usrTwitterAuthorization);
            this.pnlUserInputs.Controls.Add(this.linkLabel1);
            this.pnlUserInputs.Controls.Add(this.txbSearchTerm);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 39);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(641, 375);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.hllNetworkTypeBasic);
            this.groupBox2.Controls.Add(this.hllNetworkTypeBasicPlusFollows);
            this.groupBox2.Controls.Add(this.picNetworkPreview);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.radIncludeFriendEdges);
            this.groupBox2.Controls.Add(this.radNoFriendEdges);
            this.groupBox2.Location = new System.Drawing.Point(0, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(633, 170);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "What to import";
            // 
            // hllNetworkTypeBasic
            // 
            this.hllNetworkTypeBasic.AutoSize = true;
            this.hllNetworkTypeBasic.Location = new System.Drawing.Point(49, 69);
            this.hllNetworkTypeBasic.Name = "hllNetworkTypeBasic";
            this.hllNetworkTypeBasic.Size = new System.Drawing.Size(112, 13);
            this.hllNetworkTypeBasic.TabIndex = 2;
            this.hllNetworkTypeBasic.TabStop = true;
            this.hllNetworkTypeBasic.Tag = "";
            this.hllNetworkTypeBasic.Text = "More about this option";
            // 
            // hllNetworkTypeBasicPlusFollows
            // 
            this.hllNetworkTypeBasicPlusFollows.AutoSize = true;
            this.hllNetworkTypeBasicPlusFollows.Location = new System.Drawing.Point(49, 138);
            this.hllNetworkTypeBasicPlusFollows.Name = "hllNetworkTypeBasicPlusFollows";
            this.hllNetworkTypeBasicPlusFollows.Size = new System.Drawing.Size(112, 13);
            this.hllNetworkTypeBasicPlusFollows.TabIndex = 5;
            this.hllNetworkTypeBasicPlusFollows.TabStop = true;
            this.hllNetworkTypeBasicPlusFollows.Tag = "";
            this.hllNetworkTypeBasicPlusFollows.Text = "More about this option";
            // 
            // picNetworkPreview
            // 
            this.picNetworkPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picNetworkPreview.Location = new System.Drawing.Point(338, 19);
            this.picNetworkPreview.Name = "picNetworkPreview";
            this.picNetworkPreview.Size = new System.Drawing.Size(166, 119);
            this.picNetworkPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picNetworkPreview.TabIndex = 6;
            this.picNetworkPreview.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(49, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(148, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Add some of the users\' friends";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(49, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(267, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Show who was replied to or mentioned in recent tweets";
            // 
            // radIncludeFriendEdges
            // 
            this.radIncludeFriendEdges.AutoSize = true;
            this.radIncludeFriendEdges.Location = new System.Drawing.Point(15, 96);
            this.radIncludeFriendEdges.Name = "radIncludeFriendEdges";
            this.radIncludeFriendEdges.Size = new System.Drawing.Size(204, 17);
            this.radIncludeFriendEdges.TabIndex = 3;
            this.radIncludeFriendEdges.TabStop = true;
            this.radIncludeFriendEdges.Text = "Basic network plus &friends (very slow!)";
            this.radIncludeFriendEdges.UseVisualStyleBackColor = true;
            this.radIncludeFriendEdges.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresImageChange);
            // 
            // radNoFriendEdges
            // 
            this.radNoFriendEdges.AutoSize = true;
            this.radNoFriendEdges.Location = new System.Drawing.Point(15, 27);
            this.radNoFriendEdges.Name = "radNoFriendEdges";
            this.radNoFriendEdges.Size = new System.Drawing.Size(92, 17);
            this.radNoFriendEdges.TabIndex = 0;
            this.radNoFriendEdges.TabStop = true;
            this.radNoFriendEdges.Text = "&Basic network";
            this.radNoFriendEdges.UseVisualStyleBackColor = true;
            this.radNoFriendEdges.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresImageChange);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(387, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Li&mit to";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(499, 277);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "tweets";
            // 
            // nudMaximumStatuses
            // 
            this.nudMaximumStatuses.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudMaximumStatuses.Location = new System.Drawing.Point(433, 275);
            this.nudMaximumStatuses.Maximum = new decimal(new int[] {
            18000,
            0,
            0,
            0});
            this.nudMaximumStatuses.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudMaximumStatuses.Name = "nudMaximumStatuses";
            this.nudMaximumStatuses.Size = new System.Drawing.Size(60, 20);
            this.nudMaximumStatuses.TabIndex = 6;
            this.nudMaximumStatuses.ThousandsSeparator = true;
            this.nudMaximumStatuses.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // chkExpandStatusUrls
            // 
            this.chkExpandStatusUrls.AutoSize = true;
            this.chkExpandStatusUrls.Location = new System.Drawing.Point(390, 311);
            this.chkExpandStatusUrls.Name = "chkExpandStatusUrls";
            this.chkExpandStatusUrls.Size = new System.Drawing.Size(176, 17);
            this.chkExpandStatusUrls.TabIndex = 8;
            this.chkExpandStatusUrls.Text = "E&xpand URLs in tweets (slower)";
            this.chkExpandStatusUrls.UseVisualStyleBackColor = true;
            // 
            // usrTwitterAuthorization
            // 
            this.usrTwitterAuthorization.Location = new System.Drawing.Point(0, 246);
            this.usrTwitterAuthorization.Name = "usrTwitterAuthorization";
            this.usrTwitterAuthorization.Size = new System.Drawing.Size(352, 133);
            this.usrTwitterAuthorization.Status = Smrf.NodeXL.GraphDataProviders.Twitter.TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized;
            this.usrTwitterAuthorization.TabIndex = 4;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(-3, 45);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(194, 13);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Tag = "https://twitter.com/search-home";
            this.linkLabel1.Text = "How to use advanced search operators";
            // 
            // txbSearchTerm
            // 
            this.txbSearchTerm.Location = new System.Drawing.Point(0, 20);
            this.txbSearchTerm.MaxLength = 1000;
            this.txbSearchTerm.Name = "txbSearchTerm";
            this.txbSearchTerm.Size = new System.Drawing.Size(350, 20);
            this.txbSearchTerm.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Search for tweets that match this query:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 454);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(664, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(649, 17);
            this.slStatusLabel.Spring = true;
            this.slStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // usrTwitterRateLimits
            // 
            this.usrTwitterRateLimits.Location = new System.Drawing.Point(12, 12);
            this.usrTwitterRateLimits.Name = "usrTwitterRateLimits";
            this.usrTwitterRateLimits.Size = new System.Drawing.Size(352, 14);
            this.usrTwitterRateLimits.TabIndex = 3;
            // 
            // TwitterGetSearchNetworkDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(664, 476);
            this.Controls.Add(this.usrTwitterRateLimits);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TwitterGetSearchNetworkDialog";
            this.Text = "[Gets set in code]";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TwitterGetSearchNetworkDialog_KeyDown);
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNetworkPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumStatuses)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlUserInputs;
        private System.Windows.Forms.TextBox txbSearchTerm;
        private System.Windows.Forms.Label label1;
        private Smrf.AppLib.StartProcessLinkLabel linkLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private TwitterAuthorizationControl usrTwitterAuthorization;
        private System.Windows.Forms.CheckBox chkExpandStatusUrls;
        private TwitterRateLimitsControl usrTwitterRateLimits;
        private System.Windows.Forms.NumericUpDown nudMaximumStatuses;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private AppLib.HelpLinkLabel hllNetworkTypeBasic;
        private AppLib.HelpLinkLabel hllNetworkTypeBasicPlusFollows;
        private System.Windows.Forms.PictureBox picNetworkPreview;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radIncludeFriendEdges;
        private System.Windows.Forms.RadioButton radNoFriendEdges;
    }
}
