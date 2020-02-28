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


namespace SM3G
{
    public delegate void DelegateZoomButtonClickedEventHandler(object sender, EventArgs e);
    public delegate void DelegateChartPageChangedEventHandler(object sender, EventArgs e);
    public delegate void DelegateCalibrationEventHandler(object sender, EventArgs e);
   
    public partial class MainForm : Form
    {
        private const Int16 HEADER_SIZE = 13;
        private const string DATE_FORMAT = "dd MMMM yyyy HH:mm:ss";

        private Chart[] charts;
        private SplitterPanel[] chartPanels;

        public struct DateTimeRange 
        {
            public DateTime start;
            public DateTime end;
        }
        private DateTimeRange dateTimeRange;
        public static DateTimeRange dateTimeRangeMeasure;
        public static List<Detector> detectors;

        private NetworkCredential credentials;

        public MainForm()
        {
            InitializeComponent();

            String xmlFileSetting = System.IO.Directory.GetCurrentDirectory() + @"\Settings.xml";
            DeserializeObject(xmlFileSetting);

            charts = new Chart[] { m_tchartEnvelopeH, m_tchartEnvelopeL };
            for (Int16 chartId = 0; chartId < 2; chartId++)
            {
                charts[chartId].ZoomButtonClicked += new DelegateZoomButtonClickedEventHandler(m_btnZoom_Click);
            }
            detectors = new List<Detector>();
            chartPanels = new SplitterPanel[] { splitContainer.Panel1, splitContainer.Panel2 };
        }

        #region Serialize Settigs

            // Serialize parameters
            [XmlRoot()]
            public struct structSerializeObject
            {
                // FTP upload files settings
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

                // Diagnostic parameters
                [XmlArray("Thresholds")]
                public float[] Thresholds { get; set; }
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
                    if (stSerOb.Thresholds == null)
                    {
                        stSerOb.Thresholds = new float[Threshold.Thresholds.Length];
                    }
                    Threshold.Thresholds.CopyTo(stSerOb.Thresholds, 0);

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

                    DateTime dateStart = DateTime.Today.AddDays(- 1);
                    dateTimeRange.start = (stSerOb.DateTimeRangeStart.Year == 1) ? new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, 00, 00, 0) : stSerOb.DateTimeRangeStart;
                    dateTimeRange.end = DateTime.Now;
                    stSerOb.Detectors = ((stSerOb.Detectors == null) || (stSerOb.Detectors.Count<Int16>() == 0)) ? new Int16[] { 1, 2, 0, 0 } : stSerOb.Detectors;
                    if ((stSerOb.Thresholds == null) || (stSerOb.Thresholds.Count<float>() == 0))
                    {
                        Threshold.Thresholds = new float[] { 0.01F, 0.02F, 0.03F, 0.04F };
                    } 
                    else
                    {
                        stSerOb.Thresholds.CopyTo(Threshold.Thresholds, 0);
                    }
                }
            }

        #endregion

        #region Data

            private HRESULT loadData()
            {
                HRESULT result = HRESULT.S_OK;
                String str = "";
                Int16 fileNumber = 0;

                foreach (Detector detector in detectors)
                {
                    string searchOption = "000" + detector.Name + "*.dat";
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
           
            private void setData()
            {
                Cursor.Current = Cursors.WaitCursor;
      
                for (int chartId = 0; chartId < 2; chartId++)
                {
                    // Displaying data for each detector in chart
                    foreach (Detector detector in detectors)
                    {
                        if (chartId == 0)
                        {
                            charts[chartId].SetChartLine(detector, DATA_TYPE.BASE);
                            charts[chartId].SetChartDetector(detector, DATA_TYPE.BASE);
                        }
                        else
                        {
                            charts[1].SetChartLine(detector, DATA_TYPE.T);
                        }
                    }

                    // Set thresholders lines on first chart
                    if (chartId == 0)
                    {
                        for (int i = 0; i < Threshold.Thresholds.Length; i++)
                        {
                            charts[chartId].SetChartLineThreshold(Threshold.Thresholds[i], i);
                        }
                    }
                    
                    charts[chartId].SetChatLineIncrement();
                }

                Cursor.Current = Cursors.Default;
            }

            /*
             * 27 bites
             */
            private HRESULT getData(String fileName, Detector detector, Int16 number)
            {
                HRESULT result = HRESULT.S_OK;
                Byte[] dataBuffer;

                result = readFile(fileName, out dataBuffer);
                if (result != HRESULT.S_OK)
                {
                    return result;
                }
                if ((dataBuffer == null) || (dataBuffer.Length == 0))
                {
    //                MessageBox.Show("Empty file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_CANCEL;
                }

                if (dataBuffer.Length != 27)
                {
                    MessageBox.Show("File length is failure.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_ERROR;
                }
        	    if (dataBuffer[1] != 'U') 
                {
                    MessageBox.Show("A value of the voltage is failure.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_CANCEL;
                }
                if (dataBuffer[4] != 'Q')
                {
                    MessageBox.Show("A value of the connection quality is failure.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_CANCEL;
                }
                if (dataBuffer[25] != 0xD)
                {
                    MessageBox.Show("File end is failure.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_CANCEL;
                }
                Measure measure = new Measure();
                // Add file info
                measure.Time = getFileDateTime(fileName);
                int res = DateTime.Compare(dateTimeRangeMeasure.start, measure.Time);
                dateTimeRangeMeasure.start = (res < 0) ? dateTimeRangeMeasure.start : measure.Time;
                int res1 = DateTime.Compare(dateTimeRangeMeasure.end, measure.Time);

                dateTimeRangeMeasure.end = (res1 > 0) ? dateTimeRangeMeasure.end : measure.Time;

                String description = measure.Time.ToString(DATE_FORMAT);
                detector.Description.Add(description);

                measure.U = dataBuffer[2] * 256 + dataBuffer[3];

                char[] Q = {Convert.ToChar(dataBuffer[5]), Convert.ToChar(dataBuffer[6]), Convert.ToChar(dataBuffer[7]), Convert.ToChar(dataBuffer[8])};
                measure.Q = new String(Q);
                measure.T = (Convert.ToChar(dataBuffer[10]).Equals('+') ? 1 : -1) * (dataBuffer[11] * 256 + dataBuffer[12]);
                int A = (int)((((uint)dataBuffer[13]) << 24) | (((uint)dataBuffer[14]) << 16) | (((uint)dataBuffer[15]) << 8) | ((uint)dataBuffer[16]));
                measure.A = (double)A / (double)1000;
                measure.V = (int)((((uint)dataBuffer[17]) << 24) | (((uint)dataBuffer[18]) << 16) | (((uint)dataBuffer[19]) << 8) | ((uint)dataBuffer[20]));
                measure.S = (int)((((uint)dataBuffer[21]) << 24) | (((uint)dataBuffer[22]) << 16) | (((uint)dataBuffer[23]) << 8) | ((uint)dataBuffer[24]));

                description = "  U = " + Convert.ToString(measure.U);
                description += " Q = " + measure.Q;

                detector.Description.Add(description);
                detector.DataMeasurement.Add(measure);

                return HRESULT.S_OK;
            }

            private void clearData()
            {
                for (int chartId = 0; chartId < charts.Length; chartId++)
                {
                    charts[chartId].ZoomUndo();
                    charts[chartId].ClearData();
                }
                dateTimeRangeMeasure.start = DateTime.Now;
                dateTimeRangeMeasure.end = DateTime.MinValue;
            }

            private DateTime getFileDateTime(String filePath)
            {
                int fileNamePosition = filePath.IndexOf("000");
                if (fileNamePosition < 0)
                {
                    return new DateTime(1, 1, 2000, 0, 0, 0);
                }
                String fileName = filePath.Substring(fileNamePosition);

                // File format 0005e19d11d19e12t47t52e
                return new DateTime(
                    2000 + Int16.Parse(fileName.Substring(11, 2)),  // Year
                    Int16.Parse(fileName.Substring(8, 2)),          // Month
                    Int16.Parse(fileName.Substring(5, 2)),          // Day
                    Int16.Parse(fileName.Substring(14, 2)),         // Hour
                    Int16.Parse(fileName.Substring(17, 2)),         // Minute
                    Int16.Parse(fileName.Substring(20, 2)));        // Second
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

        #region Event

            private void m_btnOpen_Click(object sender, EventArgs e)
            {
                SettingsForm dlgForm = new SettingsForm();
                dlgForm.dateTimeStart = dateTimeRange.start;
                dlgForm.dateTimeEnd = dateTimeRange.end;
                foreach (Int16 detectorNumber in stSerOb.Detectors)
                {
                    if (detectorNumber != 0) 
                    {
                        dlgForm.detectors.Add(detectorNumber);
                    }
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
                        stSerOb.Detectors[i] = detectorNumber;
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
                Int16 chartId = (Int16)(((Chart)sender).Id - 1);

                if (this.panelZoom.Visible)
                {
                    chartPanels[chartId].Controls.Add(charts[chartId]);
                    this.panelZoom.Controls.Remove(charts[chartId]);
 
                    this.panelZoom.SendToBack();
                    this.panelZoom.Visible = false;
                }
                else
                {
                    this.panelZoom.Controls.Add(charts[chartId], 0, 1);
                    chartPanels[chartId].Controls.Remove(charts[chartId]);

                    this.panelZoom.BringToFront();
                    this.panelZoom.Visible = true;
                }

                Cursor.Current = Cursors.Default;
            }

            private void form_FormClosed(object sender, FormClosedEventArgs e)
            {
                String xmlFileFolder = System.IO.Directory.GetCurrentDirectory() + @"\Settings.xml";
                SerializeObject(xmlFileFolder);
            }

            private void m_btnThreshold_Click(object sender, EventArgs e)
            {
                ThresholdForm dlgForm = new ThresholdForm();
                for (int i = 0; i < Threshold.Thresholds.Length; i++)
                {
                    dlgForm.thresholds[i] = Threshold.Thresholds[i];
                }

                if (dlgForm.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < Threshold.Thresholds.Length; i++)
                    {
                        Threshold.Thresholds[i] = dlgForm.thresholds[i];
                    }
                }
            }

        #endregion Event
    }
}
