﻿
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class MotifUserSettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MotifUserSettingsDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkFan = new System.Windows.Forms.CheckBox();
            this.chkDParallel = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.nudDParallelMinimumAnchorVertices = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudDParallelMaximumAnchorVertices = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDParallelMinimumAnchorVertices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDParallelMaximumAnchorVertices)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(319, 250);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(404, 250);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkFan
            // 
            this.chkFan.AutoSize = true;
            this.chkFan.Location = new System.Drawing.Point(198, 37);
            this.chkFan.Name = "chkFan";
            this.chkFan.Size = new System.Drawing.Size(117, 17);
            this.chkFan.TabIndex = 0;
            this.chkFan.Text = "Group by &fan motifs";
            this.chkFan.UseVisualStyleBackColor = true;
            // 
            // chkDParallel
            // 
            this.chkDParallel.AutoSize = true;
            this.chkDParallel.Location = new System.Drawing.Point(198, 25);
            this.chkDParallel.Name = "chkDParallel";
            this.chkDParallel.Size = new System.Drawing.Size(238, 17);
            this.chkDParallel.TabIndex = 0;
            this.chkDParallel.Text = "Group by &D-parallel motifs that have between";
            this.chkDParallel.UseVisualStyleBackColor = true;
            this.chkDParallel.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(10, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(176, 75);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(10, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(176, 75);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // nudDParallelMinimumAnchorVertices
            // 
            this.nudDParallelMinimumAnchorVertices.Location = new System.Drawing.Point(3, 3);
            this.nudDParallelMinimumAnchorVertices.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudDParallelMinimumAnchorVertices.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudDParallelMinimumAnchorVertices.Name = "nudDParallelMinimumAnchorVertices";
            this.nudDParallelMinimumAnchorVertices.Size = new System.Drawing.Size(52, 20);
            this.nudDParallelMinimumAnchorVertices.TabIndex = 0;
            this.nudDParallelMinimumAnchorVertices.ThousandsSeparator = true;
            this.nudDParallelMinimumAnchorVertices.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "and";
            // 
            // nudDParallelMaximumAnchorVertices
            // 
            this.nudDParallelMaximumAnchorVertices.Location = new System.Drawing.Point(92, 3);
            this.nudDParallelMaximumAnchorVertices.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudDParallelMaximumAnchorVertices.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudDParallelMaximumAnchorVertices.Name = "nudDParallelMaximumAnchorVertices";
            this.nudDParallelMaximumAnchorVertices.Size = new System.Drawing.Size(52, 20);
            this.nudDParallelMaximumAnchorVertices.TabIndex = 2;
            this.nudDParallelMaximumAnchorVertices.ThousandsSeparator = true;
            this.nudDParallelMaximumAnchorVertices.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.nudDParallelMinimumAnchorVertices);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.nudDParallelMaximumAnchorVertices);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(215, 46);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(243, 33);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(150, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "anchor vertices";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(215, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "(Anchor vertices are shown here in black)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkDParallel);
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 129);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 111);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.chkFan);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(472, 111);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // MotifUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(500, 289);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MotifUserSettingsDialog";
            this.Text = "Group by Motif";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDParallelMinimumAnchorVertices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDParallelMaximumAnchorVertices)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkFan;
        private System.Windows.Forms.CheckBox chkDParallel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.NumericUpDown nudDParallelMinimumAnchorVertices;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudDParallelMaximumAnchorVertices;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}