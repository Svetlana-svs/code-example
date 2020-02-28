namespace SM3G
{
    partial class SettingsForm
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
            this.m_dtpDateTimeStart = new System.Windows.Forms.DateTimePicker();
            this.m_dtpDateTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.m_nDetector1 = new System.Windows.Forms.NumericUpDown();
            this.groupDetector = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_nDetector4 = new System.Windows.Forms.NumericUpDown();
            this.m_nDetector3 = new System.Windows.Forms.NumericUpDown();
            this.m_nDetector2 = new System.Windows.Forms.NumericUpDown();
            this.lblDateTimeStart = new System.Windows.Forms.Label();
            this.lblDateTimeEnd = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_nDetector1)).BeginInit();
            this.groupDetector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nDetector4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nDetector3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nDetector2)).BeginInit();
            this.SuspendLayout();
            // 
            // m_dtpDateTimeStart
            // 
            this.m_dtpDateTimeStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.m_dtpDateTimeStart.Location = new System.Drawing.Point(53, 44);
            this.m_dtpDateTimeStart.Name = "m_dtpDateTimeStart";
            this.m_dtpDateTimeStart.Size = new System.Drawing.Size(200, 20);
            this.m_dtpDateTimeStart.TabIndex = 0;
            this.m_dtpDateTimeStart.CloseUp += new System.EventHandler(this.m_dtpDateTimeStart_CloseUp);
            // 
            // m_dtpDateTimeEnd
            // 
            this.m_dtpDateTimeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.m_dtpDateTimeEnd.Location = new System.Drawing.Point(319, 44);
            this.m_dtpDateTimeEnd.Name = "m_dtpDateTimeEnd";
            this.m_dtpDateTimeEnd.Size = new System.Drawing.Size(200, 20);
            this.m_dtpDateTimeEnd.TabIndex = 1;
            this.m_dtpDateTimeEnd.CloseUp += new System.EventHandler(this.m_dtpDateTimeEnd_CloseUp);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(422, 195);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(97, 29);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(263, 195);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(97, 29);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // m_nDetector1
            // 
            this.m_nDetector1.Location = new System.Drawing.Point(21, 27);
            this.m_nDetector1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.m_nDetector1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nDetector1.Name = "m_nDetector1";
            this.m_nDetector1.Size = new System.Drawing.Size(69, 20);
            this.m_nDetector1.TabIndex = 6;
            this.m_nDetector1.Tag = "0";
            this.m_nDetector1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // groupDetector
            // 
            this.groupDetector.Controls.Add(this.m_nDetector1);
            this.groupDetector.Controls.Add(this.m_nDetector2);
            this.groupDetector.Controls.Add(this.m_nDetector3);
            this.groupDetector.Controls.Add(this.m_nDetector4);
            this.groupDetector.Controls.Add(this.label1);
            this.groupDetector.Location = new System.Drawing.Point(53, 79);
            this.groupDetector.Name = "groupDetector";
            this.groupDetector.Size = new System.Drawing.Size(466, 73);
            this.groupDetector.TabIndex = 7;
            this.groupDetector.TabStop = false;
            this.groupDetector.Text = "Номер детектора";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 10;
            // 
            // m_nDetector4
            // 
            this.m_nDetector4.Location = new System.Drawing.Point(381, 27);
            this.m_nDetector4.Name = "m_nDetector4";
            this.m_nDetector4.Size = new System.Drawing.Size(69, 20);
            this.m_nDetector4.TabIndex = 9;
            this.m_nDetector4.Tag = "3";
            // 
            // m_nDetector3
            // 
            this.m_nDetector3.Location = new System.Drawing.Point(261, 27);
            this.m_nDetector3.Name = "m_nDetector3";
            this.m_nDetector3.Size = new System.Drawing.Size(69, 20);
            this.m_nDetector3.TabIndex = 8;
            this.m_nDetector3.Tag = "2";
            // 
            // m_nDetector2
            // 
            this.m_nDetector2.Location = new System.Drawing.Point(141, 27);
            this.m_nDetector2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.m_nDetector2.Name = "m_nDetector2";
            this.m_nDetector2.Size = new System.Drawing.Size(69, 20);
            this.m_nDetector2.TabIndex = 7;
            this.m_nDetector2.Tag = "1";
            // 
            // lblDateTimeStart
            // 
            this.lblDateTimeStart.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblDateTimeStart.Location = new System.Drawing.Point(53, 20);
            this.lblDateTimeStart.Name = "lblDateTimeStart";
            this.lblDateTimeStart.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblDateTimeStart.Size = new System.Drawing.Size(200, 18);
            this.lblDateTimeStart.TabIndex = 8;
            // 
            // lblDateTimeEnd
            // 
            this.lblDateTimeEnd.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblDateTimeEnd.Location = new System.Drawing.Point(319, 20);
            this.lblDateTimeEnd.Name = "lblDateTimeEnd";
            this.lblDateTimeEnd.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblDateTimeEnd.Size = new System.Drawing.Size(200, 18);
            this.lblDateTimeEnd.TabIndex = 9;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 262);
            this.Controls.Add(this.lblDateTimeEnd);
            this.Controls.Add(this.lblDateTimeStart);
            this.Controls.Add(this.groupDetector);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.m_dtpDateTimeEnd);
            this.Controls.Add(this.m_dtpDateTimeStart);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_nDetector1)).EndInit();
            this.groupDetector.ResumeLayout(false);
            this.groupDetector.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nDetector4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nDetector3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nDetector2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker m_dtpDateTimeStart;
        private System.Windows.Forms.DateTimePicker m_dtpDateTimeEnd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.NumericUpDown m_nDetector1;
        private System.Windows.Forms.GroupBox groupDetector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown m_nDetector4;
        private System.Windows.Forms.NumericUpDown m_nDetector3;
        private System.Windows.Forms.NumericUpDown m_nDetector2;
        private System.Windows.Forms.Label lblDateTimeStart;
        private System.Windows.Forms.Label lblDateTimeEnd;
    }
}