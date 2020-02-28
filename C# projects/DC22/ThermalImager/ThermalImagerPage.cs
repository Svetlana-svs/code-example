using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using DC22.Shell.Controls.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading; 

namespace DC22.Shell.Controls.Apps
{
    public partial class ThermalImagerPage : UserControl, IPage, IHelp
    {       
        [DllImport("DLL_I2C_freescale.dll")]
        private static extern int Read_DATA_MLX90640(Int32[] dat, Int32 calibration);

        public enum ColorMode
        {
            GRAYSCALE = 0,
            COLOR = 1
        }

        public enum CalibrationMode
        {
            CLEAR = 0,
            CALIBRATION = 1,
            MEASUREMENT = 2
        }

        private static int DATA_BUFFER_WIDTH = 32;
        private static int DATA_BUFFER_HEIGHT = 24;

        // Variable for scalinhg image to all screen
        private int scale = 1;
        private int result = 0;

        private Int32[] data;

        private bool measurementProgress = false;
        private bool firstMeasurementProgress = true;
        private ColorMode colorMode = ColorMode.GRAYSCALE;
        private CalibrationMode calibrationMode = CalibrationMode.CLEAR;

        private static class Temperature
        {
            public static int max;
            public static int min;
            public static float average;
        }

        public ThermalImagerPage()
        {
            InitializeComponent();
            ApplyTheme();

            int scaleWidth = this.Width / DATA_BUFFER_WIDTH;

            pbxThermalmager.SizeMode = PictureBoxSizeMode.StretchImage;

            contextLabelColor.Description = Resources.Color;
            labelCalibration.Text = Resources.Calibration;
            contextLabelStop.Description = Resources.Stop;
            contextLabelStop.Number = "*";
            labelMin.Text = Resources.Min;
            labelMax.Text = Resources.Max;
            labelAverage.Text = Resources.Average;

            WaitControl.WaitCompletedSt += WaitControl_WaitCompletedSt;
            WaitControl.ShowWait(Resources.Loading, new WaitDelegate(GetData));
       }

        #region IPage
        public string PageId
        {
            get { return Id; }
        }

        public event EventHandler Exit;

        public object Data
        {
            get { return null; }
            set
            {
            }
        }


        public MainControl ParentMainControl
        {
            get
            {
                if (this.Parent != null && this.Parent is MainControl)
                    return (MainControl)this.Parent;
                else
                    return null;
            }
        }

        public void SetFirstFocusControl()
        {
        }

        public static string Id
        {
            get { return "thermal_imager"; }
        }
        
        #endregion IPage

        public void ApplyTheme()
        {
            ThemeUtils.ApplyColors(ThemeUtils.ActiveBackColor, ThemeUtils.ActiveForeColor, this);
            // Applay colors to controls
            ThemeUtils.ApplyColors(ThemeUtils.SpecialBackColor, ThemeUtils.SpecialForeColor,
                panelButton, labelColor, labelCalibrationButton, labelStart,
                this.contextLabelColor, this.contextLabelStop, this.labelCalibration, this.labelCalibrationButton,
                panelTemperatute, labelMin, labelMax, labelAverage, m_labelMin, m_labelMax, m_labelAverage);

            pbxArrowWhitePB.Visible = (ThemeUtils.CurrentTheme.Equals("Dark")) ? true : false;
            pbxArrowWhitePT.Visible = (ThemeUtils.CurrentTheme.Equals("Dark")) ? true : false;
            pbxArrowBlackPB.Visible = (ThemeUtils.CurrentTheme.Equals("Dark")) ? false : true;
            pbxArrowBlackPT.Visible = (ThemeUtils.CurrentTheme.Equals("Dark")) ? false : true;
        }
        
        void WaitControl_WaitCompletedSt(object sender, EventArgs e)
        {
            WaitControl.WaitCompletedSt -= WaitControl_WaitCompletedSt;
            ParentMainControl.AlwaysActivated = false;
            ParentMainControl.Header = Resources.ThermalImager;
 
            SetData();
        }

        private void Work()
        {
           GetData();
           SetData();
        }

        private void GetData()
        {
            Int32 cal = (Int32)calibrationMode;
            data = new Int32[DATA_BUFFER_WIDTH * DATA_BUFFER_HEIGHT];
            result = Read_DATA_MLX90640(data, cal);
        }

        private void SetData()
        {
            if (result < 0)
            {
                FrameworkTools.ErrorProcessing.WriteLog(new Exception(""));
                CMessageBox.Show(this.ParentMainControl, Resources.Error_on_bitmap_create, Resources.Warning);
            }
            else
            {
                setTemperature();
                pbxThermalmager.Image = (colorMode == ColorMode.GRAYSCALE) ? getBitmapGrayscale() : getBitmapColor();

                if (firstMeasurementProgress)
                {
                    timer.Enabled = true;
                    firstMeasurementProgress = false;
                    measurementProgress = true;
                }
            }
        }

        private void setTemperature()
        {
            Temperature.min = Int32.MaxValue;
            Temperature.max = Int32.MinValue;
            Int32 sum = 0;

            for (int i = 0; i < DATA_BUFFER_HEIGHT * DATA_BUFFER_WIDTH; i++)
            {
                Temperature.min = (data[i] < Temperature.min) ? data[i] : Temperature.min;
                Temperature.max = (data[i] > Temperature.max) ? data[i] : Temperature.max;
                sum += data[i];
            }

            Temperature.average = sum / (data.Length * 1.0F);

            m_labelMin.Text = Temperature.min.ToString() + " °C";
            m_labelMax.Text = Temperature.max.ToString() + " °C";
            m_labelAverage.Text = Temperature.average.ToString("0.00") + " °C";
        }

        private Bitmap getBitmapGrayscale()
        {
            Rectangle bitmapRect = new Rectangle(0, 0, DATA_BUFFER_WIDTH * scale, DATA_BUFFER_HEIGHT * scale);
            Bitmap bitmap = new Bitmap(DATA_BUFFER_WIDTH * scale, DATA_BUFFER_HEIGHT * scale, PixelFormat.Format24bppRgb);
            BitmapData bitmapData = bitmap.LockBits(bitmapRect, System.Drawing.Imaging.ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            Byte[] bitmapBuffer = new Byte[3 * DATA_BUFFER_WIDTH * DATA_BUFFER_HEIGHT * scale * scale];
            int k = 0;
            int j = 0;
            int i = 0;
            try
            {
                for (j = 0; j < DATA_BUFFER_HEIGHT; j++)
                {
                    for (i = 0; i < DATA_BUFFER_WIDTH; i++)
                    {
                        byte temp = (byte)((255 * data[i + (DATA_BUFFER_WIDTH * j)]) / Temperature.max);
                        bitmapBuffer[k] = temp;
                        bitmapBuffer[k + 1] = temp;
                        bitmapBuffer[k + 2] = temp;
                        k += 3;
                    }
 /*                                     for (j = 0; j < DATA_BUFFER_HEIGHT; j++)
                {
                  for (int sj = 0; sj < scale; sj++)
                    {
                        for (i = 0; i < DATA_BUFFER_WIDTH; i++)
                        {
                            for (int s = 0; s < scale; s++)
                            {
                                byte temp = (byte)((255 * temperature[i + (DATA_BUFFER_WIDTH * j)]) / Temperature.max);
                                bitmapBuffer[k + s] = temp;
                                bitmapBuffer[k + scale + s] = temp;
                                bitmapBuffer[k + 2 * scale + s] = temp;
                            }
                            k += 3 * scale;
                        }

                    }
  * */
                }

                Marshal.Copy(bitmapBuffer, 0, bitmapData.Scan0, bitmapBuffer.Length);
            }
            catch (Exception e)
            {
                FrameworkTools.ErrorProcessing.WriteLog(e);
                CMessageBox.Show(this.ParentMainControl, Resources.Error_on_bitmap_create, Resources.Warning);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);

                bitmapBuffer = null;
                bitmapData = null;
            }

            return bitmap;
        }

        private Bitmap getBitmapColor()
        {
            Rectangle bitmapRect = new Rectangle(0, 0, DATA_BUFFER_WIDTH * scale, DATA_BUFFER_HEIGHT * scale);
            Bitmap bitmap = new Bitmap(DATA_BUFFER_WIDTH * scale, DATA_BUFFER_HEIGHT * scale, PixelFormat.Format24bppRgb);
            BitmapData bitmapData = bitmap.LockBits(bitmapRect, System.Drawing.Imaging.ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            Byte[] bitmapBuffer = new Byte[3 * DATA_BUFFER_WIDTH * DATA_BUFFER_HEIGHT * scale * scale];
            int k = 0;
            int j = 0;
            int i = 0;

            float calibration = (255F + 255F + 255F) / Math.Abs((Temperature.max - Temperature.min));
            try
            {
                for (j = 0; j < DATA_BUFFER_HEIGHT; j++)
                {
                    for (i = 0; i < DATA_BUFFER_WIDTH; i++)
                    {
                        //                        byte temp = (byte)((255 * temperature[i + (DATA_BUFFER_WIDTH * j)]) / Temperature.max);
                        float rgb = calibration * Math.Abs(Temperature.min - data[i + (DATA_BUFFER_WIDTH * j)]);
                        if (rgb < 255)
                        {
                            bitmapBuffer[k + 2] = (byte)(255 - rgb);     // r
                            bitmapBuffer[k + 1] = (byte)(255 - rgb);
                            bitmapBuffer[k] = (byte)(255 - rgb * 0.4); // b
                        }
                   
                        if (255 < rgb && rgb < 510) 
                        {
                            bitmapBuffer[k + 2] = (byte)255;
                            bitmapBuffer[k + 1] = (byte)255;
                            bitmapBuffer[k] = (byte)(255 - (rgb - 255));
                        }
                       
                        if (rgb > 510)
                        {
                            bitmapBuffer[k + 2] = (byte)255;
                            bitmapBuffer[k + 1] = (byte)(rgb - 510);
                            bitmapBuffer[k] = (byte)0;
                        }
                        
                        k += 3;
                    }
                }

                Marshal.Copy(bitmapBuffer, 0, bitmapData.Scan0, bitmapBuffer.Length);
            }
            catch (Exception e)
            {
                FrameworkTools.ErrorProcessing.WriteLog(e);
                CMessageBox.Show(this.ParentMainControl, Resources.Error_on_bitmap_create, Resources.Warning);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);

                bitmapBuffer = null;
                bitmapData = null;
            }

            return bitmap;
        }
        
        void ctrl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                    // 0
                    calibrationMode = CalibrationMode.CLEAR;
                    break;
                case Keys.D1:
                    calibrationMode = CalibrationMode.CALIBRATION;
                    break;
                case Keys.D2:
                    // 2
                    calibrationMode = CalibrationMode.MEASUREMENT;
                    break;
                case Keys.D6:
                    colorMode = (colorMode == ColorMode.GRAYSCALE) ? ColorMode.COLOR : ColorMode.GRAYSCALE;
                    contextLabelColor.Description = (colorMode == ColorMode.GRAYSCALE) ? Resources.Color : Resources.Grayscale;
                    break;
                case Keys.Up:
                case Keys.Down:
                    // Arrow Up Down
                    this.panelButton.Visible = (panelTemperatute.Visible) ? true : false;
                    this.panelTemperatute.Visible = (panelButton.Visible) ? false : true;
                    if (panelButton.Visible)
                    {
                        contextLabelColor.Description = (colorMode == ColorMode.GRAYSCALE) ? Resources.Color : Resources.Grayscale;

                        contextLabelStop.Description =  measurementProgress ? Resources.Stop : " " + Resources.Start;
                        contextLabelStop.Number = measurementProgress ? "*" : "En";
                    }
                    break;
                case (Keys)190:
                    if (measurementProgress)
                    {
                        timer.Enabled = false;
                        measurementProgress = false;
                        contextLabelStop.Description = " " + Resources.Start;
                        contextLabelStop.Number = "En";
                    }
                    break;
                case Keys.Enter:
                    if (!timer.Enabled && !measurementProgress && !firstMeasurementProgress)
                    {
                        timer.Enabled = true;
                        measurementProgress = true;
                        labelStart.Text = Resources.Stop + ": *";
                    }
                    break;
                case Keys.Escape:
                    if (m_helpControl.Visible)
                    {
                        m_helpControl.Visible = false;
                        this.m_helpControl.Focus();
                        if (measurementProgress)
                        {
                            timer.Enabled = true;
                        }
                        break;
                    }
                    exitThermalImagerPage();
                    break;
                case Keys.A:
                    timer.Enabled = false;
                    GetHelp(sender);
                break;
            }
        }

        private void exitThermalImagerPage()
        {
            ParentMainControl.AlwaysActivated = true;
 //           DC22.Tools.ApplicationState.ExternalAppStarted = false;

            timer.Enabled = false;
            measurementProgress = false;
            Exit(this, EventArgs.Empty);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Work();
        }

        #region IHelp

            public void SetFocus(FocusControl focusControl)
            {
            }

            public void SetFocusedControl(object sender)
            {
            }

            public void GetHelp(object sender)
            {
                timer.Enabled = false;
                SetFocusedControl(sender);
                m_helpControl.BringToFront();
                m_helpControl.Text = Help.Help.getHelpText("", this.PageId);
                m_helpControl.Visible = true;
                m_helpControl.Focus();
            }

        #endregion IHelp
    }
}
