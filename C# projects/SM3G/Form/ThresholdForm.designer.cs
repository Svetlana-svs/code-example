namespace SM3G
{
    partial class ThresholdForm
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
            this.m_nThresholdN = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_nThresholdL = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.m_nThresholdW = new System.Windows.Forms.NumericUpDown();
            this.m_nThresholdH = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_nThresholdN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nThresholdL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nThresholdW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nThresholdH)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_nThresholdN
            // 
            this.m_nThresholdN.DecimalPlaces = 2;
            this.m_nThresholdN.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_nThresholdN.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.m_nThresholdN.Location = new System.Drawing.Point(149, 92);
            this.m_nThresholdN.Name = "m_nThresholdN";
            this.m_nThresholdN.Size = new System.Drawing.Size(75, 31);
            this.m_nThresholdN.TabIndex = 1;
            this.m_nThresholdN.TabStop = false;
            this.m_nThresholdN.Tag = "1";
            this.m_nThresholdN.Value = new decimal(new int[] {
            2,
            0,
            0,
            131072});
            this.m_nThresholdN.Click += new System.EventHandler(this.m_nThreshold_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(227, 213);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(101, 35);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(441, 213);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 35);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Порог";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(151, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "Норма";
            // 
            // m_nThresholdL
            // 
            this.m_nThresholdL.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.m_nThresholdL.DecimalPlaces = 2;
            this.m_nThresholdL.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_nThresholdL.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.m_nThresholdL.Location = new System.Drawing.Point(26, 92);
            this.m_nThresholdL.Name = "m_nThresholdL";
            this.m_nThresholdL.Size = new System.Drawing.Size(75, 31);
            this.m_nThresholdL.TabIndex = 0;
            this.m_nThresholdL.TabStop = false;
            this.m_nThresholdL.Tag = "0";
            this.m_nThresholdL.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.m_nThresholdL.Click += new System.EventHandler(this.m_nThreshold_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 24);
            this.label4.TabIndex = 14;
            this.label4.Text = "Ниже нормы";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(238, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(169, 24);
            this.label5.TabIndex = 18;
            this.label5.Text = "Предупреждение";
            // 
            // m_nThresholdW
            // 
            this.m_nThresholdW.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.m_nThresholdW.DecimalPlaces = 2;
            this.m_nThresholdW.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_nThresholdW.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.m_nThresholdW.Location = new System.Drawing.Point(285, 92);
            this.m_nThresholdW.Name = "m_nThresholdW";
            this.m_nThresholdW.Size = new System.Drawing.Size(75, 31);
            this.m_nThresholdW.TabIndex = 15;
            this.m_nThresholdW.TabStop = false;
            this.m_nThresholdW.Tag = "2";
            this.m_nThresholdW.Value = new decimal(new int[] {
            3,
            0,
            0,
            131072});
            this.m_nThresholdW.Click += new System.EventHandler(this.m_nThreshold_Click);
            // 
            // m_nThresholdH
            // 
            this.m_nThresholdH.DecimalPlaces = 2;
            this.m_nThresholdH.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_nThresholdH.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.m_nThresholdH.Location = new System.Drawing.Point(440, 92);
            this.m_nThresholdH.Name = "m_nThresholdH";
            this.m_nThresholdH.Size = new System.Drawing.Size(75, 31);
            this.m_nThresholdH.TabIndex = 16;
            this.m_nThresholdH.TabStop = false;
            this.m_nThresholdH.Tag = "3";
            this.m_nThresholdH.Value = new decimal(new int[] {
            4,
            0,
            0,
            131072});
            this.m_nThresholdH.Click += new System.EventHandler(this.m_nThreshold_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(425, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 24);
            this.label6.TabIndex = 17;
            this.label6.Text = "Опасность";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.m_nThresholdN);
            this.groupBox1.Controls.Add(this.m_nThresholdL);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.m_nThresholdH);
            this.groupBox1.Controls.Add(this.m_nThresholdW);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(108, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 150);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // ThresholdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 270);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "ThresholdForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Пороги";
            this.Load += new System.EventHandler(this.form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_nThresholdN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nThresholdL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nThresholdW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nThresholdH)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown m_nThresholdN;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown m_nThresholdL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown m_nThresholdW;
        private System.Windows.Forms.NumericUpDown m_nThresholdH;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}