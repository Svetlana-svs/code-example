using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace SM3G
{
    public delegate void DelegateDetectorPanelLocationChangedEventHandler(object sender, int location);

    public partial class DetectorPanel : UserControl
    {
        public event DelegateDetectorPanelLocationChangedEventHandler ChartDetectorPanelLocationChanged;
        
        public DetectorPanel()
        {
            InitializeComponent();
        }

        public Int16 DetectorId
        {
            get;
            set;
        }

        public DATA_TYPE Type
        {
            get;
            set;
        }

        public string Title
        {
            get
            {
                return this.m_lTitle.Text;
            }
            set
            {
                this.m_lTitle.Text = value;
            }
        }

        public string DetectorName
        {
            get;
            set;
        }

        public void SetDetectorFields(float[] dataBuf, Detector detector)
        {
            foreach (String description in detector.Description)
            {
                this.m_cbxDescription.Items.Add(description);
            }
        }

        public void m_btnWapFactor_Click(object sender, EventArgs e)
        {
            if (pWapFactor.Visible)
            {
                pWapFactor.Visible = false;
                pDetail.Height = 87; //84
                this.Height -= 65;
                m_btnWapFactor.Text += "...";
                this.panel.Visible = false;
                ChartDetectorPanelLocationChanged(this, -65);
            }
            else
            {
                pWapFactor.Visible = true;
                pDetail.Height = 152; // 149;
                this.Height += 65;
                this.panel.Visible = true;
                if (m_btnWapFactor.Text.Length > 3)
                {
                    m_btnWapFactor.Text = m_btnWapFactor.Text.Substring(0, m_btnWapFactor.Text.Length - 3);
                }
                ChartDetectorPanelLocationChanged(this, 65);
            }
        }

        public void m_btnComposite_Click(object sender, EventArgs e)
        {
            if (this.pDetail.Visible)
            {
                this.pDetail.Visible = false;
                this.Height = 55;
                ChartDetectorPanelLocationChanged(this, -this.pDetail.Height + 4);
            }
            else
            {
                this.pDetail.Visible = true;
                this.Height = this.pDetail.Height + 51;
                ChartDetectorPanelLocationChanged(this, this.pDetail.Height - 4);
            }
        }

        public bool isPanelsVisible()
        {
            return this.pDetail.Visible && !this.pWapFactor.Visible;
        }
    }
}
