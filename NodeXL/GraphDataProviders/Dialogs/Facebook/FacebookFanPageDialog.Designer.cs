using System.Drawing;
namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    partial class FacebookFanPageDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FacebookFanPageDialog));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.bgLoadResults = new System.ComponentModel.BackgroundWorker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnResults = new System.Windows.Forms.Panel();
            this.piLoading = new Smrf.NodeXL.GraphDataProviders.Facebook.ProgressIndicator();
            this.flpResults = new System.Windows.Forms.FlowLayoutPanel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.nuToPost = new System.Windows.Forms.NumericUpDown();
            this.nudLimit = new System.Windows.Forms.NumericUpDown();
            this.chkLimit = new System.Windows.Forms.CheckBox();
            this.lblAnd = new System.Windows.Forms.Label();
            this.dtEndDate = new System.Windows.Forms.DateTimePicker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.chkIncludeOthers = new System.Windows.Forms.CheckBox();
            this.dtStartDate = new System.Windows.Forms.DateTimePicker();
            this.rbDateDownload = new System.Windows.Forms.RadioButton();
            this.rbDownloadFromPost = new System.Windows.Forms.RadioButton();
            this.lblToPost = new System.Windows.Forms.Label();
            this.nudFromPost = new System.Windows.Forms.NumericUpDown();
            this.chkStatusUpdates = new System.Windows.Forms.CheckBox();
            this.grpAttributes = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dgAttributes = new System.Windows.Forms.DataGridView();
            this.attributeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.includeColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.attributeValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbFanPage = new System.Windows.Forms.GroupBox();
            this.txtPageUsernameID = new System.Windows.Forms.TextBox();
            this.lblNameID = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMainText = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbNetwork = new System.Windows.Forms.GroupBox();
            this.chkNetwork = new System.Windows.Forms.CheckBox();
            this.gbEdge = new System.Windows.Forms.GroupBox();
            this.chkUserRelationshipCommentAuthor = new System.Windows.Forms.CheckBox();
            this.chkEdges = new System.Windows.Forms.CheckBox();
            this.chkConsecutiveRelationship = new System.Windows.Forms.CheckBox();
            this.chkRelationshipPostAuthor = new System.Windows.Forms.CheckBox();
            this.chkPostSameRelationship = new System.Windows.Forms.CheckBox();
            this.chkUserRelationshipSamePost = new System.Windows.Forms.CheckBox();
            this.gbRelationship = new System.Windows.Forms.GroupBox();
            this.chkRelationship = new System.Windows.Forms.CheckBox();
            this.chkShare = new System.Windows.Forms.CheckBox();
            this.chkComment = new System.Windows.Forms.CheckBox();
            this.chkLike = new System.Windows.Forms.CheckBox();
            this.gbVertices = new System.Windows.Forms.GroupBox();
            this.chkVertices = new System.Windows.Forms.CheckBox();
            this.rbPost = new System.Windows.Forms.RadioButton();
            this.rbUser = new System.Windows.Forms.RadioButton();
            this.pnResults.SuspendLayout();
            this.gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuToPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFromPost)).BeginInit();
            this.grpAttributes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAttributes)).BeginInit();
            this.gbFanPage.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.gbNetwork.SuspendLayout();
            this.gbEdge.SuspendLayout();
            this.gbRelationship.SuspendLayout();
            this.gbVertices.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Unimodal Networks";
            // 
            // toolTip2
            // 
            this.toolTip2.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip2.ToolTipTitle = "Bi-Modal Networks";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // bgLoadResults
            // 
            this.bgLoadResults.WorkerReportsProgress = true;
            this.bgLoadResults.WorkerSupportsCancellation = true;
            this.bgLoadResults.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgLoadResults_DoWork);
            this.bgLoadResults.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgLoadResults_ProgressChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(614, 542);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnResults
            // 
            this.pnResults.Controls.Add(this.piLoading);
            this.pnResults.Controls.Add(this.flpResults);
            this.pnResults.Location = new System.Drawing.Point(79, 130);
            this.pnResults.Name = "pnResults";
            this.pnResults.Size = new System.Drawing.Size(429, 293);
            this.pnResults.TabIndex = 18;
            this.pnResults.Visible = false;
            // 
            // piLoading
            // 
            this.piLoading.AnimationSpeed = 90;
            this.piLoading.BackColor = System.Drawing.Color.White;
            this.piLoading.CircleColor = System.Drawing.Color.SteelBlue;
            this.piLoading.CircleSize = 0.7F;
            this.piLoading.Location = new System.Drawing.Point(174, 103);
            this.piLoading.Name = "piLoading";
            this.piLoading.NumberOfCircles = 90;
            this.piLoading.NumberOfVisibleCircles = 90;
            this.piLoading.Percentage = 0F;
            this.piLoading.Size = new System.Drawing.Size(90, 90);
            this.piLoading.TabIndex = 3;
            this.piLoading.Text = "progressIndicator1";
            // 
            // flpResults
            // 
            this.flpResults.AutoScroll = true;
            this.flpResults.BackColor = System.Drawing.Color.White;
            this.flpResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpResults.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpResults.Location = new System.Drawing.Point(0, 0);
            this.flpResults.Name = "flpResults";
            this.flpResults.Size = new System.Drawing.Size(429, 293);
            this.flpResults.TabIndex = 2;
            this.flpResults.WrapContents = false;
            this.flpResults.Enter += new System.EventHandler(this.flpResults_Enter);
            this.flpResults.Leave += new System.EventHandler(this.flpResults_Leave);
            this.flpResults.MouseHover += new System.EventHandler(this.flpResults_MouseHover);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(15, 58);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(175, 13);
            this.linkLabel1.TabIndex = 17;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Click here to logout from Facebook.";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.nuToPost);
            this.gbOptions.Controls.Add(this.nudLimit);
            this.gbOptions.Controls.Add(this.chkLimit);
            this.gbOptions.Controls.Add(this.lblAnd);
            this.gbOptions.Controls.Add(this.dtEndDate);
            this.gbOptions.Controls.Add(this.pictureBox1);
            this.gbOptions.Controls.Add(this.chkIncludeOthers);
            this.gbOptions.Controls.Add(this.dtStartDate);
            this.gbOptions.Controls.Add(this.rbDateDownload);
            this.gbOptions.Controls.Add(this.rbDownloadFromPost);
            this.gbOptions.Controls.Add(this.lblToPost);
            this.gbOptions.Controls.Add(this.nudFromPost);
            this.gbOptions.Controls.Add(this.chkStatusUpdates);
            this.gbOptions.Location = new System.Drawing.Point(358, 400);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(366, 136);
            this.gbOptions.TabIndex = 16;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            // 
            // nuToPost
            // 
            this.nuToPost.Location = new System.Drawing.Point(241, 18);
            this.nuToPost.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nuToPost.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuToPost.Name = "nuToPost";
            this.nuToPost.Size = new System.Drawing.Size(50, 20);
            this.nuToPost.TabIndex = 25;
            this.nuToPost.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nudLimit
            // 
            this.nudLimit.Location = new System.Drawing.Point(211, 66);
            this.nudLimit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLimit.Name = "nudLimit";
            this.nudLimit.Size = new System.Drawing.Size(51, 20);
            this.nudLimit.TabIndex = 24;
            this.nudLimit.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // chkLimit
            // 
            this.chkLimit.AutoSize = true;
            this.chkLimit.Location = new System.Drawing.Point(14, 67);
            this.chkLimit.Name = "chkLimit";
            this.chkLimit.Size = new System.Drawing.Size(192, 17);
            this.chkLimit.TabIndex = 23;
            this.chkLimit.Text = "Limit nr. comments/likes per post to";
            this.chkLimit.UseVisualStyleBackColor = true;
            // 
            // lblAnd
            // 
            this.lblAnd.AutoSize = true;
            this.lblAnd.Location = new System.Drawing.Point(249, 46);
            this.lblAnd.Name = "lblAnd";
            this.lblAnd.Size = new System.Drawing.Size(25, 13);
            this.lblAnd.TabIndex = 22;
            this.lblAnd.Text = "and";
            // 
            // dtEndDate
            // 
            this.dtEndDate.CustomFormat = "M/dd/yyyy";
            this.dtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtEndDate.Location = new System.Drawing.Point(277, 44);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(83, 20);
            this.dtEndDate.TabIndex = 21;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(268, 90);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(15, 15);
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // chkIncludeOthers
            // 
            this.chkIncludeOthers.AutoSize = true;
            this.chkIncludeOthers.Location = new System.Drawing.Point(13, 90);
            this.chkIncludeOthers.Name = "chkIncludeOthers";
            this.chkIncludeOthers.Size = new System.Drawing.Size(249, 17);
            this.chkIncludeOthers.TabIndex = 19;
            this.chkIncludeOthers.Text = "Include also posts not made by the page owner";
            this.chkIncludeOthers.UseVisualStyleBackColor = true;
            // 
            // dtStartDate
            // 
            this.dtStartDate.CustomFormat = "M/dd/yyyy";
            this.dtStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtStartDate.Location = new System.Drawing.Point(161, 44);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(83, 20);
            this.dtStartDate.TabIndex = 18;
            // 
            // rbDateDownload
            // 
            this.rbDateDownload.AutoSize = true;
            this.rbDateDownload.Location = new System.Drawing.Point(14, 44);
            this.rbDateDownload.Name = "rbDateDownload";
            this.rbDateDownload.Size = new System.Drawing.Size(145, 17);
            this.rbDateDownload.TabIndex = 17;
            this.rbDateDownload.Text = "Download posts between";
            this.rbDateDownload.UseVisualStyleBackColor = true;
            // 
            // rbDownloadFromPost
            // 
            this.rbDownloadFromPost.AutoSize = true;
            this.rbDownloadFromPost.Checked = true;
            this.rbDownloadFromPost.Location = new System.Drawing.Point(14, 21);
            this.rbDownloadFromPost.Name = "rbDownloadFromPost";
            this.rbDownloadFromPost.Size = new System.Drawing.Size(119, 17);
            this.rbDownloadFromPost.TabIndex = 16;
            this.rbDownloadFromPost.TabStop = true;
            this.rbDownloadFromPost.Text = "Download from post";
            this.rbDownloadFromPost.UseVisualStyleBackColor = true;
            // 
            // lblToPost
            // 
            this.lblToPost.AutoSize = true;
            this.lblToPost.Location = new System.Drawing.Point(196, 23);
            this.lblToPost.Name = "lblToPost";
            this.lblToPost.Size = new System.Drawing.Size(39, 13);
            this.lblToPost.TabIndex = 4;
            this.lblToPost.Text = "to post";
            // 
            // nudFromPost
            // 
            this.nudFromPost.Location = new System.Drawing.Point(139, 18);
            this.nudFromPost.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudFromPost.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFromPost.Name = "nudFromPost";
            this.nudFromPost.Size = new System.Drawing.Size(50, 20);
            this.nudFromPost.TabIndex = 2;
            this.nudFromPost.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFromPost.ValueChanged += new System.EventHandler(this.nudFromPost_ValueChanged);
            // 
            // chkStatusUpdates
            // 
            this.chkStatusUpdates.AutoSize = true;
            this.chkStatusUpdates.Location = new System.Drawing.Point(13, 113);
            this.chkStatusUpdates.Name = "chkStatusUpdates";
            this.chkStatusUpdates.Size = new System.Drawing.Size(145, 17);
            this.chkStatusUpdates.TabIndex = 14;
            this.chkStatusUpdates.Text = "Get status updates (slow)";
            this.chkStatusUpdates.UseVisualStyleBackColor = true;
            // 
            // grpAttributes
            // 
            this.grpAttributes.Controls.Add(this.chkSelectAll);
            this.grpAttributes.Controls.Add(this.dgAttributes);
            this.grpAttributes.Location = new System.Drawing.Point(15, 150);
            this.grpAttributes.Name = "grpAttributes";
            this.grpAttributes.Size = new System.Drawing.Size(327, 415);
            this.grpAttributes.TabIndex = 13;
            this.grpAttributes.TabStop = false;
            this.grpAttributes.Text = "Attributes";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Location = new System.Drawing.Point(268, 19);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(14, 15);
            this.chkSelectAll.TabIndex = 1;
            this.chkSelectAll.UseVisualStyleBackColor = true;
            // 
            // dgAttributes
            // 
            this.dgAttributes.AllowUserToAddRows = false;
            this.dgAttributes.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAttributes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAttributes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.attributeColumn,
            this.includeColumn,
            this.attributeValueColumn});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgAttributes.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgAttributes.Location = new System.Drawing.Point(9, 17);
            this.dgAttributes.Name = "dgAttributes";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAttributes.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgAttributes.RowHeadersVisible = false;
            this.dgAttributes.RowTemplate.Height = 24;
            this.dgAttributes.Size = new System.Drawing.Size(308, 392);
            this.dgAttributes.TabIndex = 0;
            // 
            // attributeColumn
            // 
            this.attributeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.attributeColumn.HeaderText = "Attribute";
            this.attributeColumn.Name = "attributeColumn";
            this.attributeColumn.ReadOnly = true;
            // 
            // includeColumn
            // 
            this.includeColumn.HeaderText = "Include";
            this.includeColumn.Name = "includeColumn";
            this.includeColumn.Width = 61;
            // 
            // attributeValueColumn
            // 
            this.attributeValueColumn.HeaderText = "Attribute";
            this.attributeValueColumn.Name = "attributeValueColumn";
            this.attributeValueColumn.Visible = false;
            // 
            // gbFanPage
            // 
            this.gbFanPage.Controls.Add(this.txtPageUsernameID);
            this.gbFanPage.Controls.Add(this.lblNameID);
            this.gbFanPage.Location = new System.Drawing.Point(15, 84);
            this.gbFanPage.Name = "gbFanPage";
            this.gbFanPage.Size = new System.Drawing.Size(327, 60);
            this.gbFanPage.TabIndex = 11;
            this.gbFanPage.TabStop = false;
            this.gbFanPage.Text = "Fan Page";
            // 
            // txtPageUsernameID
            // 
            this.txtPageUsernameID.Enabled = false;
            this.txtPageUsernameID.Location = new System.Drawing.Point(64, 24);
            this.txtPageUsernameID.Name = "txtPageUsernameID";
            this.txtPageUsernameID.Size = new System.Drawing.Size(253, 20);
            this.txtPageUsernameID.TabIndex = 1;
            this.txtPageUsernameID.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageUsernameID_KeyUp);
            // 
            // lblNameID
            // 
            this.lblNameID.AutoSize = true;
            this.lblNameID.Location = new System.Drawing.Point(6, 27);
            this.lblNameID.Name = "lblNameID";
            this.lblNameID.Size = new System.Drawing.Size(54, 13);
            this.lblNameID.TabIndex = 0;
            this.lblNameID.Text = "Name/ID:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 570);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(736, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // lblMainText
            // 
            this.lblMainText.Location = new System.Drawing.Point(15, 9);
            this.lblMainText.Name = "lblMainText";
            this.lblMainText.Size = new System.Drawing.Size(645, 72);
            this.lblMainText.TabIndex = 3;
            this.lblMainText.Text = resources.GetString("lblMainText.Text");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(389, 542);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(504, 542);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Download";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbNetwork
            // 
            this.gbNetwork.Controls.Add(this.chkNetwork);
            this.gbNetwork.Controls.Add(this.gbEdge);
            this.gbNetwork.Controls.Add(this.gbRelationship);
            this.gbNetwork.Controls.Add(this.gbVertices);
            this.gbNetwork.Location = new System.Drawing.Point(358, 84);
            this.gbNetwork.Name = "gbNetwork";
            this.gbNetwork.Size = new System.Drawing.Size(366, 310);
            this.gbNetwork.TabIndex = 21;
            this.gbNetwork.TabStop = false;
            // 
            // chkNetwork
            // 
            this.chkNetwork.AutoSize = true;
            this.chkNetwork.Location = new System.Drawing.Point(6, 0);
            this.chkNetwork.Name = "chkNetwork";
            this.chkNetwork.Size = new System.Drawing.Size(66, 17);
            this.chkNetwork.TabIndex = 22;
            this.chkNetwork.Text = "Network";
            this.chkNetwork.UseVisualStyleBackColor = true;
            this.chkNetwork.CheckedChanged += new System.EventHandler(this.chkNetwork_CheckedChanged);
            // 
            // gbEdge
            // 
            this.gbEdge.Controls.Add(this.chkUserRelationshipCommentAuthor);
            this.gbEdge.Controls.Add(this.chkEdges);
            this.gbEdge.Controls.Add(this.chkConsecutiveRelationship);
            this.gbEdge.Controls.Add(this.chkRelationshipPostAuthor);
            this.gbEdge.Controls.Add(this.chkPostSameRelationship);
            this.gbEdge.Controls.Add(this.chkUserRelationshipSamePost);
            this.gbEdge.Location = new System.Drawing.Point(6, 151);
            this.gbEdge.Name = "gbEdge";
            this.gbEdge.Size = new System.Drawing.Size(348, 141);
            this.gbEdge.TabIndex = 21;
            this.gbEdge.TabStop = false;
            // 
            // chkUserRelationshipCommentAuthor
            // 
            this.chkUserRelationshipCommentAuthor.AutoSize = true;
            this.chkUserRelationshipCommentAuthor.Enabled = false;
            this.chkUserRelationshipCommentAuthor.Location = new System.Drawing.Point(31, 116);
            this.chkUserRelationshipCommentAuthor.Name = "chkUserRelationshipCommentAuthor";
            this.chkUserRelationshipCommentAuthor.Size = new System.Drawing.Size(140, 17);
            this.chkUserRelationshipCommentAuthor.TabIndex = 23;
            this.chkUserRelationshipCommentAuthor.Text = "{0} and comment author";
            this.chkUserRelationshipCommentAuthor.UseVisualStyleBackColor = true;
            // 
            // chkEdges
            // 
            this.chkEdges.AutoSize = true;
            this.chkEdges.Location = new System.Drawing.Point(13, 1);
            this.chkEdges.Name = "chkEdges";
            this.chkEdges.Size = new System.Drawing.Size(143, 17);
            this.chkEdges.TabIndex = 21;
            this.chkEdges.Text = "Create an edge between";
            this.chkEdges.UseVisualStyleBackColor = true;
            this.chkEdges.CheckStateChanged += new System.EventHandler(this.chkEdges_CheckedChanged);
            // 
            // chkConsecutiveRelationship
            // 
            this.chkConsecutiveRelationship.AutoSize = true;
            this.chkConsecutiveRelationship.Enabled = false;
            this.chkConsecutiveRelationship.Location = new System.Drawing.Point(31, 93);
            this.chkConsecutiveRelationship.Name = "chkConsecutiveRelationship";
            this.chkConsecutiveRelationship.Size = new System.Drawing.Size(164, 17);
            this.chkConsecutiveRelationship.TabIndex = 3;
            this.chkConsecutiveRelationship.Text = "two consecutive commenters";
            this.chkConsecutiveRelationship.UseVisualStyleBackColor = true;
            // 
            // chkRelationshipPostAuthor
            // 
            this.chkRelationshipPostAuthor.AutoSize = true;
            this.chkRelationshipPostAuthor.Enabled = false;
            this.chkRelationshipPostAuthor.Location = new System.Drawing.Point(31, 70);
            this.chkRelationshipPostAuthor.Name = "chkRelationshipPostAuthor";
            this.chkRelationshipPostAuthor.Size = new System.Drawing.Size(117, 17);
            this.chkRelationshipPostAuthor.TabIndex = 2;
            this.chkRelationshipPostAuthor.Text = "{0} and post author";
            this.chkRelationshipPostAuthor.UseVisualStyleBackColor = true;
            // 
            // chkPostSameRelationship
            // 
            this.chkPostSameRelationship.AutoSize = true;
            this.chkPostSameRelationship.Enabled = false;
            this.chkPostSameRelationship.Location = new System.Drawing.Point(31, 47);
            this.chkPostSameRelationship.Name = "chkPostSameRelationship";
            this.chkPostSameRelationship.Size = new System.Drawing.Size(162, 17);
            this.chkPostSameRelationship.TabIndex = 1;
            this.chkPostSameRelationship.Text = "posts that have the same {0}";
            this.chkPostSameRelationship.UseVisualStyleBackColor = true;
            // 
            // chkUserRelationshipSamePost
            // 
            this.chkUserRelationshipSamePost.AutoSize = true;
            this.chkUserRelationshipSamePost.Enabled = false;
            this.chkUserRelationshipSamePost.Location = new System.Drawing.Point(31, 24);
            this.chkUserRelationshipSamePost.Name = "chkUserRelationshipSamePost";
            this.chkUserRelationshipSamePost.Size = new System.Drawing.Size(160, 17);
            this.chkUserRelationshipSamePost.TabIndex = 0;
            this.chkUserRelationshipSamePost.Text = "users who {0} the same post";
            this.chkUserRelationshipSamePost.UseVisualStyleBackColor = true;
            // 
            // gbRelationship
            // 
            this.gbRelationship.Controls.Add(this.chkRelationship);
            this.gbRelationship.Controls.Add(this.chkShare);
            this.gbRelationship.Controls.Add(this.chkComment);
            this.gbRelationship.Controls.Add(this.chkLike);
            this.gbRelationship.Location = new System.Drawing.Point(6, 91);
            this.gbRelationship.Name = "gbRelationship";
            this.gbRelationship.Size = new System.Drawing.Size(348, 53);
            this.gbRelationship.TabIndex = 20;
            this.gbRelationship.TabStop = false;
            // 
            // chkRelationship
            // 
            this.chkRelationship.AutoSize = true;
            this.chkRelationship.Location = new System.Drawing.Point(13, 0);
            this.chkRelationship.Name = "chkRelationship";
            this.chkRelationship.Size = new System.Drawing.Size(84, 17);
            this.chkRelationship.TabIndex = 21;
            this.chkRelationship.Text = "Relationship";
            this.chkRelationship.UseVisualStyleBackColor = true;
            this.chkRelationship.CheckStateChanged += new System.EventHandler(this.chkRelationship_CheckedChanged);
            // 
            // chkShare
            // 
            this.chkShare.AutoSize = true;
            this.chkShare.Location = new System.Drawing.Point(125, 23);
            this.chkShare.Name = "chkShare";
            this.chkShare.Size = new System.Drawing.Size(54, 17);
            this.chkShare.TabIndex = 2;
            this.chkShare.Text = "Share";
            this.chkShare.UseVisualStyleBackColor = true;
            this.chkShare.Visible = false;
            this.chkShare.CheckedChanged += new System.EventHandler(this.VerticesRelationship_CheckedChanged);
            // 
            // chkComment
            // 
            this.chkComment.AutoSize = true;
            this.chkComment.Location = new System.Drawing.Point(222, 23);
            this.chkComment.Name = "chkComment";
            this.chkComment.Size = new System.Drawing.Size(70, 17);
            this.chkComment.TabIndex = 1;
            this.chkComment.Text = "Comment";
            this.chkComment.UseVisualStyleBackColor = true;
            this.chkComment.CheckedChanged += new System.EventHandler(this.VerticesRelationship_CheckedChanged);
            // 
            // chkLike
            // 
            this.chkLike.AutoSize = true;
            this.chkLike.Location = new System.Drawing.Point(31, 23);
            this.chkLike.Name = "chkLike";
            this.chkLike.Size = new System.Drawing.Size(46, 17);
            this.chkLike.TabIndex = 0;
            this.chkLike.Text = "Like";
            this.chkLike.UseVisualStyleBackColor = true;
            this.chkLike.CheckedChanged += new System.EventHandler(this.VerticesRelationship_CheckedChanged);
            // 
            // gbVertices
            // 
            this.gbVertices.Controls.Add(this.rbUser);
            this.gbVertices.Controls.Add(this.rbPost);
            this.gbVertices.Controls.Add(this.chkVertices);
            this.gbVertices.Location = new System.Drawing.Point(6, 30);
            this.gbVertices.Name = "gbVertices";
            this.gbVertices.Size = new System.Drawing.Size(348, 52);
            this.gbVertices.TabIndex = 13;
            this.gbVertices.TabStop = false;
            // 
            // chkVertices
            // 
            this.chkVertices.AutoSize = true;
            this.chkVertices.Location = new System.Drawing.Point(13, 0);
            this.chkVertices.Name = "chkVertices";
            this.chkVertices.Size = new System.Drawing.Size(64, 17);
            this.chkVertices.TabIndex = 21;
            this.chkVertices.Text = "Vertices";
            this.chkVertices.UseVisualStyleBackColor = true;
            this.chkVertices.CheckedChanged += new System.EventHandler(this.chkVertices_CheckedChanged);
            // 
            // rbPost
            // 
            this.rbPost.AutoSize = true;
            this.rbPost.Location = new System.Drawing.Point(222, 21);
            this.rbPost.Name = "rbPost";
            this.rbPost.Size = new System.Drawing.Size(46, 17);
            this.rbPost.TabIndex = 22;
            this.rbPost.Text = "Post";
            this.rbPost.UseVisualStyleBackColor = true;
            this.rbPost.CheckedChanged += new System.EventHandler(this.VerticesRelationship_CheckedChanged);
            // 
            // rbUser
            // 
            this.rbUser.AutoSize = true;
            this.rbUser.Checked = true;
            this.rbUser.Location = new System.Drawing.Point(31, 21);
            this.rbUser.Name = "rbUser";
            this.rbUser.Size = new System.Drawing.Size(47, 17);
            this.rbUser.TabIndex = 23;
            this.rbUser.TabStop = true;
            this.rbUser.Text = "User";
            this.rbUser.UseVisualStyleBackColor = true;
            this.rbUser.CheckedChanged += new System.EventHandler(this.VerticesRelationship_CheckedChanged);
            // 
            // FacebookFanPageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(736, 592);
            this.Controls.Add(this.pnResults);
            this.Controls.Add(this.gbNetwork);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.grpAttributes);
            this.Controls.Add(this.gbFanPage);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblMainText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FacebookFanPageDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Facebook Fan Page Network";
            this.Load += new System.EventHandler(this.FacebookFanPageDialog_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FacebookFanPageDialog_MouseClick);
            this.pnResults.ResumeLayout(false);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuToPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFromPost)).EndInit();
            this.grpAttributes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAttributes)).EndInit();
            this.gbFanPage.ResumeLayout(false);
            this.gbFanPage.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.gbNetwork.ResumeLayout(false);
            this.gbNetwork.PerformLayout();
            this.gbEdge.ResumeLayout(false);
            this.gbEdge.PerformLayout();
            this.gbRelationship.ResumeLayout(false);
            this.gbRelationship.PerformLayout();
            this.gbVertices.ResumeLayout(false);
            this.gbVertices.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        #endregion

        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblMainText;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.GroupBox gbFanPage;
        public System.Windows.Forms.TextBox txtPageUsernameID;
        private System.Windows.Forms.Label lblNameID;
        private System.Windows.Forms.Label lblToPost;
        public System.Windows.Forms.NumericUpDown nudFromPost;
        public System.Windows.Forms.DataGridView dgAttributes;
        private System.Windows.Forms.DataGridViewTextBoxColumn attributeColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn includeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attributeValueColumn;
        private System.Windows.Forms.GroupBox grpAttributes;
        public System.Windows.Forms.CheckBox chkSelectAll;
        public System.Windows.Forms.CheckBox chkStatusUpdates;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.LinkLabel linkLabel1;
        public System.Windows.Forms.DateTimePicker dtStartDate;
        public System.Windows.Forms.RadioButton rbDateDownload;
        public System.Windows.Forms.RadioButton rbDownloadFromPost;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.CheckBox chkIncludeOthers;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label lblAnd;
        public System.Windows.Forms.DateTimePicker dtEndDate;
        private System.Windows.Forms.FlowLayoutPanel flpResults;
        private System.ComponentModel.BackgroundWorker bgLoadResults;
        private System.Windows.Forms.Panel pnResults;
        private ProgressIndicator piLoading;
        private System.Windows.Forms.CheckBox chkLimit;
        private System.Windows.Forms.NumericUpDown nudLimit;
        public System.Windows.Forms.NumericUpDown nuToPost;
        private System.Windows.Forms.GroupBox gbNetwork;
        private System.Windows.Forms.CheckBox chkNetwork;
        private System.Windows.Forms.GroupBox gbEdge;
        private System.Windows.Forms.CheckBox chkUserRelationshipCommentAuthor;
        private System.Windows.Forms.CheckBox chkEdges;
        private System.Windows.Forms.CheckBox chkConsecutiveRelationship;
        private System.Windows.Forms.CheckBox chkRelationshipPostAuthor;
        private System.Windows.Forms.CheckBox chkPostSameRelationship;
        private System.Windows.Forms.CheckBox chkUserRelationshipSamePost;
        private System.Windows.Forms.GroupBox gbRelationship;
        private System.Windows.Forms.CheckBox chkRelationship;
        private System.Windows.Forms.CheckBox chkShare;
        private System.Windows.Forms.CheckBox chkComment;
        private System.Windows.Forms.CheckBox chkLike;
        private System.Windows.Forms.GroupBox gbVertices;
        private System.Windows.Forms.CheckBox chkVertices;
        private System.Windows.Forms.RadioButton rbUser;
        private System.Windows.Forms.RadioButton rbPost;
    }
}