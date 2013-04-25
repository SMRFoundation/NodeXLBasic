

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
            this.pnlSharedWordUserThreshold = new System.Windows.Forms.Panel();
            this.nudSharedWordUserThreshold = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudMaximumStatuses = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.clbWhatEdgesToInclude = new System.Windows.Forms.CheckedListBox();
            this.chkExpandStatusUrls = new System.Windows.Forms.CheckBox();
            this.usrTwitterAuthorization = new Smrf.NodeXL.GraphDataProviders.Twitter.TwitterAuthorizationControl();
            this.chkIncludeStatuses = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new Smrf.AppLib.StartProcessLinkLabel();
            this.chkIncludeStatistics = new System.Windows.Forms.CheckBox();
            this.txbSearchTerm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.usrTwitterRateLimits = new Smrf.NodeXL.GraphDataProviders.Twitter.TwitterRateLimitsControl();
            this.pnlUserInputs.SuspendLayout();
            this.pnlSharedWordUserThreshold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSharedWordUserThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumStatuses)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(196, 529);
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
            this.btnCancel.Location = new System.Drawing.Point(282, 529);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.pnlSharedWordUserThreshold);
            this.pnlUserInputs.Controls.Add(this.label4);
            this.pnlUserInputs.Controls.Add(this.label3);
            this.pnlUserInputs.Controls.Add(this.nudMaximumStatuses);
            this.pnlUserInputs.Controls.Add(this.label2);
            this.pnlUserInputs.Controls.Add(this.clbWhatEdgesToInclude);
            this.pnlUserInputs.Controls.Add(this.chkExpandStatusUrls);
            this.pnlUserInputs.Controls.Add(this.usrTwitterAuthorization);
            this.pnlUserInputs.Controls.Add(this.chkIncludeStatuses);
            this.pnlUserInputs.Controls.Add(this.linkLabel1);
            this.pnlUserInputs.Controls.Add(this.chkIncludeStatistics);
            this.pnlUserInputs.Controls.Add(this.txbSearchTerm);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 39);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(357, 480);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // pnlSharedWordUserThreshold
            // 
            this.pnlSharedWordUserThreshold.Controls.Add(this.nudSharedWordUserThreshold);
            this.pnlSharedWordUserThreshold.Controls.Add(this.label5);
            this.pnlSharedWordUserThreshold.Location = new System.Drawing.Point(156, 188);
            this.pnlSharedWordUserThreshold.Name = "pnlSharedWordUserThreshold";
            this.pnlSharedWordUserThreshold.Size = new System.Drawing.Size(188, 38);
            this.pnlSharedWordUserThreshold.TabIndex = 8;
            // 
            // nudSharedWordUserThreshold
            // 
            this.nudSharedWordUserThreshold.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSharedWordUserThreshold.Location = new System.Drawing.Point(102, 11);
            this.nudSharedWordUserThreshold.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudSharedWordUserThreshold.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudSharedWordUserThreshold.Name = "nudSharedWordUserThreshold";
            this.nudSharedWordUserThreshold.Size = new System.Drawing.Size(58, 20);
            this.nudSharedWordUserThreshold.TabIndex = 1;
            this.nudSharedWordUserThreshold.ThousandsSeparator = true;
            this.nudSharedWordUserThreshold.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 31);
            this.label5.TabIndex = 0;
            this.label5.Text = "&Shared word\r\ntweeter threshold:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Limi&t to";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 201);
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
            this.nudMaximumStatuses.Location = new System.Drawing.Point(46, 199);
            this.nudMaximumStatuses.Maximum = new decimal(new int[] {
            18000,
            0,
            0,
            0});
            this.nudMaximumStatuses.Minimum = new decimal(new int[] {
            10,
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Add a&n edge for each:";
            // 
            // clbWhatEdgesToInclude
            // 
            this.clbWhatEdgesToInclude.CheckOnClick = true;
            this.clbWhatEdgesToInclude.FormattingEnabled = true;
            this.clbWhatEdgesToInclude.Location = new System.Drawing.Point(0, 88);
            this.clbWhatEdgesToInclude.Name = "clbWhatEdgesToInclude";
            this.clbWhatEdgesToInclude.Size = new System.Drawing.Size(350, 94);
            this.clbWhatEdgesToInclude.TabIndex = 4;
            // 
            // chkExpandStatusUrls
            // 
            this.chkExpandStatusUrls.AutoSize = true;
            this.chkExpandStatusUrls.Location = new System.Drawing.Point(21, 253);
            this.chkExpandStatusUrls.Name = "chkExpandStatusUrls";
            this.chkExpandStatusUrls.Size = new System.Drawing.Size(176, 17);
            this.chkExpandStatusUrls.TabIndex = 10;
            this.chkExpandStatusUrls.Text = "&Expand URLs in tweets (slower)";
            this.chkExpandStatusUrls.UseVisualStyleBackColor = true;
            // 
            // usrTwitterAuthorization
            // 
            this.usrTwitterAuthorization.Location = new System.Drawing.Point(0, 299);
            this.usrTwitterAuthorization.Name = "usrTwitterAuthorization";
            this.usrTwitterAuthorization.Size = new System.Drawing.Size(352, 180);
            this.usrTwitterAuthorization.Status = Smrf.NodeXL.GraphDataProviders.Twitter.TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized;
            this.usrTwitterAuthorization.TabIndex = 12;
            // 
            // chkIncludeStatuses
            // 
            this.chkIncludeStatuses.AutoSize = true;
            this.chkIncludeStatuses.Location = new System.Drawing.Point(0, 233);
            this.chkIncludeStatuses.Name = "chkIncludeStatuses";
            this.chkIncludeStatuses.Size = new System.Drawing.Size(239, 17);
            this.chkIncludeStatuses.TabIndex = 9;
            this.chkIncludeStatuses.Text = "&Add a Tweet column to the Edges worksheet";
            this.chkIncludeStatuses.UseVisualStyleBackColor = true;
            this.chkIncludeStatuses.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(0, 44);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(114, 13);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Tag = "http://support.twitter.com/groups/31-twitter-basics/topics/110-search/articles/71" +
                "577-how-to-use-advanced-twitter-search";
            this.linkLabel1.Text = "Advanced search help";
            // 
            // chkIncludeStatistics
            // 
            this.chkIncludeStatistics.AutoSize = true;
            this.chkIncludeStatistics.Location = new System.Drawing.Point(0, 275);
            this.chkIncludeStatistics.Name = "chkIncludeStatistics";
            this.chkIncludeStatistics.Size = new System.Drawing.Size(287, 17);
            this.chkIncludeStatistics.TabIndex = 11;
            this.chkIncludeStatistics.Text = "A&dd statistic columns to the Vertices worksheet";
            this.chkIncludeStatistics.UseVisualStyleBackColor = true;
            // 
            // txbSearchTerm
            // 
            this.txbSearchTerm.Location = new System.Drawing.Point(0, 19);
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
            this.label1.Size = new System.Drawing.Size(286, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add a verte&x for each person whose recent tweet contains:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 562);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(374, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(359, 17);
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
            this.ClientSize = new System.Drawing.Size(374, 584);
            this.Controls.Add(this.usrTwitterRateLimits);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TwitterGetSearchNetworkDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.pnlSharedWordUserThreshold.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSharedWordUserThreshold)).EndInit();
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
        private System.Windows.Forms.CheckBox chkIncludeStatistics;
        private Smrf.AppLib.StartProcessLinkLabel linkLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.CheckBox chkIncludeStatuses;
        private TwitterAuthorizationControl usrTwitterAuthorization;
        private System.Windows.Forms.CheckBox chkExpandStatusUrls;
        private System.Windows.Forms.CheckedListBox clbWhatEdgesToInclude;
        private System.Windows.Forms.Label label2;
        private TwitterRateLimitsControl usrTwitterRateLimits;
        private System.Windows.Forms.NumericUpDown nudMaximumStatuses;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlSharedWordUserThreshold;
        private System.Windows.Forms.NumericUpDown nudSharedWordUserThreshold;
        private System.Windows.Forms.Label label5;
    }
}
