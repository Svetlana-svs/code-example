namespace SM3G
{
    partial class DetectorPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lRms = new System.Windows.Forms.Label();
            this.m_txbRms = new System.Windows.Forms.TextBox();
            this.m_lTitle = new System.Windows.Forms.Label();
            this.m_txbRmsShockLess = new System.Windows.Forms.TextBox();
            this.panel = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.label11 = new System.Windows.Forms.Label();
            this.m_txbPeakMsd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_txbPeakAvg = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_txbPeakFactor = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.m_txbRmsQuotient = new System.Windows.Forms.TextBox();
            this.m_txbWapFactor = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.m_btnWapFactor = new System.Windows.Forms.Button();
            this.pWapFactor = new System.Windows.Forms.Panel();
            this.m_txbPeakNumberAvg = new System.Windows.Forms.TextBox();
            this.m_txbPeakMaxAvgRmsShockLessQuotient = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.m_imgPeakNumberAvg = new System.Windows.Forms.PictureBox();
            this.m_imgPeakMaxAvgRmsShockLessQuotient = new System.Windows.Forms.PictureBox();
            this.m_imgRmsQuotient = new System.Windows.Forms.PictureBox();
            this.m_imgWapFactor = new System.Windows.Forms.PictureBox();
            this.m_imgPeakFactor = new System.Windows.Forms.PictureBox();
            this.m_imgRms = new System.Windows.Forms.PictureBox();
            this.pDetail = new System.Windows.Forms.Panel();
            this.m_cbxDescription = new System.Windows.Forms.ComboBox();
            this.panel.SuspendLayout();
            this.pWapFactor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgPeakNumberAvg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgPeakMaxAvgRmsShockLessQuotient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgRmsQuotient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgWapFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgPeakFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgRms)).BeginInit();
            this.pDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // lRms
            // 
            this.lRms.AutoSize = true;
            this.lRms.Location = new System.Drawing.Point(8, 6);
            this.lRms.Name = "lRms";
            this.lRms.Size = new System.Drawing.Size(14, 13);
            this.lRms.TabIndex = 0;
            this.lRms.Text = "E";
            // 
            // m_txbRms
            // 
            this.m_txbRms.Location = new System.Drawing.Point(127, 16);
            this.m_txbRms.Name = "m_txbRms";
            this.m_txbRms.ReadOnly = true;
            this.m_txbRms.Size = new System.Drawing.Size(45, 20);
            this.m_txbRms.TabIndex = 3;
            this.m_txbRms.Text = "0,00";
            this.m_txbRms.Visible = false;
            // 
            // m_lTitle
            // 
            this.m_lTitle.AutoSize = true;
            this.m_lTitle.Location = new System.Drawing.Point(1, 3);
            this.m_lTitle.Name = "m_lTitle";
            this.m_lTitle.Size = new System.Drawing.Size(56, 13);
            this.m_lTitle.TabIndex = 2;
            this.m_lTitle.Text = "Детектор";
            // 
            // m_txbRmsShockLess
            // 
            this.m_txbRmsShockLess.Location = new System.Drawing.Point(127, 32);
            this.m_txbRmsShockLess.Name = "m_txbRmsShockLess";
            this.m_txbRmsShockLess.ReadOnly = true;
            this.m_txbRmsShockLess.Size = new System.Drawing.Size(45, 20);
            this.m_txbRmsShockLess.TabIndex = 3;
            this.m_txbRmsShockLess.Text = "0.00";
            this.m_txbRmsShockLess.Visible = false;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Controls.Add(this.treeView);
            this.panel.Controls.Add(this.label11);
            this.panel.Controls.Add(this.m_txbPeakMsd);
            this.panel.Controls.Add(this.label1);
            this.panel.Controls.Add(this.m_txbPeakAvg);
            this.panel.Location = new System.Drawing.Point(145, 8);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(80, 137);
            this.panel.TabIndex = 5;
            this.panel.Visible = false;
            // 
            // treeView
            // 
            this.treeView.Location = new System.Drawing.Point(1, 35);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(80, 102);
            this.treeView.TabIndex = 59;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(1, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 32);
            this.label11.TabIndex = 33;
            this.label11.Text = "Число ударов за один об.";
            // 
            // m_txbPeakMsd
            // 
            this.m_txbPeakMsd.BackColor = System.Drawing.SystemColors.Control;
            this.m_txbPeakMsd.Location = new System.Drawing.Point(58, 33);
            this.m_txbPeakMsd.Name = "m_txbPeakMsd";
            this.m_txbPeakMsd.ReadOnly = true;
            this.m_txbPeakMsd.Size = new System.Drawing.Size(45, 20);
            this.m_txbPeakMsd.TabIndex = 8;
            this.m_txbPeakMsd.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 64;
            this.label1.Text = "СЧ-У";
            this.label1.Visible = false;
            // 
            // m_txbPeakAvg
            // 
            this.m_txbPeakAvg.BackColor = System.Drawing.SystemColors.Control;
            this.m_txbPeakAvg.Location = new System.Drawing.Point(56, 59);
            this.m_txbPeakAvg.Name = "m_txbPeakAvg";
            this.m_txbPeakAvg.ReadOnly = true;
            this.m_txbPeakAvg.Size = new System.Drawing.Size(45, 20);
            this.m_txbPeakAvg.TabIndex = 9;
            this.m_txbPeakAvg.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(177, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "ПФ";
            this.label2.Visible = false;
            // 
            // m_txbPeakFactor
            // 
            this.m_txbPeakFactor.Location = new System.Drawing.Point(178, 16);
            this.m_txbPeakFactor.Name = "m_txbPeakFactor";
            this.m_txbPeakFactor.ReadOnly = true;
            this.m_txbPeakFactor.Size = new System.Drawing.Size(45, 20);
            this.m_txbPeakFactor.TabIndex = 4;
            this.m_txbPeakFactor.Text = "0.0000";
            this.m_txbPeakFactor.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(127, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "СКЗ-П";
            this.label9.Visible = false;
            // 
            // m_txbRmsQuotient
            // 
            this.m_txbRmsQuotient.BackColor = System.Drawing.SystemColors.Control;
            this.m_txbRmsQuotient.Location = new System.Drawing.Point(11, 45);
            this.m_txbRmsQuotient.Name = "m_txbRmsQuotient";
            this.m_txbRmsQuotient.Size = new System.Drawing.Size(40, 20);
            this.m_txbRmsQuotient.TabIndex = 5;
            this.m_txbRmsQuotient.Text = "0.0000";
            this.m_txbRmsQuotient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_txbWapFactor
            // 
            this.m_txbWapFactor.Location = new System.Drawing.Point(130, 150);
            this.m_txbWapFactor.Name = "m_txbWapFactor";
            this.m_txbWapFactor.ReadOnly = true;
            this.m_txbWapFactor.Size = new System.Drawing.Size(45, 20);
            this.m_txbWapFactor.TabIndex = 47;
            this.m_txbWapFactor.Text = "0.0000";
            this.m_txbWapFactor.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "ПФ";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(19, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "С";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_btnWapFactor
            // 
            this.m_btnWapFactor.Location = new System.Drawing.Point(6, 57);
            this.m_btnWapFactor.Name = "m_btnWapFactor";
            this.m_btnWapFactor.Size = new System.Drawing.Size(42, 24);
            this.m_btnWapFactor.TabIndex = 1;
            this.m_btnWapFactor.Text = "ВФ...";
            this.m_btnWapFactor.UseVisualStyleBackColor = true;
            // 
            // pWapFactor
            // 
            this.pWapFactor.BackColor = System.Drawing.SystemColors.Control;
            this.pWapFactor.Controls.Add(this.m_txbPeakNumberAvg);
            this.pWapFactor.Controls.Add(this.m_txbPeakMaxAvgRmsShockLessQuotient);
            this.pWapFactor.Controls.Add(this.label8);
            this.pWapFactor.Controls.Add(this.label6);
            this.pWapFactor.Controls.Add(this.m_imgPeakNumberAvg);
            this.pWapFactor.Controls.Add(this.m_imgPeakMaxAvgRmsShockLessQuotient);
            this.pWapFactor.Controls.Add(this.m_imgRmsQuotient);
            this.pWapFactor.Controls.Add(this.label5);
            this.pWapFactor.Controls.Add(this.m_txbRmsQuotient);
            this.pWapFactor.Location = new System.Drawing.Point(0, 82);
            this.pWapFactor.Name = "pWapFactor";
            this.pWapFactor.Size = new System.Drawing.Size(145, 65);
            this.pWapFactor.TabIndex = 70;
            this.pWapFactor.Visible = false;
            // 
            // m_txbPeakNumberAvg
            // 
            this.m_txbPeakNumberAvg.Location = new System.Drawing.Point(99, 45);
            this.m_txbPeakNumberAvg.Name = "m_txbPeakNumberAvg";
            this.m_txbPeakNumberAvg.ReadOnly = true;
            this.m_txbPeakNumberAvg.Size = new System.Drawing.Size(40, 20);
            this.m_txbPeakNumberAvg.TabIndex = 7;
            this.m_txbPeakNumberAvg.Text = "0.0000";
            this.m_txbPeakNumberAvg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_txbPeakMaxAvgRmsShockLessQuotient
            // 
            this.m_txbPeakMaxAvgRmsShockLessQuotient.Location = new System.Drawing.Point(55, 45);
            this.m_txbPeakMaxAvgRmsShockLessQuotient.Name = "m_txbPeakMaxAvgRmsShockLessQuotient";
            this.m_txbPeakMaxAvgRmsShockLessQuotient.ReadOnly = true;
            this.m_txbPeakMaxAvgRmsShockLessQuotient.Size = new System.Drawing.Size(40, 20);
            this.m_txbPeakMaxAvgRmsShockLessQuotient.TabIndex = 6;
            this.m_txbPeakMaxAvgRmsShockLessQuotient.Text = "0.0000";
            this.m_txbPeakMaxAvgRmsShockLessQuotient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(63, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 13);
            this.label8.TabIndex = 47;
            this.label8.Text = "P";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(107, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "N";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_imgPeakNumberAvg
            // 
            this.m_imgPeakNumberAvg.Location = new System.Drawing.Point(0, 0);
            this.m_imgPeakNumberAvg.Name = "m_imgPeakNumberAvg";
            this.m_imgPeakNumberAvg.Size = new System.Drawing.Size(100, 50);
            this.m_imgPeakNumberAvg.TabIndex = 48;
            this.m_imgPeakNumberAvg.TabStop = false;
            // 
            // m_imgPeakMaxAvgRmsShockLessQuotient
            // 
            this.m_imgPeakMaxAvgRmsShockLessQuotient.Location = new System.Drawing.Point(0, 0);
            this.m_imgPeakMaxAvgRmsShockLessQuotient.Name = "m_imgPeakMaxAvgRmsShockLessQuotient";
            this.m_imgPeakMaxAvgRmsShockLessQuotient.Size = new System.Drawing.Size(100, 50);
            this.m_imgPeakMaxAvgRmsShockLessQuotient.TabIndex = 49;
            this.m_imgPeakMaxAvgRmsShockLessQuotient.TabStop = false;
            // 
            // m_imgRmsQuotient
            // 
            this.m_imgRmsQuotient.Location = new System.Drawing.Point(0, 0);
            this.m_imgRmsQuotient.Name = "m_imgRmsQuotient";
            this.m_imgRmsQuotient.Size = new System.Drawing.Size(100, 50);
            this.m_imgRmsQuotient.TabIndex = 50;
            this.m_imgRmsQuotient.TabStop = false;
            // 
            // m_imgWapFactor
            // 
            this.m_imgWapFactor.Location = new System.Drawing.Point(0, 0);
            this.m_imgWapFactor.Name = "m_imgWapFactor";
            this.m_imgWapFactor.Size = new System.Drawing.Size(100, 50);
            this.m_imgWapFactor.TabIndex = 73;
            this.m_imgWapFactor.TabStop = false;
            // 
            // m_imgPeakFactor
            // 
            this.m_imgPeakFactor.Location = new System.Drawing.Point(0, 0);
            this.m_imgPeakFactor.Name = "m_imgPeakFactor";
            this.m_imgPeakFactor.Size = new System.Drawing.Size(100, 50);
            this.m_imgPeakFactor.TabIndex = 71;
            this.m_imgPeakFactor.TabStop = false;
            // 
            // m_imgRms
            // 
            this.m_imgRms.Location = new System.Drawing.Point(0, 0);
            this.m_imgRms.Name = "m_imgRms";
            this.m_imgRms.Size = new System.Drawing.Size(100, 50);
            this.m_imgRms.TabIndex = 72;
            this.m_imgRms.TabStop = false;
            // 
            // pDetail
            // 
            this.pDetail.Controls.Add(this.pWapFactor);
            this.pDetail.Controls.Add(this.label7);
            this.pDetail.Controls.Add(this.panel);
            this.pDetail.Controls.Add(this.lRms);
            this.pDetail.Controls.Add(this.m_btnWapFactor);
            this.pDetail.Controls.Add(this.m_imgPeakFactor);
            this.pDetail.Controls.Add(this.m_imgRms);
            this.pDetail.Controls.Add(this.m_imgWapFactor);
            this.pDetail.Controls.Add(this.m_txbWapFactor);
            this.pDetail.Location = new System.Drawing.Point(0, 51);
            this.pDetail.Name = "pDetail";
            this.pDetail.Size = new System.Drawing.Size(225, 87);
            this.pDetail.TabIndex = 72;
            this.pDetail.Visible = false;
            // 
            // m_cbxDescription
            // 
            this.m_cbxDescription.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_cbxDescription.FormattingEnabled = true;
            this.m_cbxDescription.Location = new System.Drawing.Point(3, 33);
            this.m_cbxDescription.Name = "m_cbxDescription";
            this.m_cbxDescription.Size = new System.Drawing.Size(219, 21);
            this.m_cbxDescription.TabIndex = 73;
            // 
            // DetectorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.m_cbxDescription);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_txbRmsShockLess);
            this.Controls.Add(this.m_txbRms);
            this.Controls.Add(this.pDetail);
            this.Controls.Add(this.m_txbPeakFactor);
            this.Controls.Add(this.m_lTitle);
            this.Controls.Add(this.label9);
            this.Name = "DetectorPanel";
            this.Size = new System.Drawing.Size(226, 81);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.pWapFactor.ResumeLayout(false);
            this.pWapFactor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgPeakNumberAvg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgPeakMaxAvgRmsShockLessQuotient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgRmsQuotient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgWapFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgPeakFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_imgRms)).EndInit();
            this.pDetail.ResumeLayout(false);
            this.pDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lRms;
        private System.Windows.Forms.TextBox m_txbRms;
        private System.Windows.Forms.Label m_lTitle;
        private System.Windows.Forms.TextBox m_txbRmsShockLess;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox m_txbWapFactor;
        private System.Windows.Forms.TextBox m_txbPeakFactor;
        private System.Windows.Forms.TextBox m_txbRmsQuotient;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.TextBox m_txbPeakAvg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_txbPeakMsd;
        private System.Windows.Forms.PictureBox m_imgRms;
        private System.Windows.Forms.PictureBox m_imgPeakFactor;
        private System.Windows.Forms.PictureBox m_imgWapFactor;
        private System.Windows.Forms.Button m_btnWapFactor;
        private System.Windows.Forms.Panel pWapFactor;
        private System.Windows.Forms.PictureBox m_imgPeakNumberAvg;
        private System.Windows.Forms.PictureBox m_imgPeakMaxAvgRmsShockLessQuotient;
        private System.Windows.Forms.PictureBox m_imgRmsQuotient;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox m_txbPeakNumberAvg;
        private System.Windows.Forms.TextBox m_txbPeakMaxAvgRmsShockLessQuotient;
        private System.Windows.Forms.Panel pDetail;
        private System.Windows.Forms.ComboBox m_cbxDescription;
    }
}
