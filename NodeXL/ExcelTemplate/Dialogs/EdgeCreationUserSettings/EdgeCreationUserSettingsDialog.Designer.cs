

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class EdgeCreationUserSettingsDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblColumn = new System.Windows.Forms.Label();
            this.cbxVertexColumn = new Smrf.NodeXL.ExcelTemplate.VertexColumnComboBox();
            this.lblColumnAnnot = new System.Windows.Forms.Label();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.lblThrePerc = new System.Windows.Forms.Label();
            this.nudThreshold = new System.Windows.Forms.NumericUpDown();
            this.lblEdgeLimit = new System.Windows.Forms.Label();
            this.lblThresholdAnnot = new System.Windows.Forms.Label();
            this.nudEdgeLimit = new System.Windows.Forms.NumericUpDown();
            this.lblEdgeLimitUnit = new System.Windows.Forms.Label();
            this.lblEdgeLimitAnnot = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(857, 319);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 44);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(636, 319);
            this.btnOK.Margin = new System.Windows.Forms.Padding(6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(160, 44);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblColumn
            // 
            this.lblColumn.AutoSize = true;
            this.lblColumn.Location = new System.Drawing.Point(24, 38);
            this.lblColumn.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblColumn.Name = "lblColumn";
            this.lblColumn.Size = new System.Drawing.Size(358, 25);
            this.lblColumn.TabIndex = 0;
            this.lblColumn.Text = "Analyze the contents of this column:";
            // 
            // cbxVertexColumn
            // 
            this.cbxVertexColumn.FormattingEnabled = true;
            this.cbxVertexColumn.Location = new System.Drawing.Point(458, 35);
            this.cbxVertexColumn.Name = "cbxVertexColumn";
            this.cbxVertexColumn.Size = new System.Drawing.Size(559, 33);
            this.cbxVertexColumn.TabIndex = 1;
            // 
            // lblColumnAnnot
            // 
            this.lblColumnAnnot.AutoEllipsis = true;
            this.lblColumnAnnot.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblColumnAnnot.Location = new System.Drawing.Point(24, 71);
            this.lblColumnAnnot.Name = "lblColumnAnnot";
            this.lblColumnAnnot.Size = new System.Drawing.Size(1011, 28);
            this.lblColumnAnnot.TabIndex = 0;
            this.lblColumnAnnot.Text = "(Please select a column of space or comma delimited text to be used to calculate " +
    "user similarities.)";
            // 
            // lblThreshold
            // 
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(24, 121);
            this.lblThreshold.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(368, 25);
            this.lblThreshold.TabIndex = 0;
            this.lblThreshold.Text = "Strength threshold for edge creation: ";
            // 
            // lblThrePerc
            // 
            this.lblThrePerc.AutoSize = true;
            this.lblThrePerc.Location = new System.Drawing.Point(587, 121);
            this.lblThrePerc.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblThrePerc.Name = "lblThrePerc";
            this.lblThrePerc.Size = new System.Drawing.Size(31, 25);
            this.lblThrePerc.TabIndex = 12;
            this.lblThrePerc.Text = "%";
            // 
            // nudThreshold
            // 
            this.nudThreshold.Location = new System.Drawing.Point(458, 119);
            this.nudThreshold.Name = "nudThreshold";
            this.nudThreshold.Size = new System.Drawing.Size(120, 31);
            this.nudThreshold.TabIndex = 2;
            this.nudThreshold.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // lblEdgeLimit
            // 
            this.lblEdgeLimit.AutoSize = true;
            this.lblEdgeLimit.Location = new System.Drawing.Point(24, 227);
            this.lblEdgeLimit.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblEdgeLimit.Name = "lblEdgeLimit";
            this.lblEdgeLimit.Size = new System.Drawing.Size(412, 25);
            this.lblEdgeLimit.TabIndex = 0;
            this.lblEdgeLimit.Text = "Maximum number of new edges to create:";
            // 
            // lblThresholdAnnot
            // 
            this.lblThresholdAnnot.AutoEllipsis = true;
            this.lblThresholdAnnot.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblThresholdAnnot.Location = new System.Drawing.Point(24, 159);
            this.lblThresholdAnnot.Name = "lblThresholdAnnot";
            this.lblThresholdAnnot.Size = new System.Drawing.Size(924, 54);
            this.lblThresholdAnnot.TabIndex = 0;
            this.lblThresholdAnnot.Text = "(Higher threshold values will generate fewer edges. Lower values will generate mo" +
    "re edges.  Select a level that will generate the number of edges you desire.)";
            // 
            // nudEdgeLimit
            // 
            this.nudEdgeLimit.Location = new System.Drawing.Point(458, 225);
            this.nudEdgeLimit.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudEdgeLimit.Name = "nudEdgeLimit";
            this.nudEdgeLimit.Size = new System.Drawing.Size(120, 31);
            this.nudEdgeLimit.TabIndex = 3;
            this.nudEdgeLimit.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // lblEdgeLimitUnit
            // 
            this.lblEdgeLimitUnit.AutoSize = true;
            this.lblEdgeLimitUnit.Location = new System.Drawing.Point(587, 227);
            this.lblEdgeLimitUnit.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblEdgeLimitUnit.Name = "lblEdgeLimitUnit";
            this.lblEdgeLimitUnit.Size = new System.Drawing.Size(73, 25);
            this.lblEdgeLimitUnit.TabIndex = 0;
            this.lblEdgeLimitUnit.Text = "Edges";
            this.lblEdgeLimitUnit.UseMnemonic = false;
            // 
            // lblEdgeLimitAnnot
            // 
            this.lblEdgeLimitAnnot.AutoEllipsis = true;
            this.lblEdgeLimitAnnot.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblEdgeLimitAnnot.Location = new System.Drawing.Point(24, 259);
            this.lblEdgeLimitAnnot.Name = "lblEdgeLimitAnnot";
            this.lblEdgeLimitAnnot.Size = new System.Drawing.Size(924, 54);
            this.lblEdgeLimitAnnot.TabIndex = 0;
            this.lblEdgeLimitAnnot.Text = "(Note, it is possible that we will create fewer than this number)";
            // 
            // EdgeCreationUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1047, 394);
            this.Controls.Add(this.lblEdgeLimitAnnot);
            this.Controls.Add(this.lblEdgeLimitUnit);
            this.Controls.Add(this.nudEdgeLimit);
            this.Controls.Add(this.lblThresholdAnnot);
            this.Controls.Add(this.lblEdgeLimit);
            this.Controls.Add(this.nudThreshold);
            this.Controls.Add(this.lblThrePerc);
            this.Controls.Add(this.lblThreshold);
            this.Controls.Add(this.lblColumnAnnot);
            this.Controls.Add(this.cbxVertexColumn);
            this.Controls.Add(this.lblColumn);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EdgeCreationUserSettingsDialog";
            this.Text = "Edge Creation Metrics";
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblColumn;
        private VertexColumnComboBox cbxVertexColumn;
        private System.Windows.Forms.Label lblColumnAnnot;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.Label lblThrePerc;
        private System.Windows.Forms.NumericUpDown nudThreshold;
        private System.Windows.Forms.Label lblEdgeLimit;
        private System.Windows.Forms.Label lblThresholdAnnot;
        private System.Windows.Forms.NumericUpDown nudEdgeLimit;
        private System.Windows.Forms.Label lblEdgeLimitUnit;
        private System.Windows.Forms.Label lblEdgeLimitAnnot;
    }
}
