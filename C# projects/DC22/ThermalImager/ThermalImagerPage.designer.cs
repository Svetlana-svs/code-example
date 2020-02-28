namespace DC22.Shell.Controls.Apps
{
    partial class ThermalImagerPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThermalImagerPage));
            this.pbxThermalmager = new System.Windows.Forms.PictureBox();
            this.labelMin = new System.Windows.Forms.Label();
            this.labelAverage = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.m_labelMin = new System.Windows.Forms.Label();
            this.m_labelMax = new System.Windows.Forms.Label();
            this.m_labelAverage = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer();
            this.pbxBlack = new System.Windows.Forms.PictureBox();
            this.pbxRed = new System.Windows.Forms.PictureBox();
            this.panelTemperatute = new System.Windows.Forms.Panel();
            this.pbxArrowBlackPT = new System.Windows.Forms.PictureBox();
            this.pbxArrowWhitePT = new System.Windows.Forms.PictureBox();
            this.panelButton = new System.Windows.Forms.Panel();
            this.labelCalibration = new System.Windows.Forms.Label();
            this.pbxArrowWhitePB = new System.Windows.Forms.PictureBox();
            this.pbxArrowBlackPB = new System.Windows.Forms.PictureBox();
            this.labelCalibrationButton = new System.Windows.Forms.Label();
            this.labelColor = new System.Windows.Forms.Label();
            this.labelStart = new System.Windows.Forms.Label();
            this.contextLabelStop = new DC22.Shell.Controls.Common.ContextLabel();
            this.contextLabelColor = new DC22.Shell.Controls.Common.ContextLabel();
            this.m_helpControl = new DC22.Shell.Help.HelpControl();
            this.panelTemperatute.SuspendLayout();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbxThermalmager
            // 
            this.pbxThermalmager.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxThermalmager.Location = new System.Drawing.Point(0, 0);
            this.pbxThermalmager.Name = "pbxThermalmager";
            this.pbxThermalmager.Size = new System.Drawing.Size(500, 243);
            this.pbxThermalmager.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // labelMin
            // 
            this.labelMin.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelMin.Location = new System.Drawing.Point(22, 2);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(47, 23);
            this.labelMin.Text = "Мин.:";
            // 
            // labelAverage
            // 
            this.labelAverage.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelAverage.Location = new System.Drawing.Point(324, 2);
            this.labelAverage.Name = "labelAverage";
            this.labelAverage.Size = new System.Drawing.Size(73, 23);
            this.labelAverage.Text = "Средняя:";
            // 
            // labelMax
            // 
            this.labelMax.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelMax.Location = new System.Drawing.Point(182, 2);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(48, 23);
            this.labelMax.Text = "Мах.:";
            // 
            // m_labelMin
            // 
            this.m_labelMin.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.m_labelMin.Location = new System.Drawing.Point(69, 2);
            this.m_labelMin.Name = "m_labelMin";
            this.m_labelMin.Size = new System.Drawing.Size(70, 23);
            // 
            // m_labelMax
            // 
            this.m_labelMax.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.m_labelMax.Location = new System.Drawing.Point(230, 2);
            this.m_labelMax.Name = "m_labelMax";
            this.m_labelMax.Size = new System.Drawing.Size(70, 23);
            // 
            // m_labelAverage
            // 
            this.m_labelAverage.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.m_labelAverage.Location = new System.Drawing.Point(398, 2);
            this.m_labelAverage.Name = "m_labelAverage";
            this.m_labelAverage.Size = new System.Drawing.Size(65, 23);
            // 
            // timer
            // 
            this.timer.Interval = 500;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // pbxBlack
            // 
            this.pbxBlack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pbxBlack.BackColor = System.Drawing.Color.Blue;
            this.pbxBlack.Location = new System.Drawing.Point(5, 7);
            this.pbxBlack.Name = "pbxBlack";
            this.pbxBlack.Size = new System.Drawing.Size(10, 10);
            // 
            // pbxRed
            // 
            this.pbxRed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pbxRed.BackColor = System.Drawing.Color.Red;
            this.pbxRed.Location = new System.Drawing.Point(162, 7);
            this.pbxRed.Name = "pbxRed";
            this.pbxRed.Size = new System.Drawing.Size(10, 10);
            // 
            // panelTemperatute
            // 
            this.panelTemperatute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTemperatute.Controls.Add(this.pbxArrowBlackPT);
            this.panelTemperatute.Controls.Add(this.pbxArrowWhitePT);
            this.panelTemperatute.Controls.Add(this.m_labelMax);
            this.panelTemperatute.Controls.Add(this.m_labelMin);
            this.panelTemperatute.Controls.Add(this.m_labelAverage);
            this.panelTemperatute.Controls.Add(this.labelMax);
            this.panelTemperatute.Controls.Add(this.labelMin);
            this.panelTemperatute.Controls.Add(this.pbxBlack);
            this.panelTemperatute.Controls.Add(this.pbxRed);
            this.panelTemperatute.Controls.Add(this.labelAverage);
            this.panelTemperatute.Location = new System.Drawing.Point(0, 245);
            this.panelTemperatute.Name = "panelTemperatute";
            this.panelTemperatute.Size = new System.Drawing.Size(500, 25);
            // 
            // pbxArrowBlackPT
            // 
            this.pbxArrowBlackPT.BackColor = System.Drawing.Color.White;
            this.pbxArrowBlackPT.Image = ((System.Drawing.Image)(resources.GetObject("pbxArrowBlackPT.Image")));
            this.pbxArrowBlackPT.Location = new System.Drawing.Point(480, 3);
            this.pbxArrowBlackPT.Name = "pbxArrowBlackPT";
            this.pbxArrowBlackPT.Size = new System.Drawing.Size(20, 20);
            this.pbxArrowBlackPT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // pbxArrowWhitePT
            // 
            this.pbxArrowWhitePT.BackColor = System.Drawing.Color.Transparent;
            this.pbxArrowWhitePT.Image = ((System.Drawing.Image)(resources.GetObject("pbxArrowWhitePT.Image")));
            this.pbxArrowWhitePT.Location = new System.Drawing.Point(480, 3);
            this.pbxArrowWhitePT.Name = "pbxArrowWhitePT";
            this.pbxArrowWhitePT.Size = new System.Drawing.Size(20, 20);
            this.pbxArrowWhitePT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxArrowWhitePT.Visible = false;
            // 
            // panelButton
            // 
            this.panelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButton.Controls.Add(this.labelCalibration);
            this.panelButton.Controls.Add(this.contextLabelStop);
            this.panelButton.Controls.Add(this.contextLabelColor);
            this.panelButton.Controls.Add(this.pbxArrowWhitePB);
            this.panelButton.Controls.Add(this.pbxArrowBlackPB);
            this.panelButton.Controls.Add(this.labelCalibrationButton);
            this.panelButton.Controls.Add(this.labelColor);
            this.panelButton.Controls.Add(this.labelStart);
            this.panelButton.Location = new System.Drawing.Point(0, 245);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(500, 25);
            this.panelButton.Visible = false;
            // 
            // labelCalibration
            // 
            this.labelCalibration.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelCalibration.Location = new System.Drawing.Point(228, 2);
            this.labelCalibration.Name = "labelCalibration";
            this.labelCalibration.Size = new System.Drawing.Size(88, 23);
            this.labelCalibration.Text = "Calibration";
            // 
            // pbxArrowWhitePB
            // 
            this.pbxArrowWhitePB.BackColor = System.Drawing.Color.Transparent;
            this.pbxArrowWhitePB.Image = ((System.Drawing.Image)(resources.GetObject("pbxArrowWhitePB.Image")));
            this.pbxArrowWhitePB.Location = new System.Drawing.Point(475, 3);
            this.pbxArrowWhitePB.Name = "pbxArrowWhitePB";
            this.pbxArrowWhitePB.Size = new System.Drawing.Size(20, 20);
            this.pbxArrowWhitePB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxArrowWhitePB.Visible = false;
            // 
            // pbxArrowBlackPB
            // 
            this.pbxArrowBlackPB.BackColor = System.Drawing.Color.White;
            this.pbxArrowBlackPB.Image = ((System.Drawing.Image)(resources.GetObject("pbxArrowBlackPB.Image")));
            this.pbxArrowBlackPB.Location = new System.Drawing.Point(475, 3);
            this.pbxArrowBlackPB.Name = "pbxArrowBlackPB";
            this.pbxArrowBlackPB.Size = new System.Drawing.Size(20, 20);
            this.pbxArrowBlackPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // labelCalibrationButton
            // 
            this.labelCalibrationButton.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Underline);
            this.labelCalibrationButton.Location = new System.Drawing.Point(184, 2);
            this.labelCalibrationButton.Name = "labelCalibrationButton";
            this.labelCalibrationButton.Size = new System.Drawing.Size(44, 23);
            this.labelCalibrationButton.Text = "0,1,2";
            // 
            // labelColor
            // 
            this.labelColor.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.labelColor.Location = new System.Drawing.Point(7, 2);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(135, 23);
            this.labelColor.Text = "6 Черно-белый";
            // 
            // labelStart
            // 
            this.labelStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStart.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Underline);
            this.labelStart.Location = new System.Drawing.Point(379, 2);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(40, 23);
            this.labelStart.Text = "Enter";
            // 
            // contextLabelStop
            // 
            this.contextLabelStop.LabelWidth = 91;
            this.contextLabelStop.Location = new System.Drawing.Point(379, 2);
            this.contextLabelStop.Name = "contextLabelStop";
            this.contextLabelStop.Number = "*";
            this.contextLabelStop.Size = new System.Drawing.Size(91, 23);
            this.contextLabelStop.TabIndex = 16;
            // 
            // contextLabelColor
            // 
            this.contextLabelColor.LabelWidth = 135;
            this.contextLabelColor.Location = new System.Drawing.Point(7, 2);
            this.contextLabelColor.Name = "contextLabelColor";
            this.contextLabelColor.Number = "6";
            this.contextLabelColor.Size = new System.Drawing.Size(135, 23);
            this.contextLabelColor.TabIndex = 14;
            // 
            // m_helpControl
            // 
            this.m_helpControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_helpControl.Location = new System.Drawing.Point(0, 0);
            this.m_helpControl.Name = "m_helpControl";
            this.m_helpControl.Size = new System.Drawing.Size(500, 270);
            this.m_helpControl.TabIndex = 7;
            this.m_helpControl.TabStop = false;
            this.m_helpControl.Visible = false;
            this.m_helpControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ctrl_KeyDown);
            // 
            // ThermalImagerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panelTemperatute);
            this.Controls.Add(this.panelButton);
            this.Controls.Add(this.pbxThermalmager);
            this.Controls.Add(this.m_helpControl);
            this.Name = "ThermalImagerPage";
            this.Size = new System.Drawing.Size(500, 270);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ctrl_KeyDown);
            this.panelTemperatute.ResumeLayout(false);
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxThermalmager;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label labelAverage;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.Label m_labelMin;
        private System.Windows.Forms.Label m_labelMax;
        private System.Windows.Forms.Label m_labelAverage;
        private System.Windows.Forms.Timer timer;
        private DC22.Shell.Help.HelpControl m_helpControl;
        private System.Windows.Forms.PictureBox pbxBlack;
        private System.Windows.Forms.PictureBox pbxRed;
        private System.Windows.Forms.Panel panelTemperatute;
        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.Label labelColor;
        private System.Windows.Forms.Label labelStart;
        private System.Windows.Forms.Label labelCalibrationButton;
        private System.Windows.Forms.PictureBox pbxArrowWhitePT;
        private System.Windows.Forms.PictureBox pbxArrowBlackPT;
        private System.Windows.Forms.PictureBox pbxArrowWhitePB;
        private System.Windows.Forms.PictureBox pbxArrowBlackPB;
        private DC22.Shell.Controls.Common.ContextLabel contextLabelColor;
        private DC22.Shell.Controls.Common.ContextLabel contextLabelStop;
        private System.Windows.Forms.Label labelCalibration;
    }
}
