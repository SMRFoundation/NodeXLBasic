﻿

namespace Smrf.NodeXL.GraphDataProviders.YouTube
{
    partial class YouTubeGetUserNetworkDialog
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
            this.usrLimitToN = new Smrf.NodeXL.GraphDataProviders.LimitToNControl();
            this.chkIncludeAllStatistics = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radIncludeFollowedAndFollower = new System.Windows.Forms.RadioButton();
            this.radIncludeSubscriptionVertices = new System.Windows.Forms.RadioButton();
            this.radIncludeFriendVertices = new System.Windows.Forms.RadioButton();
            this.txbUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.usrNetworkLevel = new Smrf.NodeXL.GraphDataProviders.NetworkLevelControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlUserInputs.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(151, 244);
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
            this.btnCancel.Location = new System.Drawing.Point(237, 244);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.usrLimitToN);
            this.pnlUserInputs.Controls.Add(this.chkIncludeAllStatistics);
            this.pnlUserInputs.Controls.Add(this.groupBox1);
            this.pnlUserInputs.Controls.Add(this.txbUserName);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Controls.Add(this.usrNetworkLevel);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 12);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(314, 229);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // usrLimitToN
            // 
            this.usrLimitToN.Location = new System.Drawing.Point(129, 192);
            this.usrLimitToN.N = 2147483647;
            this.usrLimitToN.Name = "usrLimitToN";
            this.usrLimitToN.ObjectName = "people";
            this.usrLimitToN.Size = new System.Drawing.Size(168, 27);
            this.usrLimitToN.TabIndex = 5;
            // 
            // chkIncludeAllStatistics
            // 
            this.chkIncludeAllStatistics.Location = new System.Drawing.Point(129, 141);
            this.chkIncludeAllStatistics.Name = "chkIncludeAllStatistics";
            this.chkIncludeAllStatistics.Size = new System.Drawing.Size(176, 43);
            this.chkIncludeAllStatistics.TabIndex = 4;
            this.chkIncludeAllStatistics.Text = "&Add statistic columns and image files to the Vertices worksheet (slower)";
            this.chkIncludeAllStatistics.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radIncludeFollowedAndFollower);
            this.groupBox1.Controls.Add(this.radIncludeSubscriptionVertices);
            this.groupBox1.Controls.Add(this.radIncludeFriendVertices);
            this.groupBox1.Location = new System.Drawing.Point(0, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 90);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add a vertex for each";
            // 
            // radIncludeFollowedAndFollower
            // 
            this.radIncludeFollowedAndFollower.AutoSize = true;
            this.radIncludeFollowedAndFollower.Location = new System.Drawing.Point(15, 60);
            this.radIncludeFollowedAndFollower.Name = "radIncludeFollowedAndFollower";
            this.radIncludeFollowedAndFollower.Size = new System.Drawing.Size(47, 17);
            this.radIncludeFollowedAndFollower.TabIndex = 2;
            this.radIncludeFollowedAndFollower.TabStop = true;
            this.radIncludeFollowedAndFollower.Text = "&Both";
            this.radIncludeFollowedAndFollower.UseVisualStyleBackColor = true;
            // 
            // radIncludeSubscriptionVertices
            // 
            this.radIncludeSubscriptionVertices.AutoSize = true;
            this.radIncludeSubscriptionVertices.Location = new System.Drawing.Point(15, 40);
            this.radIncludeSubscriptionVertices.Name = "radIncludeSubscriptionVertices";
            this.radIncludeSubscriptionVertices.Size = new System.Drawing.Size(179, 17);
            this.radIncludeSubscriptionVertices.TabIndex = 1;
            this.radIncludeSubscriptionVertices.Text = "Person &subscribed to by the user";
            this.radIncludeSubscriptionVertices.UseVisualStyleBackColor = true;
            // 
            // radIncludeFriendVertices
            // 
            this.radIncludeFriendVertices.AutoSize = true;
            this.radIncludeFriendVertices.Checked = true;
            this.radIncludeFriendVertices.Location = new System.Drawing.Point(15, 20);
            this.radIncludeFriendVertices.Name = "radIncludeFriendVertices";
            this.radIncludeFriendVertices.Size = new System.Drawing.Size(107, 17);
            this.radIncludeFriendVertices.TabIndex = 0;
            this.radIncludeFriendVertices.TabStop = true;
            this.radIncludeFriendVertices.Text = "&Friend of the user";
            this.radIncludeFriendVertices.UseVisualStyleBackColor = true;
            // 
            // txbUserName
            // 
            this.txbUserName.Location = new System.Drawing.Point(0, 17);
            this.txbUserName.MaxLength = 20;
            this.txbUserName.Name = "txbUserName";
            this.txbUserName.Size = new System.Drawing.Size(305, 20);
            this.txbUserName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Get the YouTube network of the user with this username:";
            // 
            // usrNetworkLevel
            // 
            this.usrNetworkLevel.Level = Smrf.SocialNetworkLib.NetworkLevel.One;
            this.usrNetworkLevel.Location = new System.Drawing.Point(0, 142);
            this.usrNetworkLevel.Name = "usrNetworkLevel";
            this.usrNetworkLevel.Size = new System.Drawing.Size(119, 79);
            this.usrNetworkLevel.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 278);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(329, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(314, 17);
            this.slStatusLabel.Spring = true;
            this.slStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // YouTubeGetUserNetworkDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(329, 300);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YouTubeGetUserNetworkDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlUserInputs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radIncludeFollowedAndFollower;
        private System.Windows.Forms.RadioButton radIncludeSubscriptionVertices;
        private System.Windows.Forms.RadioButton radIncludeFriendVertices;
        private System.Windows.Forms.TextBox txbUserName;
        private System.Windows.Forms.Label label1;
        private NetworkLevelControl usrNetworkLevel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.CheckBox chkIncludeAllStatistics;
        private LimitToNControl usrLimitToN;
    }
}
