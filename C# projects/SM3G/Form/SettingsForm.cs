using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SM3G
{
    public partial class SettingsForm : Form
    {
        public DateTime dateTimeStart;
        public DateTime dateTimeEnd;

        public List<Int16> detectors;

        private static string DATE_FORMAT = "dd MMMM yyyy г."; 
        
        public SettingsForm()
        {
            InitializeComponent();
            detectors = new List<Int16>();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            dateTimeStart = this.m_dtpDateTimeStart.Value;
            dateTimeEnd = this.m_dtpDateTimeEnd.Value;
            detectors = new List<Int16>();

            foreach (var control in this.groupDetector.Controls)
            {
                NumericUpDown numericUpDown = control as NumericUpDown;

                if ((numericUpDown != null) &&  (numericUpDown.Value != 0))
                {
                    detectors.Add((Int16)numericUpDown.Value);
                }
            }
        }

        private void m_dtpDateTimeStart_CloseUp(object sender, EventArgs e)
        {
            lblDateTimeStart.Text = ((DateTimePicker)sender).Value.ToString(DATE_FORMAT);
        }

        private void m_dtpDateTimeEnd_CloseUp(object sender, EventArgs e)
        {
            lblDateTimeEnd.Text = ((DateTimePicker)sender).Value.ToString(DATE_FORMAT);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.m_dtpDateTimeStart.Format = DateTimePickerFormat.Time;
            this.m_dtpDateTimeStart.Value = dateTimeStart;

            this.m_dtpDateTimeEnd.Format = DateTimePickerFormat.Time;
            this.m_dtpDateTimeEnd.Value = dateTimeEnd;

            lblDateTimeStart.Text = this.m_dtpDateTimeStart.Value.ToString(DATE_FORMAT);
            lblDateTimeEnd.Text = this.m_dtpDateTimeEnd.Value.ToString(DATE_FORMAT);


            foreach (var control in this.groupDetector.Controls)
            {
                NumericUpDown numericUpDown = control as NumericUpDown;
                if (numericUpDown != null)
                {
                    int detectorId = Int32.Parse(numericUpDown.Tag.ToString());
                    if (detectorId < detectors.Count)
                    {
                        numericUpDown.Value = detectors[detectorId];
                    }
                }
            }
        }
    }
}
