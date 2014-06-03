
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class LabelUserSettingsDialog
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
            this.btnEdgeFont = new System.Windows.Forms.Button();
            this.lblWrapNarrow = new System.Windows.Forms.Label();
            this.lblWrapWide = new System.Windows.Forms.Label();
            this.tbVertexLabelWrapMaxTextWidth = new System.Windows.Forms.TrackBar();
            this.chkVertexLabelWrapText = new System.Windows.Forms.CheckBox();
            this.usrVertexLabelMaximumLength = new Smrf.NodeXL.ExcelTemplate.MaximumLabelLengthControl();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxVertexLabelPosition = new Smrf.NodeXL.ExcelTemplate.VertexLabelPositionComboBox();
            this.usrVertexLabelFillColor = new Smrf.AppLib.ColorPicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.usrEdgeLabelTextColor = new Smrf.AppLib.ColorPicker();
            this.usrEdgeLabelMaximumLength = new Smrf.NodeXL.ExcelTemplate.MaximumLabelLengthControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnVertexFont = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnGroupFont = new System.Windows.Forms.Button();
            this.cbxGroupLabelPosition = new Smrf.NodeXL.ExcelTemplate.VertexLabelPositionComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblEdgeAlpha = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.nudGroupLabelTextAlpha = new System.Windows.Forms.NumericUpDown();
            this.usrGroupLabelTextColor = new Smrf.AppLib.ColorPicker();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbVertexLabelWrapMaxTextWidth)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGroupLabelTextAlpha)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(506, 384);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(592, 384);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnEdgeFont
            // 
            this.btnEdgeFont.Location = new System.Drawing.Point(12, 21);
            this.btnEdgeFont.Name = "btnEdgeFont";
            this.btnEdgeFont.Size = new System.Drawing.Size(85, 23);
            this.btnEdgeFont.TabIndex = 0;
            this.btnEdgeFont.Text = "&Font...";
            this.btnEdgeFont.UseVisualStyleBackColor = true;
            this.btnEdgeFont.Click += new System.EventHandler(this.btnEdgeFont_Click);
            // 
            // lblWrapNarrow
            // 
            this.lblWrapNarrow.AutoSize = true;
            this.lblWrapNarrow.Location = new System.Drawing.Point(28, 185);
            this.lblWrapNarrow.Name = "lblWrapNarrow";
            this.lblWrapNarrow.Size = new System.Drawing.Size(62, 13);
            this.lblWrapNarrow.TabIndex = 4;
            this.lblWrapNarrow.Text = "Narrow Box";
            // 
            // lblWrapWide
            // 
            this.lblWrapWide.AutoSize = true;
            this.lblWrapWide.Location = new System.Drawing.Point(126, 185);
            this.lblWrapWide.Name = "lblWrapWide";
            this.lblWrapWide.Size = new System.Drawing.Size(53, 13);
            this.lblWrapWide.TabIndex = 5;
            this.lblWrapWide.Text = "Wide Box";
            this.lblWrapWide.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbVertexLabelWrapMaxTextWidth
            // 
            this.tbVertexLabelWrapMaxTextWidth.LargeChange = 100;
            this.tbVertexLabelWrapMaxTextWidth.Location = new System.Drawing.Point(31, 137);
            this.tbVertexLabelWrapMaxTextWidth.Maximum = 550;
            this.tbVertexLabelWrapMaxTextWidth.Minimum = 50;
            this.tbVertexLabelWrapMaxTextWidth.Name = "tbVertexLabelWrapMaxTextWidth";
            this.tbVertexLabelWrapMaxTextWidth.Size = new System.Drawing.Size(148, 45);
            this.tbVertexLabelWrapMaxTextWidth.SmallChange = 50;
            this.tbVertexLabelWrapMaxTextWidth.TabIndex = 3;
            this.tbVertexLabelWrapMaxTextWidth.TickFrequency = 50;
            this.tbVertexLabelWrapMaxTextWidth.Value = 50;
            // 
            // chkVertexLabelWrapText
            // 
            this.chkVertexLabelWrapText.AutoSize = true;
            this.chkVertexLabelWrapText.Location = new System.Drawing.Point(12, 112);
            this.chkVertexLabelWrapText.Name = "chkVertexLabelWrapText";
            this.chkVertexLabelWrapText.Size = new System.Drawing.Size(72, 17);
            this.chkVertexLabelWrapText.TabIndex = 2;
            this.chkVertexLabelWrapText.Text = "&Wrap text";
            this.chkVertexLabelWrapText.UseVisualStyleBackColor = true;
            this.chkVertexLabelWrapText.CheckedChanged += new System.EventHandler(this.chkVertexLabelWrapText_CheckedChanged);
            // 
            // usrVertexLabelMaximumLength
            // 
            this.usrVertexLabelMaximumLength.Location = new System.Drawing.Point(12, 55);
            this.usrVertexLabelMaximumLength.Name = "usrVertexLabelMaximumLength";
            this.usrVertexLabelMaximumLength.Size = new System.Drawing.Size(183, 52);
            this.usrVertexLabelMaximumLength.TabIndex = 1;
            this.usrVertexLabelMaximumLength.Value = 2147483647;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Position:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fi&ll color:";
            // 
            // cbxVertexLabelPosition
            // 
            this.cbxVertexLabelPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertexLabelPosition.FormattingEnabled = true;
            this.cbxVertexLabelPosition.Location = new System.Drawing.Point(65, 20);
            this.cbxVertexLabelPosition.Name = "cbxVertexLabelPosition";
            this.cbxVertexLabelPosition.Size = new System.Drawing.Size(118, 21);
            this.cbxVertexLabelPosition.TabIndex = 1;
            // 
            // usrVertexLabelFillColor
            // 
            this.usrVertexLabelFillColor.Color = System.Drawing.Color.White;
            this.usrVertexLabelFillColor.Location = new System.Drawing.Point(83, 20);
            this.usrVertexLabelFillColor.Name = "usrVertexLabelFillColor";
            this.usrVertexLabelFillColor.ShowButton = true;
            this.usrVertexLabelFillColor.Size = new System.Drawing.Size(64, 32);
            this.usrVertexLabelFillColor.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.usrEdgeLabelTextColor);
            this.groupBox1.Controls.Add(this.btnEdgeFont);
            this.groupBox1.Controls.Add(this.usrEdgeLabelMaximumLength);
            this.groupBox1.Location = new System.Drawing.Point(14, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 362);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edge labels";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Te&xt color:";
            // 
            // usrEdgeLabelTextColor
            // 
            this.usrEdgeLabelTextColor.Color = System.Drawing.Color.White;
            this.usrEdgeLabelTextColor.Location = new System.Drawing.Point(77, 107);
            this.usrEdgeLabelTextColor.Name = "usrEdgeLabelTextColor";
            this.usrEdgeLabelTextColor.ShowButton = true;
            this.usrEdgeLabelTextColor.Size = new System.Drawing.Size(64, 32);
            this.usrEdgeLabelTextColor.TabIndex = 3;
            // 
            // usrEdgeLabelMaximumLength
            // 
            this.usrEdgeLabelMaximumLength.Location = new System.Drawing.Point(12, 55);
            this.usrEdgeLabelMaximumLength.Name = "usrEdgeLabelMaximumLength";
            this.usrEdgeLabelMaximumLength.Size = new System.Drawing.Size(183, 52);
            this.usrEdgeLabelMaximumLength.TabIndex = 1;
            this.usrEdgeLabelMaximumLength.Value = 2147483647;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnVertexFont);
            this.groupBox2.Controls.Add(this.lblWrapNarrow);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.chkVertexLabelWrapText);
            this.groupBox2.Controls.Add(this.usrVertexLabelMaximumLength);
            this.groupBox2.Controls.Add(this.lblWrapWide);
            this.groupBox2.Controls.Add(this.tbVertexLabelWrapMaxTextWidth);
            this.groupBox2.Location = new System.Drawing.Point(224, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(226, 362);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vertex labels";
            // 
            // btnVertexFont
            // 
            this.btnVertexFont.Location = new System.Drawing.Point(12, 21);
            this.btnVertexFont.Name = "btnVertexFont";
            this.btnVertexFont.Size = new System.Drawing.Size(85, 23);
            this.btnVertexFont.TabIndex = 0;
            this.btnVertexFont.Text = "F&ont...";
            this.btnVertexFont.UseVisualStyleBackColor = true;
            this.btnVertexFont.Click += new System.EventHandler(this.btnVertexFont_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbxVertexLabelPosition);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(12, 215);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(201, 58);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Label annotations";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.usrVertexLabelFillColor);
            this.groupBox4.Location = new System.Drawing.Point(12, 281);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(201, 64);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Label shapes";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnGroupFont);
            this.groupBox3.Controls.Add(this.cbxGroupLabelPosition);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.lblEdgeAlpha);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.nudGroupLabelTextAlpha);
            this.groupBox3.Controls.Add(this.usrGroupLabelTextColor);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(460, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(212, 362);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Group box labels";
            // 
            // btnGroupFont
            // 
            this.btnGroupFont.Location = new System.Drawing.Point(12, 21);
            this.btnGroupFont.Name = "btnGroupFont";
            this.btnGroupFont.Size = new System.Drawing.Size(85, 23);
            this.btnGroupFont.TabIndex = 0;
            this.btnGroupFont.Text = "Fo&nt...";
            this.btnGroupFont.UseVisualStyleBackColor = true;
            this.btnGroupFont.Click += new System.EventHandler(this.btnGroupFont_Click);
            // 
            // cbxGroupLabelPosition
            // 
            this.cbxGroupLabelPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxGroupLabelPosition.FormattingEnabled = true;
            this.cbxGroupLabelPosition.Location = new System.Drawing.Point(79, 116);
            this.cbxGroupLabelPosition.Name = "cbxGroupLabelPosition";
            this.cbxGroupLabelPosition.Size = new System.Drawing.Size(118, 21);
            this.cbxGroupLabelPosition.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Po&sition:";
            // 
            // lblEdgeAlpha
            // 
            this.lblEdgeAlpha.AutoSize = true;
            this.lblEdgeAlpha.Location = new System.Drawing.Point(138, 88);
            this.lblEdgeAlpha.Name = "lblEdgeAlpha";
            this.lblEdgeAlpha.Size = new System.Drawing.Size(15, 13);
            this.lblEdgeAlpha.TabIndex = 5;
            this.lblEdgeAlpha.Text = "%";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 88);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Opacit&y:";
            // 
            // nudGroupLabelTextAlpha
            // 
            this.nudGroupLabelTextAlpha.Location = new System.Drawing.Point(79, 86);
            this.nudGroupLabelTextAlpha.Name = "nudGroupLabelTextAlpha";
            this.nudGroupLabelTextAlpha.Size = new System.Drawing.Size(56, 20);
            this.nudGroupLabelTextAlpha.TabIndex = 4;
            // 
            // usrGroupLabelTextColor
            // 
            this.usrGroupLabelTextColor.Color = System.Drawing.Color.White;
            this.usrGroupLabelTextColor.Location = new System.Drawing.Point(77, 48);
            this.usrGroupLabelTextColor.Name = "usrGroupLabelTextColor";
            this.usrGroupLabelTextColor.ShowButton = true;
            this.usrGroupLabelTextColor.Size = new System.Drawing.Size(64, 32);
            this.usrGroupLabelTextColor.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "T&ext color:";
            // 
            // LabelUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(686, 420);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LabelUserSettingsDialog";
            this.Text = "Label Options";
            ((System.ComponentModel.ISupportInitialize)(this.tbVertexLabelWrapMaxTextWidth)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGroupLabelTextAlpha)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEdgeFont;
        private Smrf.AppLib.ColorPicker usrVertexLabelFillColor;
        private Smrf.NodeXL.ExcelTemplate.VertexLabelPositionComboBox cbxVertexLabelPosition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private MaximumLabelLengthControl usrVertexLabelMaximumLength;
        private MaximumLabelLengthControl usrEdgeLabelMaximumLength;
        private System.Windows.Forms.Label label3;
        private Smrf.AppLib.ColorPicker usrEdgeLabelTextColor;
        private System.Windows.Forms.CheckBox chkVertexLabelWrapText;
        private System.Windows.Forms.Label lblWrapWide;
        private System.Windows.Forms.TrackBar tbVertexLabelWrapMaxTextWidth;
        private System.Windows.Forms.Label lblWrapNarrow;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private Smrf.AppLib.ColorPicker usrGroupLabelTextColor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblEdgeAlpha;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nudGroupLabelTextAlpha;
        private VertexLabelPositionComboBox cbxGroupLabelPosition;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnVertexFont;
        private System.Windows.Forms.Button btnGroupFont;
    }
}
