using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Net;
using System.Globalization;
using Vast.Wlds.DataProcessing;
using ipp;
using System.Diagnostics;
using System.Runtime.InteropServices;


using System.Drawing.Imaging;


namespace Wlds
{
    public delegate void DelegateZoomButtonClickedEventHandler(object sender, EventArgs e);
    public delegate void DelegateCircleButtonClickedEventHandler(object sender, EventArgs e);
    public delegate void DelegateCircleMeasurementChangedEventHandler(object sender, EventArgs e);
   
    public partial class MainForm : Form
    {
        public static int[] SamplingFrequency = {1838, 3676, 7352, 14704};
        public static MODE mode = MODE.BASE;

        public static List<Detector> detectors;

        private const Int16 HEADER_SIZE = 13;
        private const string DATE_FORMAT = "dd MMMM yyyy HH:mm:ss";

        // 000 to file name
        private string detectorNumberPrefix = "";
        private const int DETECTOR_NUMBER_LENGTH = 4;

        private Chart[] charts;
        private SplitterPanel[] chartPanels;

        public struct DateTimeRange 
        {
            public DateTime start;
            public DateTime end;
        }
        private DateTimeRange dateTimeRange;

        private NetworkCredential credentials;
        public MainForm()
        {
            InitializeComponent();

            detectors = new List<Detector>();
            String xmlFileSetting = System.IO.Directory.GetCurrentDirectory() + @"\Settings.xml";
            DeserializeObject(xmlFileSetting);

            charts = new Chart[] { m_tchartEnvelopeH, m_tchartEnvelopeL };
            for (Int16 chartId = 0; chartId < 2; chartId++)
            {
                charts[chartId].ZoomButtonClicked += new DelegateZoomButtonClickedEventHandler(m_btnZoom_Click);
                charts[chartId].CircleButtonClicked += new DelegateCircleButtonClickedEventHandler(m_btnCircle_Click);
                charts[chartId].CircleMeasurementChanged += new DelegateCircleMeasurementChangedEventHandler(circleMeasurementChanged);
            }
            chartPanels = new SplitterPanel[] { splitContainer.Panel1, splitContainer.Panel2 };
        }

        #region Data

            private HRESULT loadData()
            {
                HRESULT result = HRESULT.S_OK;
                String str = "";
                Int16 fileNumber = 0;

                foreach (Detector detector in detectors)
                {
                    setDetectorNumberPrefix(detector.Number);
                    getInfo(detector);

                    String searchOption = detectorNumberPrefix + detector.Number + "e*";
                    String[] fileNameList = Directory.GetFiles(stSerOb.LocalPath, searchOption, SearchOption.AllDirectories);
                    List<String> l = fileNameList.ToList();
                    String[] fileInDateTimeRangeList = Array.FindAll(fileNameList, findFileInDateTimeRange);
                    fileNumber = 1;

                    foreach (String fileName in fileInDateTimeRangeList)
                    {
                        result = getData(fileName, detector, fileNumber);
                        if (result == HRESULT.S_OK)
                        {
                            fileNumber++;
                        }
                    }
                }

                setData();

                return result;
            }

            private void clearData()
            {
                for (int chartId = 0; chartId < charts.Length; chartId++)
                {
                    charts[chartId].ZoomUndo();
                    charts[chartId].ClearData();
                }
                Circle.detectorIndex = 0;
                Circle.detectorIndexes = null;
            }
               
            private HRESULT setData()
                {
                    HRESULT result = HRESULT.S_OK;
                    Cursor.Current = Cursors.WaitCursor;
          
                    // Averaging detector data
                    foreach (Detector detector in detectors)
                    {
                        Statistic.SetAvgData(detector);
                        Statistic.geFFTData(detector, -1);

                        for (Int16 i = 0; i < detector.dataMeasurement.Count; i++)
                        {
                            result = Statistic.geFFTData(detector, i);
                        }
                    }

                    for (int chartId = 0; chartId < charts.Length; chartId++)
                    {
                        foreach (Detector detector in detectors)
                        {

                            if (result == HRESULT.S_OK)
                            {
                                if (chartId == 0)
                                {
                                    charts[chartId].SetChartLine(detector, DATA_TYPE.FFT, detector.data[DATA_TYPE.FFT]);
                                    charts[chartId].SetChartChannel(detector, DATA_TYPE.FFT);
                                } 
                                else 
                                {
                                    charts[chartId].SetChartLine(detector, DATA_TYPE.FFT_S, detector.data[DATA_TYPE.FFT_S]);
                                }
                            }
                        }
                        Circle.detectorIndexes = new int[detectors.Count];
                    }

                    Cursor.Current = Cursors.Default;
                    return result;
                }

            private HRESULT getData(String fileName, Detector detector, Int16 number)
            {
                HRESULT result = HRESULT.S_OK;
                Byte[] dataBuffer;
                uint[] dataBufferInt;

                result = readFile(fileName, out dataBuffer);
                if (result != HRESULT.S_OK)
                {
                    return result;
                }
                if ((dataBuffer == null) || (dataBuffer.Length == 0))
                {
                    MessageBox.Show("Empty file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_ERROR;
                }

                int len = detector.Info.NN * 1023;
                //8192 * 3
                if (dataBuffer.Length < len)
                {
                    return HRESULT.S_CANCEL;
                }
        	    if (dataBuffer[0] != 'S') 
                {
                    return HRESULT.S_CANCEL;
                }
                
                // Add file info
                DateTime fileDateTime = getFileDateTime(fileName);
                String description = fileDateTime.ToString(DATE_FORMAT);
                detector.Description.Add(description);
                description = "  U = " + (dataBuffer[2] * 256 + dataBuffer[3]).ToString();
                description += " T = " + (dataBuffer[11] * 256 + dataBuffer[12]).ToString();
                description += " Q = " + Convert.ToChar(dataBuffer[5]).ToString() + Convert.ToChar(dataBuffer[6]).ToString() + Convert.ToChar(dataBuffer[7]).ToString() + Convert.ToChar(dataBuffer[8]).ToString();
                detector.Description.Add(description);

                int dataLength = dataBuffer.Length;
                dataBufferInt = new uint[dataLength / 3];
                int dataLengthInt = dataBufferInt.Length;
                float[] dataBufferF = new float[dataLengthInt];

                for (int i = HEADER_SIZE, j = 0; i + 3 < dataLength; i += 3)
                {
                    dataBufferInt[j] = (((uint)dataBuffer[i]) << 16) | (((uint)dataBuffer[i + 1])<< 8) | ((uint)dataBuffer[i + 2]);

                    if ((dataBufferInt[j] & 0x800000) != 0)
                    {
                        dataBufferInt[j] = dataBufferInt[j] | 0xFF000000;
                        
                    }
                    int temp = (int)dataBufferInt[j];
                    dataBufferF[j] = (float)temp;
                    j++;
                }

                Statistic.MeanData(ref dataBufferF);
                Dictionary<DATA_TYPE, float[]> measurement = new Dictionary<DATA_TYPE,float[]>();
                foreach (DATA_TYPE type in Enum.GetValues(typeof(DATA_TYPE)))
                {
                    measurement.Add(type, null);
                }
                measurement[DATA_TYPE.BASE] = new float[dataLengthInt];
                detector.dataMeasurement.Add(measurement);

                int detectorIndex = detector.dataMeasurement.Count - 1;
                for (int j = 0; j < dataLengthInt; j++)
                {
                    detector.dataMeasurement[detectorIndex][DATA_TYPE.BASE][j] = (float)(dataBufferF[j] * 5000000.0 / (1 << 23) / 100);
                }

                return HRESULT.S_OK;
            }

            private HRESULT getInfo(Detector detector)
            {
                HRESULT result = HRESULT.S_OK;
                try
                {
                    string fileNameInfo = detectorNumberPrefix + detector.Number + ".txt";
                    // NN,D,TT
                    int detectorNumberLength = detector.Number.Length;

                    Byte[] dataBuffer;
                    result = readFile(stSerOb.LocalPath + "/" + fileNameInfo, out dataBuffer);
                    if (result == HRESULT.S_OK)
                    {
                        char[] dataBufferCh = new char[dataBuffer.Length];
                        for (int i = 0; i < dataBufferCh.Length - 1; i++)
                        {
                            dataBufferCh[i] = Convert.ToChar(dataBuffer[i]);
                        }

                        String infoStr = new String(dataBufferCh);
                        String[] info = infoStr.Split(new[] { ',' });
                        detector.Info.NN = Int16.Parse(info[0]);
                        detector.Info.D = Int16.Parse(info[1]);
                        detector.Info.TT = Int16.Parse(info[2]);
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                return result;
            }

            private DateTime getFileDateTime(String filePath)
            {
                int fileNamePosition = filePath.IndexOf(detectorNumberPrefix);
                if (fileNamePosition < 0)
                {
                    return new DateTime(1, 1, 2000, 0, 0, 0);
                }
                String fileName = filePath.Substring(fileNamePosition);

                // File format 0005e01d09d19e00t32t18e
                return new DateTime(
                    2000 + Int16.Parse(fileName.Substring(11, 2)),  // Year
                    Int16.Parse(fileName.Substring(8, 2)),          // Month
                    Int16.Parse(fileName.Substring(5, 2)),          // Day
                    Int16.Parse(fileName.Substring(14, 2)),         // Hour
                    Int16.Parse(fileName.Substring(17, 2)),         // Minute
                    Int16.Parse(fileName.Substring(20, 2)));        // Second
            }
            
            private void setDetectorNumberPrefix(String detectorNumber)
            {
                detectorNumberPrefix = "";
                for (int i = detectorNumber.Length; i < DETECTOR_NUMBER_LENGTH; i++)
                {
                    detectorNumberPrefix += "0";
                }
            }

            private bool findFileInDateTimeRange(String filePath)
            {
                DateTime fileDateTime = getFileDateTime(filePath);
                return (DateTime.Compare(dateTimeRange.start, fileDateTime) <= 0) && (DateTime.Compare(dateTimeRange.end, fileDateTime) >= 0);
            }

            private HRESULT readFile(String fileName, out Byte[] dataBuffer)
            {
                dataBuffer = null;
                try
                {
                    FileStream fileStream = new FileStream(fileName, FileMode.Open, System.IO.FileAccess.Read);
                    dataBuffer = new Byte[fileStream.Length];
                    fileStream.Read(dataBuffer, 0, (int)fileStream.Length);
                    fileStream.Close();
                    fileStream.Dispose();
                }
                catch
                {
                    MessageBox.Show("Error by reading from file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_ERROR;
                }

                return HRESULT.S_OK;
            }

        #endregion

        #region Serialize Settigs

            // Serialize parameters
            [XmlRoot()]
            public struct structSerializeObject
            {
                [XmlElement()]
                public string Host { get; set; }

                [XmlElement()]
                public string RemotePath { get; set; }

                [XmlElement()]
                public string LocalPath { get; set; }

                // View results parameters
                [XmlElement()]
                public DateTime DateTimeRangeStart { get; set; }

                [XmlArray("Detectors")]
                public Int16[] Detectors { get; set; }

                [XmlElement()]
                public int TimerInterval { get; set; }
            }

            public structSerializeObject stSerOb;
            private void SerializeObject(string filename)
            {
                FileStream fStream = null;

                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(structSerializeObject));
                    if (dateTimeRange.start != null)
                    {
                        stSerOb.DateTimeRangeStart = dateTimeRange.start;
                    }
                    stSerOb.Detectors = new Int16[detectors.Count];
                    for (int i = 0; i < detectors.Count; i++)
                    {
                        stSerOb.Detectors[i] = Int16.Parse(detectors[i].Number);
                    }

                    fStream = new FileStream(filename, FileMode.Create);
                    serializer.Serialize(fStream, stSerOb);
                }
                catch (Exception e)
                {

                }
                finally
                {
                    if (fStream != null)
                    {
                        fStream.Close();
                    }
                }
            }

            private void DeserializeObject(string fileName)
            {
                FileStream fStream = null;

                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(structSerializeObject));
                    fStream = new FileStream(fileName, FileMode.Open);
                    if (fStream.Length > 0)
                    {
                        stSerOb = (structSerializeObject)serializer.Deserialize(fStream);
                  }
                }
                catch
                {
                }
                finally
                {
                    if (fStream != null)
                    {
                        fStream.Close();
                    }
                    stSerOb.RemotePath = ((stSerOb.RemotePath == null) || (stSerOb.RemotePath.Equals(""))) ? "" : stSerOb.RemotePath;

                    DateTime dateStart = DateTime.Today.AddDays(-1);
                    dateTimeRange.start = (stSerOb.DateTimeRangeStart.Year == 1) ? new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, 00, 00, 0) : stSerOb.DateTimeRangeStart;
                    dateTimeRange.end = DateTime.Now;
 
                    stSerOb.Detectors = ((stSerOb.Detectors == null) || (stSerOb.Detectors.Count<Int16>() == 0)) ? new Int16[] { 1, 2, 0, 0 } : stSerOb.Detectors;
                    for (Int16 i = 0; i < stSerOb.Detectors.Length; i++)
                    {
                        detectors.Add(new Detector(i, stSerOb.Detectors[i].ToString()));
                    }

                    setTimerInterval(stSerOb.TimerInterval);
                }
            }

        #endregion


        #region Events

            private void form_FormClosed(object sender, FormClosedEventArgs e)
            {
                String xmlFileFolder = System.IO.Directory.GetCurrentDirectory() + @"\Settings.xml";
                SerializeObject(xmlFileFolder);
            }
            [DllImport("DLL_I2C_freescale.dll")]
            private static extern int Read_DATA_MLX90640(UInt16[] dat);

            private static int DATA_BUFFER_WIDTH = 32;
            private static int DATA_BUFFER_HEIGHT = 24;

            private static class Temperature
            {
                public static UInt16 max;
                public static UInt16 min;
                public static float average;
            }

            private void btnSettings_Click(object sender, EventArgs e)
            {

                int result;
                UInt16[] data = new UInt16[DATA_BUFFER_WIDTH * DATA_BUFFER_HEIGHT];
                result = getData(data);
                if (result < 0)
                {

                }
                else
                {
                    setTemperature(data);
                     pictureBox.Image =  getBitmap(data);
                }

                ParametersForm dlgForm = new ParametersForm();
                dlgForm.timerInterval = timer.Interval;

                if (dlgForm.ShowDialog() == DialogResult.OK)
                {
                    stSerOb.TimerInterval = dlgForm.timerInterval;
                    setTimerInterval(stSerOb.TimerInterval);
                }
            }

            private int getData(UInt16[] data)
            {
                int result = 0;
                for (int i = 0, j= 0; i < DATA_BUFFER_HEIGHT * DATA_BUFFER_WIDTH; i++)
                {
                    data[i] = (UInt16)j;
                    j = (j == 255) ? 0 : j + 1;
                }

    //            result = Read_DATA_MLX90640(data);

                return result;
            }

            private void setTemperature(UInt16[] dataBuffer)
            {
                UInt16 min = UInt16.MaxValue;
                UInt16 max = UInt16.MinValue;
                UInt16 sum = 0;

                for (int i = 0; i < DATA_BUFFER_HEIGHT * DATA_BUFFER_WIDTH; i++)
                {
                    min = (dataBuffer[i] < min) ? dataBuffer[i] : min;
                    max = (dataBuffer[i] > max) ? dataBuffer[i] : max;
                    sum += dataBuffer[i];
                }

                Temperature.min = min;
                Temperature.max = max;
                Temperature.average = sum / (dataBuffer.Length * 1.0F);
            }

            private Bitmap getBitmap(UInt16[] dataBuffer)
            {
                int scale = 5;
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
                        for (int sj = 0; sj < scale; sj++)
                        {
                            for (i = 0; i < DATA_BUFFER_WIDTH; i++)
                            {
                                int d = dataBuffer[i + (DATA_BUFFER_WIDTH * j)];
                                int temp = (255 * d) / Temperature.max;
                                Byte[] values = BitConverter.GetBytes(temp);
                                Byte value = values[0];
                                for (int s = 0; s < scale; s++)
                                {
                                    bitmapBuffer[k + s] = value;
                                    bitmapBuffer[k + scale + s] = value;
                                    bitmapBuffer[k + 2 * scale + s] = value;
                                }
                                k += 3 * scale;
                            }
                        }
                    }

                    Marshal.Copy(bitmapBuffer, 0, bitmapData.Scan0, bitmapBuffer.Length);
                }
                catch (ArgumentNullException e)
                {
                        MessageBox.Show("Error by transformation data array to bitmap array. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                 }
                catch (ArgumentOutOfRangeException e)
                {
                         MessageBox.Show("Error by transformation data array to bitmap array. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception e)
                {
                         MessageBox.Show("Error by transformation data array to bitmap array. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);

                    bitmapBuffer = null;
                    bitmapData = null;
                }

                return bitmap;
            }


            private void m_btnOpen_Click(object sender, EventArgs e)
            {
                if (mode != MODE.BASE)
                {
                    stopTimer();
                }
                SettingsForm dlgForm = new SettingsForm();
                dlgForm.dateTimeStart = dateTimeRange.start;
                dlgForm.dateTimeEnd = dateTimeRange.end;
                foreach (Detector detector in detectors)
                {
                    dlgForm.detectors.Add(Int16.Parse(detector.Number));
                }

                detectors.Clear();

                if (dlgForm.ShowDialog() == DialogResult.OK)
                {
                    dateTimeRange.start = dlgForm.dateTimeStart;
                    dateTimeRange.end = dlgForm.dateTimeEnd;
                    Int16 i = 0;
                    foreach (Int16 detectorNumber in dlgForm.detectors)
                    {
                        detectors.Add(new Detector(i, detectorNumber.ToString()));
                        detectors[i].Number = detectorNumber.ToString();
                        i++;
                    }

                    clearData();
                    loadData();
                }
            }

            private void m_btnUpload_Click(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    UriBuilder uri = new UriBuilder("ftp", stSerOb.Host, 21, stSerOb.RemotePath);
                    FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(uri.Uri);
                    listRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                    listRequest.Credentials = credentials;
                    listRequest.UsePassive = false;
                    List<string> lines = new List<string>();
                    string uriPath = uri.Path;

                    using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
                    using (Stream listStream = listResponse.GetResponseStream())
                    using (StreamReader listReader = new StreamReader(listStream))
                    {
                        while (!listReader.EndOfStream)
                        {
                            lines.Add(listReader.ReadLine());
                        }
                    }

                    if (!Directory.Exists(stSerOb.LocalPath))
                    {
                        DirectoryInfo directoryInfo = Directory.CreateDirectory(stSerOb.LocalPath);
                        if (!directoryInfo.Exists)
                        {
                            MessageBox.Show("Directory " + stSerOb.LocalPath + " cannot be created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            Cursor.Current = Cursors.Default;
                        }
                    }

                    foreach (string line in lines)
                    {
                        // For WebRequestMethods.Ftp.ListDirectoryDetails
                        string name = line;
                        string localFilePath = Path.Combine(stSerOb.LocalPath, name);
                        if (!System.IO.File.Exists(localFilePath))
                        {

                            uri.Path = uriPath + name;
                            UriBuilder fileUri = new UriBuilder(uri.Uri);
                            FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(fileUri.Uri);
                            downloadRequest.UsePassive = false;
                            downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                            downloadRequest.Credentials = credentials;

                            using (FtpWebResponse downloadResponse =
                                      (FtpWebResponse)downloadRequest.GetResponse())
                            using (Stream sourceStream = downloadResponse.GetResponseStream())
                            using (Stream targetStream = System.IO.File.Create(localFilePath))
                            {
                                byte[] buffer = new byte[10240];
                                int read;
                                while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    targetStream.Write(buffer, 0, read);
                                }
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    String status = ((FtpWebResponse)ex.Response).StatusDescription;
                    MessageBox.Show("Error by files download. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                Cursor.Current = Cursors.Default;
            }

            private void m_btnZoom_Click(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                Int16 detectorId = (Int16)(((Chart)sender).Id - 1);
                if (detectorId < 0)
                {
 //                   return;
                }

                if (this.panelZoom.Visible)
                {
                    chartPanels[detectorId].Controls.Add(charts[detectorId]);

                    this.panelZoom.Controls.Remove(charts[detectorId]);

                    this.panelZoom.SendToBack();
                    this.panelZoom.Visible = false;
                }
                else
                {
                    this.panelZoom.Controls.Add(charts[detectorId], 0, 1);
                    chartPanels[detectorId].Controls.Remove(charts[detectorId]);

                    this.panelZoom.BringToFront();
                    this.panelZoom.Visible = true;
                }

                Cursor.Current = Cursors.Default;
            }

            private void m_btnCircle_Click(object sender, EventArgs e)
            {
                Chart currentChart = sender as Chart;
                currentChart.SetDetectorIndexList();
                Circle.measurementNumberCount = getMeasurementNumberCount();
                for (int chartId = 0; chartId < charts.Length; chartId++)
                {
                    charts[chartId].HandleModeCircle();
                }

                if (mode == MODE.BASE)
                {
                    stopTimer();
                }
                else
                {
                    Circle.measurementNumber = 0;
                    timer.Start();
                }
            }
 
        #endregion

        #region Circle

            private void stopTimer()
            {
                timer.Stop();
                mode = MODE.BASE;
                if (charts != null)
                {
                    for (int chartId = 0; chartId < charts.Length; chartId++)
                    {
                        charts[chartId].ClearCircle();
                    }
                }
                Circle.detectorIndexes = null;
            }

            private void setTimerInterval(int interval) 
            {
                stopTimer();

                timer.Interval = (interval == 0) ? 100000 : interval;
            }

            private void timer_Tick(object sender, EventArgs e)
            {
                for (int chartId = 0; chartId < charts.Length; chartId++)
                {
                    for (Circle.detectorIndex = 0; Circle.detectorIndex < Circle.detectorIndexes.Count(); Circle.detectorIndex++)
                    {

                        charts[chartId].SetChartLine(detectors[Circle.detectorIndexes[Circle.detectorIndex]], charts[chartId].Type, 
                            detectors[Circle.detectorIndexes[Circle.detectorIndex]].dataMeasurement[Circle.measurementNumber][charts[chartId].Type]);
                    }
                }
                Circle.measurementNumber = (Circle.measurementNumber == Circle.measurementNumberCount - 1) ? 0 : Circle.measurementNumber + 1;
            }

            private void circleMeasurementChanged(object sender, EventArgs e)
            {
                Control control = sender as Control;
                if (control == null) return;

                timer.Stop();

                switch (control.Tag.ToString())
                {
                    case "next":
                        Circle.measurementNumber = (Circle.measurementNumber == Circle.measurementNumberCount - 1) ? 0 : Circle.measurementNumber + 1;
                        break;
                    case "prev":
                        Circle.measurementNumber = (Circle.measurementNumber == 0) ? Circle.measurementNumber - 1 : Circle.measurementNumber - 1;
                        break;
                    case "page":
                        NumericUpDown numericUpDown = sender as NumericUpDown;
                        Circle.measurementNumber = (int)numericUpDown.Value - 1;
                        break;
                    case "pause":
                        for (int chartId = 0; chartId < charts.Length; chartId++)
                        {
                            charts[chartId].HandleModePause();
                        }
                        if (MainForm.mode == MODE.CIRCLE)
                        {
                            timer.Start();
                        }
                        break;
                    case "first":
                        Circle.measurementNumber = 0;
                        break;
                    case "end":
                        Circle.measurementNumber = Circle.measurementNumberCount;
                        break;
                }
                for (int chartId = 0; chartId < charts.Length; chartId++)
                {
                    for (Circle.detectorIndex = 0; Circle.detectorIndex < Circle.detectorIndexes.Count(); Circle.detectorIndex++)
                    {
                        charts[chartId].SetChartLine(detectors[Circle.detectorIndexes[Circle.detectorIndex]], charts[chartId].Type, 
                            detectors[Circle.detectorIndexes[Circle.detectorIndex]].dataMeasurement[Circle.measurementNumber][charts[chartId].Type]);
                        charts[chartId].SetCircleMeasurement();
                    }
                }
            }

        private int getMeasurementNumberCount()
        {
            int min = detectors[0].dataMeasurement.Count;
            for (Circle.detectorIndex = 1; Circle.detectorIndex < Circle.detectorIndexes.Count(); Circle.detectorIndex++)
            {
                if (detectors[Circle.detectorIndexes[Circle.detectorIndex]].dataMeasurement.Count < min)
                {
                    min = detectors[Circle.detectorIndexes[Circle.detectorIndex]].dataMeasurement.Count;
                }
            }

            return min;
        }

        #endregion
    }
}
