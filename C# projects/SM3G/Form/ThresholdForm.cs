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
    public partial class ThresholdForm : Form
    {
        public float[] thresholds;

        public bool thresholdUpdate;

        private String messageError = "Неверно задан порог";

        public ThresholdForm()
        {
            InitializeComponent();
            btnCancel.CausesValidation = false;
            thresholds = new float[4];
        }

        private void form_Load(object sender, EventArgs e)
        {
            foreach (Control control in this.groupBox1.Controls)
            {
                if (control is NumericUpDown)
                {
                    NumericUpDown nThreshold = control as NumericUpDown;
                    nThreshold.Value = (decimal)thresholds[Int32.Parse(nThreshold.Tag.ToString())];
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control control in this.groupBox1.Controls)
                {
                    if (control is NumericUpDown)
                    {
                        NumericUpDown nThreshold = control as NumericUpDown;
                        thresholds[Int32.Parse(nThreshold.Tag.ToString())] = (float)nThreshold.Value;
                    }
                }
                if (!validateThreshold())
                {
                    this.DialogResult = DialogResult.None;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private bool validateThreshold()
        {
            foreach (Control control in this.groupBox1.Controls)
            {
                if (control is NumericUpDown)
                {
                    NumericUpDown nThreshold = control as NumericUpDown;
                    int id = Int32.Parse(nThreshold.Tag.ToString());
                    if (id != 0)
                    {
                        if (thresholds[id - 1] > thresholds[id])
                        {
                            this.errorProvider.SetError(control, messageError);
                            return false;
                        }
                    }
                }
            } 
            
            return true;
        }

        private void m_nThreshold_Click(object sender, EventArgs e)
        {
            if (errorProvider.GetError((NumericUpDown)sender).Length > 0)
            {
                errorProvider.SetError((NumericUpDown)sender, "");
            }
        }
    }
}
