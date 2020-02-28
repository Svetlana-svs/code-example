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
#if !NET20
using System.Linq;
#endif


namespace SM3G
{
    public partial class Chart : UserControl
    {
        public static Color[] LINE_BASE_COLORS = {  
                                                        Color.FromArgb(255, 153, 153, 255), 
                                                           
                                                        Color.FromArgb(255, 0, 255, 0), 
                                                        Color.FromArgb(255, 255, 255, 0), 
                                                        Color.FromArgb(255, 255, 165, 0), 
                                                        Color.FromArgb(255, 0, 191, 255), 
                                                        Color.FromArgb(255, 255, 0, 255), 
                                                        Color.FromArgb(255, 255, 20, 147), 
                                                        Color.FromArgb(255, 255, 153, 153) 
                                                    };
        // Additional colors not using in the current project
        public static Color[] LINE_TRANSPARENT_COLORS = {   
                                                            Color.FromArgb(255, 0, 0, 204), 
                                                          
                                                            Color.FromArgb(25, 0, 255, 0),
                                                            Color.FromArgb(25, 255, 255, 0), 
                                                            Color.FromArgb(25, 255, 165, 0), 
                                                            Color.FromArgb(25, 0, 191, 255), 
                                                            Color.FromArgb(25, 255, 0, 255), 
                                                            Color.FromArgb(25, 255, 20, 147), 
                                                            Color.FromArgb(255, 204, 0, 0) 
                                                    };
        public static Color[] LINE_THRESHOLD_COLORS = {  
                                                            Color.Blue, 
                                                            Color.Green,
                                                            Color.Yellow, 
                                                            Color.Red
                                                    };

        public event DelegateZoomButtonClickedEventHandler ZoomButtonClicked;

        public Chart()
        {
            InitializeComponent();

            m_nPage.Controls.RemoveAt(0);
       }

        // Properties
        public Int16 Id { get; set; }
        public DATA_TYPE Type { get; set; }
        public String Title { get; set; }
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
            tChart.Axes.Bottom.Title.Caption = this.Title;
            tChart.Axes.Bottom.Title.Font.Color = Color.White;
            tChart.Axes.Bottom.Labels.DateTimeFormat = "dd.MM HH:MM";
        }

        #region Data

        public void SetChartDetector(Detector detector, DATA_TYPE type)
            {
                DetectorPanel pDetector = new DetectorPanel();
                pDetector.DetectorId = detector.Id;
                pDetector.Type = type;
                pDetector.Title += " " + detector.Name;
                pDetector.Top = pDetector.Height * detector.Id;
                pDetector.Top += 10;

                setDetectorFields(detector, type, pDetector);

                this.splitContainer.Panel2.Controls.Add(pDetector);
            }

            public void SetChartLineThreshold(float threshold, int num)
            {
                HorizLine line = new Steema.TeeChart.Styles.HorizLine();
                line.LinePen.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                line.Color = LINE_THRESHOLD_COLORS[num];
                line.LinePen.Width = 2;
                line.Add(MainForm.dateTimeRangeMeasure.start, threshold);
                line.Add(MainForm.dateTimeRangeMeasure.end, threshold);

                tChart.Series.Add(line);
                tChart.Series[tChart.Series.Count - 1].ShowInLegend = false;
            }

            public void SetChartLine(Detector detector, DATA_TYPE type)
            {
                if ((detector.DataMeasurement == null) || (detector.DataMeasurement.Count == 0))
                {
                    return;
                }
                try
                {
                    Line line = new Line();
                    int dataLength = detector.DataMeasurement.Count;
                    
                    tChart.AutoRepaint = false;
                    line.Legend.Visible = true;
                    line.Legend.AutoSize = false;
                    line.Marks.Visible = false;
                    line.Pointer.HorizSize = 3;
                    line.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
                    line.Pointer.VertSize = 3;
                    line.Pointer.Visible = true;
                    line.MouseEnter += new System.EventHandler(tChart_MouseEnter);

                    line.Legend.Text = "Детектор " + detector.Name;
                                       
                    switch (type)
                    {
                        case DATA_TYPE.BASE:
                            foreach(Measure measure in detector.DataMeasurement)
                            {
                                line.Add(measure.Time, measure.A);
                            }

                            line.Color = getLineColor(detector.Id, DATA_TYPE.BASE);
                            break;
                        case DATA_TYPE.T:
                            foreach(Measure measure in detector.DataMeasurement)
                            {
                                line.Add(measure.Time, measure.T);
                            }
                            line.Color = getLineColor(detector.Id, DATA_TYPE.BASE);
                            break;
                    }

                    line.Legend.Color = line.Color;
                    line.Tag = type;
                    tChart.Series.Add(line);

                    tChart.AutoRepaint = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error by set data to chart. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            public void SetChatLineIncrement()
            {
                double totalDays = (MainForm.dateTimeRangeMeasure.end - MainForm.dateTimeRangeMeasure.start).TotalDays;
                double increment = Steema.TeeChart.Utils.GetDateTimeStep(Steema.TeeChart.DateTimeSteps.OneDay);
                if (totalDays <= 1)
                {
                    // 1 hours
                    increment = Steema.TeeChart.Utils.GetDateTimeStep(Steema.TeeChart.DateTimeSteps.OneHour) * 2; 

                } else if ((1 < totalDays) && (totalDays <= 4))
                {
                    // 3 hours
                    increment = Steema.TeeChart.Utils.GetDateTimeStep(Steema.TeeChart.DateTimeSteps.SixHours); 
                }
                else if ((4 < totalDays) && (totalDays < 5))
                {
                    // 6 hours
                    increment = Steema.TeeChart.Utils.GetDateTimeStep(Steema.TeeChart.DateTimeSteps.SixHours) * 2; 
                }
                else
                {   
                    // 12 hours
                    increment = Steema.TeeChart.Utils.GetDateTimeStep(Steema.TeeChart.DateTimeSteps.SixHours) * 4; 
                }
                
                tChart.Axes.Bottom.Increment = increment;
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

            private void setDetectorFields(Detector detector, DATA_TYPE type, DetectorPanel pDetector)
            {
                // Display and set descriptions results
                pDetector.SetDetectorFields(detector.Data[type], detector);
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

            private Color getLineColor(Int16 lineId, DATA_TYPE type)
            {
                if (LINE_BASE_COLORS.Length <= lineId)
                {
                    // Random color
                    Random rnd = new Random();

                    Color[] tempColors = new Color[LINE_BASE_COLORS.Length + 1];
                    LINE_BASE_COLORS.CopyTo(tempColors, 0);
                    LINE_BASE_COLORS = new Color[tempColors.Length];
                    tempColors[LINE_BASE_COLORS.Length - 1] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    tempColors.CopyTo(LINE_BASE_COLORS, 0);

                    LINE_TRANSPARENT_COLORS.CopyTo(tempColors, 0);
                    LINE_TRANSPARENT_COLORS = new Color[tempColors.Length];
                    tempColors[LINE_TRANSPARENT_COLORS.Length - 1] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    tempColors.CopyTo(LINE_TRANSPARENT_COLORS, 0);
                }

                return ((type == DATA_TYPE.BASE) ? LINE_BASE_COLORS[lineId] : LINE_TRANSPARENT_COLORS[lineId]);
            }

        #endregion

        #region Event

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

        #endregion

            private void tChart_MouseEnter(object sender, EventArgs e)
            {
                Series tmp;
                int clicked;

                tmp = (Series)sender;  // Sender is the Series

                // Obtain point index under mouse cursor
                clicked = tmp.Clicked(tChart.PointToClient(Cursor.Position));
                this.tChart.Series[0].Legend.Text = tmp.YValues.Value[clicked].ToString();

                // Show Series name and point index and value
     //           label1.Text = "Series: " + tmp.T oString() +
     //                       " point: " + clicked.ToString() +
      //                      " value: " + tmp.YValues.Value[clicked];

            }
    }
}
 