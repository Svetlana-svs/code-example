using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RMU1
{
    public partial class Launch : Form
    {
        public int diameter, wall, volltage, crop, overlap, framesCount, expositionTime, targetTime, expositionTimeAbut;
        public double current;
        public String[] comment;
        public bool reverse;
        private bool isNumeric = false;
        public Launch()
        {
            InitializeComponent();
        }

        private void Launch_Load(object sender, EventArgs e)
        {
            cbxPipeDiameter.SelectedIndex = cbxPipeDiameter.FindString(diameter.ToString());
            cbxPipeWall.SelectedIndex = cbxPipeWall.FindString(wall.ToString()); 
            tbxVolltage.Text = volltage.ToString();
            tbxCurrent.Text = current.ToString();
            tbxOverlapZone.Text = overlap.ToString();
            tbxCropZone.Text = crop.ToString();
            tbFramesCount.Text = GetFramesCount();
            tbxExpositionTime.Text = expositionTime.ToString();
            tbxTargetingTime.Text = targetTime.ToString();
            chbxReverse.Checked = reverse;
            tbxExpositionTimeAbut.Text = GetExpositionTimeAbut();
        }

        private String GetFramesCount()
        {
            if (cbxPipeDiameter.Text != "") 
            {
                return ((int)(((int.Parse(cbxPipeDiameter.Text) * Math.PI) / (50 - 2 - (int.Parse(tbxOverlapZone.Text)))) + 1)).ToString();
            } 
            else 
            {
                return "0";
            }
        }

        private String GetExpositionTimeAbut()
        {
            return tbxExpositionTime.Text;
        }

        private void tbx_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tbx = (TextBox)sender;
            bool isAlreadyPoint = (tbx.Text.IndexOf(',') == -1 && tbx.Text.IndexOf('.') == -1) ? false : true;
            if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                isNumeric = true;
            }
            else if (InputLanguage.CurrentInputLanguage.Culture.Name.Substring(0, 2) == "ru")
            {
                if ((e.KeyValue == 191) && !(isAlreadyPoint))
                {
                    isNumeric = true;
                }
            }
            else if ((e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.OemPeriod) && !(isAlreadyPoint))
            {
                isNumeric = true;
            }
        }

        private void tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumeric)
            {
                e.Handled = true;
            }
            isNumeric = false;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            cbxPipeDiameter.SelectedIndex = cbxPipeDiameter.FindString(diameter.ToString());
            cbxPipeWall.SelectedIndex = cbxPipeWall.FindString(wall.ToString());
            tbxVolltage.Text = volltage.ToString();
            tbxCurrent.Text = current.ToString();
            tbxOverlapZone.Text = overlap.ToString();
            tbxCropZone.Text = crop.ToString();
            tbxExpositionTime.Text = expositionTime.ToString();
            tbxTargetingTime.Text = targetTime.ToString();
            chbxReverse.Checked = reverse;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                diameter = int.Parse(cbxPipeDiameter.Text);
                wall = int.Parse(cbxPipeWall.Text);
                volltage = int.Parse(tbxVolltage.Text);
                current = double.Parse(tbxCurrent.Text);
                overlap = int.Parse(tbxOverlapZone.Text);
                crop = int.Parse(tbxCropZone.Text);
                framesCount = int.Parse(tbFramesCount.Text);
                expositionTime = int.Parse(tbxExpositionTime.Text);
                targetTime = int.Parse(tbxTargetingTime.Text);
                reverse = chbxReverse.Checked;
                comment = tbxComment.Lines;
            }
            catch (Exception ex)
            {
                
            }
        }

        private void pipeParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFramesCount.Text = GetFramesCount();
        }

        private void expositionTime_TextChanged(object sender, EventArgs e)
        {
            tbxExpositionTimeAbut.Text = GetExpositionTimeAbut();
        }

        private void tbxOverlapZone_TextChanged(object sender, EventArgs e)
        {
            errorProvider2.Clear();
            if (int.Parse(tbxOverlapZone.Text) > (50 - 2))
            {
                errorProvider2.SetError(tbxOverlapZone, "Error! Value must be less 48.");
            }
            else
            {
                tbFramesCount.Text = GetFramesCount();
            }
        }

        private void tbx_Leave(object sender, EventArgs e)
        {
        }

        private void tbx_Validating(object sender, CancelEventArgs e)
        {
              string sValue = ((TextBox)sender).Text;
            int nValue = 0;
            if ((sValue == null) || sValue.Trim().Equals(""))
            {
                ((TextBox)sender).Text = "0";
                return;
            }
            if (!int.TryParse(sValue, out nValue))
            {
                e.Cancel = true;
            }
         }
     }
}
