using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Wlds
{
    public partial class ParametersForm : Form
    {
        public int timerInterval;

        public ParametersForm()
        {
            InitializeComponent();
            btnCancel.CausesValidation = false;
        }

        private void form_Load(object sender, EventArgs e)
        {
            m_nTimerInterval.Value = timerInterval;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            timerInterval = (int)m_nTimerInterval.Value;
        }
    }
}
