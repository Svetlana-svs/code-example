namespace SM3G
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tolstrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.m_btnThreshold = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.m_btnUpload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_btnOpen = new System.Windows.Forms.ToolStripButton();
            this.m_btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_btnMeasureRoute = new System.Windows.Forms.ToolStripButton();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelZoom = new System.Windows.Forms.TableLayoutPanel();
            this.m_tchartEnvelopeH = new SM3G.Chart();
            this.m_tchartEnvelopeL = new SM3G.Chart();
            this.tolstrip.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tolstrip
            // 
            this.tolstrip.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.tolstrip.AutoSize = false;
            this.tolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton,
            this.toolStripSeparator3,
            this.m_btnUpload,
            this.toolStripSeparator2,
            this.m_btnOpen,
            this.m_btnSave,
            this.toolStripSeparator1,
            this.m_btnMeasureRoute});
            this.tolstrip.Location = new System.Drawing.Point(0, 0);
            this.tolstrip.Name = "tolstrip";
            this.tolstrip.Size = new System.Drawing.Size(1274, 35);
            this.tolstrip.TabIndex = 9;
            this.tolstrip.Text = "Измерить по маршруту";
            // 
            // toolStripDropDownButton
            // 
            this.toolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_btnThreshold});
            this.toolStripDropDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.toolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton.Image")));
            this.toolStripDropDownButton.ImageTransparentColor = System.Drawing.SystemColors.Control;
            this.toolStripDropDownButton.Name = "toolStripDropDownButton";
            this.toolStripDropDownButton.Size = new System.Drawing.Size(104, 32);
            this.toolStripDropDownButton.Text = "Настройки";
            // 
            // m_btnThreshold
            // 
            this.m_btnThreshold.Name = "m_btnThreshold";
            this.m_btnThreshold.Size = new System.Drawing.Size(219, 24);
            this.m_btnThreshold.Text = "Пороги измерений";
            this.m_btnThreshold.Click += new System.EventHandler(this.m_btnThreshold_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 35);
            // 
            // m_btnUpload
            // 
            this.m_btnUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_btnUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.m_btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("m_btnUpload.Image")));
            this.m_btnUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnUpload.Name = "m_btnUpload";
            this.m_btnUpload.Size = new System.Drawing.Size(77, 32);
            this.m_btnUpload.Text = "Скачать";
            this.m_btnUpload.Click += new System.EventHandler(this.m_btnUpload_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
            // 
            // m_btnOpen
            // 
            this.m_btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_btnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.m_btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("m_btnOpen.Image")));
            this.m_btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnOpen.Name = "m_btnOpen";
            this.m_btnOpen.Size = new System.Drawing.Size(80, 32);
            this.m_btnOpen.Text = "Открыть";
            this.m_btnOpen.Click += new System.EventHandler(this.m_btnOpen_Click);
            // 
            // m_btnSave
            // 
            this.m_btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.m_btnSave.Image = ((System.Drawing.Image)(resources.GetObject("m_btnSave.Image")));
            this.m_btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnSave.Name = "m_btnSave";
            this.m_btnSave.Size = new System.Drawing.Size(94, 32);
            this.m_btnSave.Text = "Сохранить";
            this.m_btnSave.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
            // 
            // m_btnMeasureRoute
            // 
            this.m_btnMeasureRoute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_btnMeasureRoute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.m_btnMeasureRoute.Image = ((System.Drawing.Image)(resources.GetObject("m_btnMeasureRoute.Image")));
            this.m_btnMeasureRoute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnMeasureRoute.Name = "m_btnMeasureRoute";
            this.m_btnMeasureRoute.Size = new System.Drawing.Size(188, 32);
            this.m_btnMeasureRoute.Text = "Измерить по маршруту";
            this.m_btnMeasureRoute.Visible = false;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.splitContainer);
            this.groupBox.Controls.Add(this.panelZoom);
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(1274, 660);
            this.groupBox.TabIndex = 11;
            this.groupBox.TabStop = false;
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(3, 16);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Panel1.Controls.Add(this.m_tchartEnvelopeH);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.m_tchartEnvelopeL);
            this.splitContainer.Size = new System.Drawing.Size(1268, 641);
            this.splitContainer.SplitterDistance = 320;
            this.splitContainer.TabIndex = 0;
            // 
            // panelZoom
            // 
            this.panelZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelZoom.BackColor = System.Drawing.SystemColors.Control;
            this.panelZoom.ColumnCount = 1;
            this.panelZoom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelZoom.Location = new System.Drawing.Point(3, 9);
            this.panelZoom.Name = "panelZoom";
            this.panelZoom.RowCount = 2;
            this.panelZoom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.960784F));
            this.panelZoom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98.03922F));
            this.panelZoom.Size = new System.Drawing.Size(1274, 648);
            this.panelZoom.TabIndex = 3;
            this.panelZoom.Visible = false;
            // 
            // m_tchartEnvelopeH
            // 
            this.m_tchartEnvelopeH.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.m_tchartEnvelopeH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tchartEnvelopeH.Header = "Полоса 10 - 5000 Гц, m/s^2";
            this.m_tchartEnvelopeH.Id = ((short)(1));
            this.m_tchartEnvelopeH.Location = new System.Drawing.Point(0, 10);
            this.m_tchartEnvelopeH.Margin = new System.Windows.Forms.Padding(0);
            this.m_tchartEnvelopeH.Name = "m_tchartEnvelopeH";
            this.m_tchartEnvelopeH.Size = new System.Drawing.Size(1268, 310);
            this.m_tchartEnvelopeH.TabIndex = 0;
            this.m_tchartEnvelopeH.Title = "";
            this.m_tchartEnvelopeH.Type = SM3G.DATA_TYPE.BASE;
            // 
            // m_tchartEnvelopeL
            // 
            this.m_tchartEnvelopeL.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.m_tchartEnvelopeL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tchartEnvelopeL.Header = "Температура, °C";
            this.m_tchartEnvelopeL.Id = ((short)(2));
            this.m_tchartEnvelopeL.Location = new System.Drawing.Point(0, 0);
            this.m_tchartEnvelopeL.Margin = new System.Windows.Forms.Padding(0);
            this.m_tchartEnvelopeL.Name = "m_tchartEnvelopeL";
            this.m_tchartEnvelopeL.Size = new System.Drawing.Size(1268, 317);
            this.m_tchartEnvelopeL.TabIndex = 0;
            this.m_tchartEnvelopeL.Title = "";
            this.m_tchartEnvelopeL.Type = SM3G.DATA_TYPE.T;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1274, 662);
            this.Controls.Add(this.tolstrip);
            this.Controls.Add(this.groupBox);
            this.Name = "MainForm";
            this.Text = "SM3G";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.form_FormClosed);
            this.tolstrip.ResumeLayout(false);
            this.tolstrip.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip tolstrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem m_btnThreshold;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton m_btnOpen;
        private System.Windows.Forms.ToolStripButton m_btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton m_btnUpload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton m_btnMeasureRoute;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Chart m_tchartEnvelopeH;
        private Chart m_tchartEnvelopeL;
        private System.Windows.Forms.TableLayoutPanel panelZoom;
    }
}