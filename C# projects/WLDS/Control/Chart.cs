using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Steema.TeeChart.Drawing;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using Steema.TeeChart.Themes;
using Wlds.DataProcessing;
using System.Diagnostics;

#if !NET20
using System.Linq;
#endif


namespace Wlds
{
    public partial class Chart : UserControl
    {
        public static Color[] CHANNEL_BASE_COLORS = {  
                                                        Color.FromArgb(255, 204, 0, 0), 
                                                        Color.FromArgb(255, 0, 0, 204), 
                                                      
                                                        Color.FromArgb(25, 0, 255, 0),
                                                        Color.FromArgb(25, 255, 255, 0), 
                                                        Color.FromArgb(25, 255, 165, 0), 
                                                        Color.FromArgb(25, 0, 191, 255), 
                                                        Color.FromArgb(25, 255, 0, 255), 
                                                        Color.FromArgb(25, 255, 20, 147) 
                                                    };
        public static Color[] CHANNEL_TRANSPARENT_COLORS = {   
                                                            Color.FromArgb(255, 255, 153, 153), 
                                                            Color.FromArgb(255, 153, 153, 255), 
                                                           
                                                            Color.FromArgb(255, 0, 255, 0), 
                                                            Color.FromArgb(255, 255, 255, 0), 
                                                            Color.FromArgb(255, 255, 165, 0), 
                                                            Color.FromArgb(255, 0, 191, 255), 
                                                            Color.FromArgb(255, 255, 0, 255), 
                                                            Color.FromArgb(255, 255, 20, 147) 
                                                        };

        public event DelegateZoomButtonClickedEventHandler ZoomButtonClicked;
        public event DelegateCircleButtonClickedEventHandler CircleButtonClicked;
        public event DelegateCircleMeasurementChangedEventHandler CircleMeasurementChanged;

        // Temp variables
        double ms = Math.Round(1000 / 65538.00, 3);
        double decimalValue;
        double y;
        int i = 0;

        public Chart()
        {
            InitializeComponent();

            m_nPage.Controls.RemoveAt(0);
        }

        // Properties
        public Int16 Id { get; set; }
        public DATA_TYPE Type { get; set; }
        public string Header
        {
            get
            {
                return this.tChart.Header.Text;
            }
            set
            {
                this.tChart.Header.Text = value;
            }
        }

        private DateTime xval = DateTime.Now;

        protected virtual void CreateChart()
        {
            Utils.CalcFramesPerSecond = true;
            tChart.Dock = DockStyle.Fill;
            tChart.Panel.Gradient.Visible = false;
            tChart.Panel.Color = Color.Black;
        }

        protected virtual void InitializeChart()
        {
            Utils.CalcFramesPerSecond = true;
            tChart.Dock = DockStyle.Fill;
            tChart.Panel.Gradient.Visible = true;
            tChart.Aspect.View3D = false;

            tChart.Walls.Visible = false;
            tChart.Header.Visible = true;

            tChart.Legend.Visible = true;
            tChart.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Auto;

            tChart.Axes.Left.AxisPen.UseStyling = false;
            tChart.Axes.Bottom.AxisPen.UseStyling = false;

            tChart.Axes.Left.Grid.UseStyling = false;
            tChart.Axes.Bottom.Grid.UseStyling = false;

            tChart.Panel.Pen.UseStyling = false;
            tChart.Panel.Bevel.Inner = BevelStyles.None;
            tChart.Panel.Bevel.Outer = BevelStyles.None;

            tChart.Axes.Bottom.Automatic = true;
            tChart.Axes.Bottom.Labels.ValueFormat = "#0.00";
            tChart.Axes.Bottom.MinAxisIncrement = 0.000001;
            tChart.Axes.Bottom.Title.Caption = "Hz";
            tChart.Axes.Bottom.Title.Font.Color = Color.White;

            tChart.Axes.Bottom.Title.Caption = "Hz";
            tChart.Axes.Bottom.Title.Font.Color = Color.White;

            tChart.Axes.Left.Title.Caption = (this.Id == 1) ? "G" : "dB";
            tChart.Axes.Left.Title.Font.Color = Color.White;
        }

        #region Data

            public void SetChartChannel(Detector detector, DATA_TYPE type)
            {
                DetectorPanel pDetector = new DetectorPanel();
                pDetector.DetectorId = detector.Id;
                pDetector.Type = type;
                pDetector.Title += " " + detector.Number;
                pDetector.Top = pDetector.Height * detector.Id;
                pDetector.Top += 10;

                setChannelFields(detector, type, pDetector);

                this.splitContainer.Panel2.Controls.Add(pDetector);
            }

            public void SetChartLine(Detector detector, DATA_TYPE type, float[] bufData)
            {
                if ((bufData == null) || (bufData.Length == 0))
                {
                    return;
                }
                try
                {
                    FastLine line = new FastLine();
                    int dataLength = bufData.Length;

                    tChart.AutoRepaint = false;

                    line.LinePen.UseStyling = true;
                    line.Legend.Visible = true;
                    line.Legend.AutoSize = false;

                    line.Legend.Text = "Детектор " + detector.Number;
                    line.Tag = detector.Id;

                    switch (type)
                    {
                        case DATA_TYPE.BASE:
                            for (i = 0; i < dataLength; i++)
                            {
                                decimalValue = System.Convert.ToDouble(bufData[i]);
                                y = Math.Round(decimalValue, 4);
                                line.Add(i, y);
                            }
                            line.Color = getChannelColor(detector.Id, DATA_TYPE.BASE);
                            break;
                        case DATA_TYPE.FFT:
                        case DATA_TYPE.FFT_S:
                            float kf = MainForm.SamplingFrequency[detector.Info.D] / (float)Math.Pow(2.0, detector.Order); 
                            for (i = 0; i < dataLength; i++)
                            {
                                decimalValue = System.Convert.ToDouble(bufData[i]);
                                y = Math.Round(decimalValue, 4);
                                line.Add(i * kf, y);
                            }

                            line.Color = getChannelColor(detector.Id, DATA_TYPE.FFT);
                            break;
                    }

                    line.Legend.Color = line.Color;
                    if (MainForm.mode == MODE.BASE)
                    {
                        tChart.Series.Add(line);
                    }
                    else
                    {
                        if (tChart.Series.Count < (MainForm.detectors.Count + Circle.detectorIndexes.Count()))
                        {
                            tChart.Series.Add(line);
                        }
                        else
                        {
                            tChart.Series.RemoveAt(MainForm.detectors.Count);
                            tChart.Series.Add(line);
                            SetCircleMeasurement();
                        }
                        tChart.Refresh();
                    }

                    tChart.AutoRepaint = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error by set data to chart. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            public void ClearData()
            {
                tChart.Zoom.Undo();
                tChart.Series.RemoveAllSeries();

                this.splitContainer.Panel2.Controls.Clear();
            }

            public void ZoomUndo()
            {
                tChart.Zoom.Undo();
            }

            private void setChannelFields(Detector detector, DATA_TYPE type, DetectorPanel pDetector)
            {
                // Display and set text fields with result statistics
                pDetector.SetDetectorFields(detector.data[type], detector);
            }

            private void scrollPagerChange()
            {
                tChart.AutoRepaint = false;
                tChart.Legend.Visible = true;
                tChart.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series;
                tChart.Legend.CheckBoxes = true;
                tChart.Legend.Alignment = Steema.TeeChart.LegendAlignments.Right;
                tChart.Legend.DividingLines.Visible = false;

                tChart.AutoRepaint = true;
                tChart.Invalidate();
            }

            private Color getChannelColor(Int16 channelId, DATA_TYPE type)
            {
                if (CHANNEL_BASE_COLORS.Length <= channelId)
                {
                    // Random color
                    Random rnd = new Random();

                    Color[] tempColors = new Color[CHANNEL_BASE_COLORS.Length + 1];
                    CHANNEL_BASE_COLORS.CopyTo(tempColors, 0);
                    CHANNEL_BASE_COLORS = new Color[tempColors.Length];
                    tempColors[CHANNEL_BASE_COLORS.Length - 1] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    tempColors.CopyTo(CHANNEL_BASE_COLORS, 0);

                    CHANNEL_TRANSPARENT_COLORS.CopyTo(tempColors, 0);
                    CHANNEL_TRANSPARENT_COLORS = new Color[tempColors.Length];
                    tempColors[CHANNEL_TRANSPARENT_COLORS.Length - 1] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    tempColors.CopyTo(CHANNEL_TRANSPARENT_COLORS, 0);
                }

                return ((type == DATA_TYPE.BASE) ? CHANNEL_BASE_COLORS[channelId] : CHANNEL_TRANSPARENT_COLORS[channelId]);
            }

        #endregion

        #region Events

            private void chart_Load(object sender, EventArgs e)
            {
                InitializeChart();
            }

            private void chart_Resize(object sender, EventArgs e)
            {
                scrollPagerChange();
            }

            private void m_btnZoom_Click(object sender, EventArgs e)
            {
                if (((Button)sender).Text == "+")
                {
                    ((Button)sender).Text = "-";
                }
                else
                {
                    ((Button)sender).Text = "+";
                }

                // Resize chart
                if (ZoomButtonClicked != null)
                {
                    ZoomButtonClicked(this, e);
                }
            }

            private void Chart_MouseClick(object sender, MouseEventArgs e)
            {
                if (this.tChart.Series.Count > 0)
                {
                    tChart.Tools.Clear();
                    for (int i = 0; i < tChart.Series.Count; i++)
                    {
                        if (tChart.Series[i].Active)
                        {
                            int index = this.tChart.Series[i].Clicked(e.X, e.Y);
                            if (index != -1)
                            {
                                Steema.TeeChart.Tools.Annotation annotation = new Steema.TeeChart.Tools.Annotation(tChart.Chart);
                                annotation.Text = Math.Round(this.tChart.Series[i].YValues.Value[index], 2).ToString();
                                annotation.Shape.CustomPosition = true;
                                annotation.Shape.Left = e.X + 15;
                                annotation.Shape.Top = e.Y + i * 20;
                            }
                        }
                    }
                }
            }

        #endregion

        #region Circle

            public void SetDetectorIndexList()
            {
                if (MainForm.mode == MODE.CIRCLE)
                {
                    int lineActivateCount = 0;
                    for (int i = 0; i < tChart.Series.Count; i++)
                    {
                        if (tChart.Series[i].Active)
                        {
                            lineActivateCount++;
                        }
                    }
                    Circle.detectorIndexes = new int[lineActivateCount];
                    for (int i = 0, j = 0; i < tChart.Series.Count; i++)
                    {
                        if (tChart.Series[i].Active)
                        {
                            Circle.detectorIndexes[j] = Int32.Parse(tChart.Series[i].Tag.ToString());
                            j++;
                        }
                    }
                }
            }
 
            public void SetCircleMeasurement()
            {
                foreach (Control control in this.splitContainer.Panel2.Controls)
                {
                    if (control.Name.Equals("DetectorPanel"))
                    {
                        DetectorPanel detectorPanel = control as DetectorPanel;
                        if (Circle.detectorIndexes.Contains(detectorPanel.DetectorId)) 
                        {
                            detectorPanel.SelectCircleMeasurement(Circle.measurementNumber * 2);
                        }
                    }
                }
                this.m_nPage.Value = Circle.measurementNumber + 1;
            }

            public void ClearCircle()
            {
                if (tChart.Series.Count > MainForm.detectors.Count)
                {
                    for (int i = 0; i < Circle.detectorIndexes.Count(); i++)
                    {
                        tChart.Series.RemoveAt(tChart.Series.Count - 1);
                    }
                }
                tChart.Series.AllActive = true;
                tChart.Invalidate();
                this.m_nPage.Value = 1;
            }

            public void HandleModeCircle()
            {
                m_btnCircle.Text = (MainForm.mode == MODE.BASE) ? "Цикл" : "Сброс";
                enableButtons(false);
                m_nPage.Maximum = Circle.measurementNumberCount;
                pPagination.Visible = (MainForm.mode != MODE.BASE);
                if (MainForm.mode == MODE.CIRCLE && !m_btnCircle.Visible)
                {
                    m_btnCircle.Visible = true;
                }

                for (int i = 0; i < tChart.Series.Count; i++)
                {
                    if (tChart.Series[i].Active)
                    {
                        tChart.Series[i].Active = false;
                    }
                }
            }

            public void HandleModePause()
            {
                m_btnPause.Text = (MainForm.mode == MODE.CIRCLE) ? "  |>" : " ||";
                enableButtons((MainForm.mode == MODE.CIRCLE) ? false : true);
            }

            private void m_btnCicle_Click(object sender, EventArgs e)
            {
                // Measurement circle display
                MainForm.mode = (MainForm.mode == MODE.BASE) ? MODE.CIRCLE : MODE.BASE;
                CircleButtonClicked(this, e);
            }

            private void m_btnPause_Click(object sender, EventArgs e)
            {
                MainForm.mode = (MainForm.mode == MODE.CIRCLE) ? MODE.PAUSE : MODE.CIRCLE;
                CircleMeasurementChanged((Control)sender, e);
            }

            private void m_btnCircleMeasurement_Click(object sender, EventArgs e)
            {
                CircleMeasurementChanged((Control)sender, e);
            }

            private void m_nPage_KeyUp(object sender, KeyEventArgs e)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        if (Circle.measurementNumber != (int)m_nPage.Value)
                        {
                            CircleMeasurementChanged(sender, e);
                        }
                        break;
                }
            }

            private void m_nPage_Changed(object sender, EventArgs e)
            {
                if (Circle.measurementNumber != (int)this.m_nPage.Value)
                {
                    CircleMeasurementChanged(sender, e);
                }
            }
        
            private void enableButtons(Boolean isEnabled)
            {
                this.m_btnFirst.Enabled = isEnabled;
                this.m_btnEnd.Enabled = isEnabled;
                this.m_btnNext.Enabled = isEnabled;
                this.m_btnPrev.Enabled = isEnabled;
                this.m_nPage.Enabled = isEnabled;
            }
 
        #endregion
    }
}
 