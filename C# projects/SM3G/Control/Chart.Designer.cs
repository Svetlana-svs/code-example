namespace SM3G
{
    partial class Chart
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
        this.m_btnZoom = new System.Windows.Forms.Button();
        this.tChart = new Steema.TeeChart.TChart();
        this.m_pChart = new System.Windows.Forms.Panel();
        this.splitContainer = new System.Windows.Forms.SplitContainer();
        this.pDiagnostic = new System.Windows.Forms.Panel();
        this.m_txbDiagnostic = new System.Windows.Forms.TextBox();
        this.pPagination = new System.Windows.Forms.Panel();
        this.m_btnFirst = new System.Windows.Forms.Button();
        this.m_btnEnd = new System.Windows.Forms.Button();
        this.m_btnPrev = new System.Windows.Forms.Button();
        this.m_btnNext = new System.Windows.Forms.Button();
        this.m_nPage = new System.Windows.Forms.NumericUpDown();
        this.m_pChart.SuspendLayout();
        this.splitContainer.Panel1.SuspendLayout();
        this.splitContainer.SuspendLayout();
        this.pDiagnostic.SuspendLayout();
        this.pPagination.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.m_nPage)).BeginInit();
        this.SuspendLayout();
        // 
        // m_btnZoom
        // 
        this.m_btnZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.m_btnZoom.Location = new System.Drawing.Point(654, 268);
        this.m_btnZoom.Margin = new System.Windows.Forms.Padding(0);
        this.m_btnZoom.Name = "m_btnZoom";
        this.m_btnZoom.Size = new System.Drawing.Size(24, 22);
        this.m_btnZoom.TabIndex = 6;
        this.m_btnZoom.Text = "+";
        this.m_btnZoom.UseVisualStyleBackColor = true;
        this.m_btnZoom.Click += new System.EventHandler(this.m_btnZoom_Click);
        // 
        // tChart
        // 
        // 
        // 
        // 
        this.tChart.Aspect.View3D = false;
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.Bottom.Automatic = false;
        this.tChart.Axes.Bottom.AutomaticMaximum = false;
        this.tChart.Axes.Bottom.AutomaticMinimum = false;
        // 
        // 
        // 
        this.tChart.Axes.Bottom.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.Bottom.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        this.tChart.Axes.Bottom.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
        // 
        // 
        // 
        this.tChart.Axes.Bottom.Labels.Alternate = true;
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.Bottom.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        this.tChart.Axes.Bottom.Labels.MultiLine = true;
        this.tChart.Axes.Bottom.Maximum = 0;
        this.tChart.Axes.Bottom.Minimum = 0;
        // 
        // 
        // 
        this.tChart.Axes.Bottom.MinorTicks.Visible = false;
        // 
        // 
        // 
        this.tChart.Axes.Bottom.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.Bottom.TicksInner.Visible = true;
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.Depth.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.Depth.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        this.tChart.Axes.Depth.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.Depth.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        this.tChart.Axes.Depth.MinorTicks.Visible = false;
        // 
        // 
        // 
        this.tChart.Axes.Depth.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.Depth.TicksInner.Visible = true;
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.DepthTop.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.DepthTop.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        this.tChart.Axes.DepthTop.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.DepthTop.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        this.tChart.Axes.DepthTop.MinorTicks.Visible = false;
        // 
        // 
        // 
        this.tChart.Axes.DepthTop.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.DepthTop.TicksInner.Visible = true;
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.Left.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.Left.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        this.tChart.Axes.Left.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.Left.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        this.tChart.Axes.Left.MinorTicks.Visible = false;
        // 
        // 
        // 
        this.tChart.Axes.Left.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.Left.TicksInner.Visible = true;
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.Right.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.Right.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        this.tChart.Axes.Right.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.Right.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        this.tChart.Axes.Right.MinorTicks.Visible = false;
        // 
        // 
        // 
        this.tChart.Axes.Right.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.Right.TicksInner.Visible = true;
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.Top.AxisPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.Top.Grid.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        this.tChart.Axes.Top.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Axes.Top.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        this.tChart.Axes.Top.MinorTicks.Visible = false;
        // 
        // 
        // 
        this.tChart.Axes.Top.Ticks.Color = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        // 
        // 
        // 
        this.tChart.Axes.Top.TicksInner.Visible = true;
        this.tChart.Dock = System.Windows.Forms.DockStyle.Fill;
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Header.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
        this.tChart.Header.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.tChart.Header.Brush.Gradient.SigmaFocus = 0F;
        this.tChart.Header.Brush.Gradient.SigmaScale = 0F;
        this.tChart.Header.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Header.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        this.tChart.Legend.Alignment = Steema.TeeChart.LegendAlignments.Top;
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Legend.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
        this.tChart.Legend.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.tChart.Legend.Brush.Gradient.SigmaFocus = 0F;
        this.tChart.Legend.Brush.Gradient.SigmaScale = 0F;
        this.tChart.Legend.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
        this.tChart.Legend.Brush.Gradient.Visible = true;
        // 
        // 
        // 
        this.tChart.Legend.DividingLines.Visible = true;
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        this.tChart.Legend.Font.Shadow.Height = 4;
        this.tChart.Legend.Font.Shadow.Visible = true;
        this.tChart.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Palette;
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Legend.Shadow.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        this.tChart.Legend.Title.Visible = true;
        this.tChart.Legend.VertMargin = 50;
        this.tChart.Location = new System.Drawing.Point(0, 0);
        this.tChart.Margin = new System.Windows.Forms.Padding(0);
        this.tChart.Name = "tChart";
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Panel.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
        this.tChart.Panel.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.tChart.Panel.Brush.Gradient.SigmaFocus = 0F;
        this.tChart.Panel.Brush.Gradient.SigmaScale = 0F;
        this.tChart.Panel.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
        this.tChart.Panel.Brush.Gradient.UseMiddle = false;
        this.tChart.Size = new System.Drawing.Size(690, 300);
        this.tChart.TabIndex = 6;
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Walls.Back.Brush.Color = System.Drawing.Color.White;
        // 
        // 
        // 
        this.tChart.Walls.Back.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
        this.tChart.Walls.Back.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.tChart.Walls.Back.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
        // 
        // 
        // 
        this.tChart.Walls.Back.Pen.Visible = false;
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Walls.Bottom.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
        this.tChart.Walls.Bottom.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.tChart.Walls.Bottom.Brush.Gradient.SigmaFocus = 0F;
        this.tChart.Walls.Bottom.Brush.Gradient.SigmaScale = 0F;
        this.tChart.Walls.Bottom.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        this.tChart.Walls.Bottom.Pen.Visible = false;
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Walls.Left.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
        this.tChart.Walls.Left.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.tChart.Walls.Left.Brush.Gradient.SigmaFocus = 0F;
        this.tChart.Walls.Left.Brush.Gradient.SigmaScale = 0F;
        this.tChart.Walls.Left.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        this.tChart.Walls.Left.Pen.Visible = false;
        // 
        // 
        // 
        // 
        // 
        // 
        this.tChart.Walls.Right.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
        // 
        // 
        // 
        this.tChart.Walls.Right.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
        this.tChart.Walls.Right.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.tChart.Walls.Right.Brush.Gradient.SigmaFocus = 0F;
        this.tChart.Walls.Right.Brush.Gradient.SigmaScale = 0F;
        this.tChart.Walls.Right.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        // 
        // 
        // 
        this.tChart.Walls.Right.Pen.Visible = false;
        // 
        // m_pChart
        // 
        this.m_pChart.Controls.Add(this.splitContainer);
        this.m_pChart.Dock = System.Windows.Forms.DockStyle.Fill;
        this.m_pChart.Location = new System.Drawing.Point(0, 0);
        this.m_pChart.Name = "m_pChart";
        this.m_pChart.Size = new System.Drawing.Size(838, 300);
        this.m_pChart.TabIndex = 7;
        // 
        // splitContainer
        // 
        this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitContainer.Location = new System.Drawing.Point(0, 0);
        this.splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        this.splitContainer.Panel1.Controls.Add(this.pDiagnostic);
        this.splitContainer.Panel1.Controls.Add(this.pPagination);
        this.splitContainer.Panel1.Controls.Add(this.m_btnZoom);
        this.splitContainer.Panel1.Controls.Add(this.tChart);
        // 
        // splitContainer.Panel2
        // 
        this.splitContainer.Panel2.AutoScroll = true;
        this.splitContainer.Panel2.Resize += new System.EventHandler(this.chart_Resize);
        this.splitContainer.Size = new System.Drawing.Size(838, 300);
        this.splitContainer.SplitterDistance = 690;
        this.splitContainer.SplitterWidth = 8;
        this.splitContainer.TabIndex = 7;
        // 
        // pDiagnostic
        // 
        this.pDiagnostic.AllowDrop = true;
        this.pDiagnostic.BackColor = System.Drawing.SystemColors.WindowFrame;
        this.pDiagnostic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        this.pDiagnostic.Controls.Add(this.m_txbDiagnostic);
        this.pDiagnostic.Location = new System.Drawing.Point(549, 71);
        this.pDiagnostic.Name = "pDiagnostic";
        this.pDiagnostic.Size = new System.Drawing.Size(200, 100);
        this.pDiagnostic.TabIndex = 11;
        this.pDiagnostic.Visible = false;
        // 
        // m_txbDiagnostic
        // 
        this.m_txbDiagnostic.BackColor = System.Drawing.SystemColors.WindowFrame;
        this.m_txbDiagnostic.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.m_txbDiagnostic.Dock = System.Windows.Forms.DockStyle.Fill;
        this.m_txbDiagnostic.ForeColor = System.Drawing.SystemColors.Window;
        this.m_txbDiagnostic.Location = new System.Drawing.Point(0, 0);
        this.m_txbDiagnostic.Name = "m_txbDiagnostic";
        this.m_txbDiagnostic.Size = new System.Drawing.Size(196, 13);
        this.m_txbDiagnostic.TabIndex = 10;
        // 
        // pPagination
        // 
        this.pPagination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.pPagination.BackColor = System.Drawing.Color.Transparent;
        this.pPagination.Controls.Add(this.m_btnFirst);
        this.pPagination.Controls.Add(this.m_btnEnd);
        this.pPagination.Controls.Add(this.m_btnPrev);
        this.pPagination.Controls.Add(this.m_btnNext);
        this.pPagination.Controls.Add(this.m_nPage);
        this.pPagination.Location = new System.Drawing.Point(547, 234);
        this.pPagination.Name = "pPagination";
        this.pPagination.Size = new System.Drawing.Size(131, 23);
        this.pPagination.TabIndex = 8;
        this.pPagination.Visible = false;
        // 
        // m_btnFirst
        // 
        this.m_btnFirst.Location = new System.Drawing.Point(0, 0);
        this.m_btnFirst.Name = "m_btnFirst";
        this.m_btnFirst.Size = new System.Drawing.Size(30, 23);
        this.m_btnFirst.TabIndex = 0;
        this.m_btnFirst.Tag = "first";
        this.m_btnFirst.Text = "<<";
        this.m_btnFirst.UseVisualStyleBackColor = true;
        // 
        // m_btnEnd
        // 
        this.m_btnEnd.Location = new System.Drawing.Point(103, 0);
        this.m_btnEnd.Name = "m_btnEnd";
        this.m_btnEnd.Size = new System.Drawing.Size(30, 23);
        this.m_btnEnd.TabIndex = 4;
        this.m_btnEnd.Tag = "end";
        this.m_btnEnd.Text = ">>";
        this.m_btnEnd.UseVisualStyleBackColor = true;
        // 
        // m_btnPrev
        // 
        this.m_btnPrev.Location = new System.Drawing.Point(30, 0);
        this.m_btnPrev.Name = "m_btnPrev";
        this.m_btnPrev.Size = new System.Drawing.Size(23, 23);
        this.m_btnPrev.TabIndex = 1;
        this.m_btnPrev.Tag = "prev";
        this.m_btnPrev.Text = "<";
        this.m_btnPrev.UseVisualStyleBackColor = true;
        // 
        // m_btnNext
        // 
        this.m_btnNext.Location = new System.Drawing.Point(80, 0);
        this.m_btnNext.Name = "m_btnNext";
        this.m_btnNext.Size = new System.Drawing.Size(23, 23);
        this.m_btnNext.TabIndex = 3;
        this.m_btnNext.Tag = "next";
        this.m_btnNext.Text = ">";
        this.m_btnNext.UseVisualStyleBackColor = true;
        // 
        // m_nPage
        // 
        this.m_nPage.Location = new System.Drawing.Point(55, 2);
        this.m_nPage.Name = "m_nPage";
        this.m_nPage.Size = new System.Drawing.Size(42, 20);
        this.m_nPage.TabIndex = 2;
        this.m_nPage.Tag = "page";
        this.m_nPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        this.m_nPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
        // 
        // Chart
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.Controls.Add(this.m_pChart);
        this.Margin = new System.Windows.Forms.Padding(0);
        this.Name = "Chart";
        this.Size = new System.Drawing.Size(838, 300);
        this.Load += new System.EventHandler(this.chart_Load);
        this.m_pChart.ResumeLayout(false);
        this.splitContainer.Panel1.ResumeLayout(false);
        this.splitContainer.ResumeLayout(false);
        this.pDiagnostic.ResumeLayout(false);
        this.pDiagnostic.PerformLayout();
        this.pPagination.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.m_nPage)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button m_btnZoom;
    public Steema.TeeChart.TChart tChart;
    private System.Windows.Forms.Panel m_pChart;
    private System.Windows.Forms.SplitContainer splitContainer;
    private System.Windows.Forms.Button m_btnNext;
    private System.Windows.Forms.Panel pPagination;
    private System.Windows.Forms.Button m_btnFirst;
    private System.Windows.Forms.Button m_btnEnd;
    private System.Windows.Forms.Button m_btnPrev;
    private System.Windows.Forms.NumericUpDown m_nPage;
    private System.Windows.Forms.TextBox m_txbDiagnostic;
    private System.Windows.Forms.Panel pDiagnostic;
  }
}
