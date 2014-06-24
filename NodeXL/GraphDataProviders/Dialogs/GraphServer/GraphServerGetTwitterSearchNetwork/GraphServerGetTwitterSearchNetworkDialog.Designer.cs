

namespace Smrf.NodeXL.GraphDataProviders.GraphServer
{
    partial class GraphServerGetTwitterSearchNetworkDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.flpUseMaximumStatuses = new System.Windows.Forms.FlowLayoutPanel();
            this.nudMaximumStatuses = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.flpUseMaximumStatusDateUtc = new System.Windows.Forms.FlowLayoutPanel();
            this.dtpMaximumStatusDateUtc = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.radUseMaximumStatusDateUtc = new System.Windows.Forms.RadioButton();
            this.radUseMaximumStatuses = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpMinimumStatusDateUtc = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.chkExpandStatusUrls = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txbGraphServerPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txbGraphServerUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txbSearchTerm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlUserInputs.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flpUseMaximumStatuses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumStatuses)).BeginInit();
            this.flpUseMaximumStatusDateUtc.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(173, 375);
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
            this.btnCancel.Location = new System.Drawing.Point(259, 375);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.groupBox2);
            this.pnlUserInputs.Controls.Add(this.chkExpandStatusUrls);
            this.pnlUserInputs.Controls.Add(this.groupBox1);
            this.pnlUserInputs.Controls.Add(this.txbSearchTerm);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 12);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(339, 357);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.flowLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(0, 47);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(328, 117);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tweet range";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.flpUseMaximumStatuses);
            this.panel1.Controls.Add(this.flpUseMaximumStatusDateUtc);
            this.panel1.Controls.Add(this.radUseMaximumStatusDateUtc);
            this.panel1.Controls.Add(this.radUseMaximumStatuses);
            this.panel1.Location = new System.Drawing.Point(22, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(305, 62);
            this.panel1.TabIndex = 1;
            // 
            // flpUseMaximumStatuses
            // 
            this.flpUseMaximumStatuses.Controls.Add(this.nudMaximumStatuses);
            this.flpUseMaximumStatuses.Controls.Add(this.label7);
            this.flpUseMaximumStatuses.Location = new System.Drawing.Point(110, 29);
            this.flpUseMaximumStatuses.Name = "flpUseMaximumStatuses";
            this.flpUseMaximumStatuses.Size = new System.Drawing.Size(192, 32);
            this.flpUseMaximumStatuses.TabIndex = 3;
            // 
            // nudMaximumStatuses
            // 
            this.nudMaximumStatuses.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudMaximumStatuses.Location = new System.Drawing.Point(3, 3);
            this.nudMaximumStatuses.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.nudMaximumStatuses.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaximumStatuses.Name = "nudMaximumStatuses";
            this.nudMaximumStatuses.Size = new System.Drawing.Size(74, 20);
            this.nudMaximumStatuses.TabIndex = 0;
            this.nudMaximumStatuses.ThousandsSeparator = true;
            this.nudMaximumStatuses.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(83, 6);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "tweets";
            // 
            // flpUseMaximumStatusDateUtc
            // 
            this.flpUseMaximumStatusDateUtc.Controls.Add(this.dtpMaximumStatusDateUtc);
            this.flpUseMaximumStatusDateUtc.Controls.Add(this.label6);
            this.flpUseMaximumStatusDateUtc.Location = new System.Drawing.Point(110, 0);
            this.flpUseMaximumStatusDateUtc.Name = "flpUseMaximumStatusDateUtc";
            this.flpUseMaximumStatusDateUtc.Size = new System.Drawing.Size(192, 28);
            this.flpUseMaximumStatusDateUtc.TabIndex = 1;
            // 
            // dtpMaximumStatusDateUtc
            // 
            this.dtpMaximumStatusDateUtc.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpMaximumStatusDateUtc.Location = new System.Drawing.Point(3, 3);
            this.dtpMaximumStatusDateUtc.Name = "dtpMaximumStatusDateUtc";
            this.dtpMaximumStatusDateUtc.Size = new System.Drawing.Size(108, 20);
            this.dtpMaximumStatusDateUtc.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(117, 6);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "(UTC)";
            // 
            // radUseMaximumStatusDateUtc
            // 
            this.radUseMaximumStatusDateUtc.AutoSize = true;
            this.radUseMaximumStatusDateUtc.Location = new System.Drawing.Point(3, 4);
            this.radUseMaximumStatusDateUtc.Name = "radUseMaximumStatusDateUtc";
            this.radUseMaximumStatusDateUtc.Size = new System.Drawing.Size(64, 17);
            this.radUseMaximumStatusDateUtc.TabIndex = 0;
            this.radUseMaximumStatusDateUtc.TabStop = true;
            this.radUseMaximumStatusDateUtc.Text = "&through:";
            this.radUseMaximumStatusDateUtc.UseVisualStyleBackColor = true;
            this.radUseMaximumStatusDateUtc.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radUseMaximumStatuses
            // 
            this.radUseMaximumStatuses.AutoSize = true;
            this.radUseMaximumStatuses.Location = new System.Drawing.Point(3, 34);
            this.radUseMaximumStatuses.Name = "radUseMaximumStatuses";
            this.radUseMaximumStatuses.Size = new System.Drawing.Size(104, 17);
            this.radUseMaximumStatuses.TabIndex = 2;
            this.radUseMaximumStatuses.TabStop = true;
            this.radUseMaximumStatuses.Text = "to a &maximum of:";
            this.radUseMaximumStatuses.UseVisualStyleBackColor = true;
            this.radUseMaximumStatuses.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.dtpMinimumStatusDateUtc);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 19);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(196, 28);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "&From:";
            // 
            // dtpMinimumStatusDateUtc
            // 
            this.dtpMinimumStatusDateUtc.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpMinimumStatusDateUtc.Location = new System.Drawing.Point(42, 3);
            this.dtpMinimumStatusDateUtc.Name = "dtpMinimumStatusDateUtc";
            this.dtpMinimumStatusDateUtc.Size = new System.Drawing.Size(108, 20);
            this.dtpMinimumStatusDateUtc.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(156, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "(UTC)";
            // 
            // chkExpandStatusUrls
            // 
            this.chkExpandStatusUrls.AutoSize = true;
            this.chkExpandStatusUrls.Location = new System.Drawing.Point(0, 175);
            this.chkExpandStatusUrls.Name = "chkExpandStatusUrls";
            this.chkExpandStatusUrls.Size = new System.Drawing.Size(176, 17);
            this.chkExpandStatusUrls.TabIndex = 3;
            this.chkExpandStatusUrls.Text = "&Expand URLs in tweets (slower)";
            this.chkExpandStatusUrls.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txbGraphServerPassword);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txbGraphServerUserName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(0, 206);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 121);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Your NodeXL Graph Server account";
            // 
            // txbGraphServerPassword
            // 
            this.txbGraphServerPassword.Location = new System.Drawing.Point(13, 85);
            this.txbGraphServerPassword.MaxLength = 100;
            this.txbGraphServerPassword.Name = "txbGraphServerPassword";
            this.txbGraphServerPassword.PasswordChar = '*';
            this.txbGraphServerPassword.Size = new System.Drawing.Size(205, 20);
            this.txbGraphServerPassword.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "&Password:";
            // 
            // txbGraphServerUserName
            // 
            this.txbGraphServerUserName.Location = new System.Drawing.Point(13, 39);
            this.txbGraphServerUserName.MaxLength = 100;
            this.txbGraphServerUserName.Name = "txbGraphServerUserName";
            this.txbGraphServerUserName.Size = new System.Drawing.Size(205, 20);
            this.txbGraphServerUserName.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "&User name:";
            // 
            // txbSearchTerm
            // 
            this.txbSearchTerm.Location = new System.Drawing.Point(0, 20);
            this.txbSearchTerm.MaxLength = 200;
            this.txbSearchTerm.Name = "txbSearchTerm";
            this.txbSearchTerm.Size = new System.Drawing.Size(271, 20);
            this.txbSearchTerm.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Query that was collected:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 410);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(355, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(340, 17);
            this.slStatusLabel.Spring = true;
            this.slStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GraphServerGetTwitterSearchNetworkDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(355, 432);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GraphServerGetTwitterSearchNetworkDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flpUseMaximumStatuses.ResumeLayout(false);
            this.flpUseMaximumStatuses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumStatuses)).EndInit();
            this.flpUseMaximumStatusDateUtc.ResumeLayout(false);
            this.flpUseMaximumStatusDateUtc.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.TextBox txbSearchTerm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.DateTimePicker dtpMinimumStatusDateUtc;
        private System.Windows.Forms.DateTimePicker dtpMaximumStatusDateUtc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txbGraphServerPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbGraphServerUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkExpandStatusUrls;
        private System.Windows.Forms.RadioButton radUseMaximumStatuses;
        private System.Windows.Forms.NumericUpDown nudMaximumStatuses;
        private System.Windows.Forms.RadioButton radUseMaximumStatusDateUtc;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flpUseMaximumStatuses;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FlowLayoutPanel flpUseMaximumStatusDateUtc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
    }
}
