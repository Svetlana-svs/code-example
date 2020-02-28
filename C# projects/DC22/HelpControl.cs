using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DC22.AccelAF.PL;
using DC22.Shell.PL.Shock;
using System.Threading;
using DC22.Shell.Controls.Measures;

namespace DC22.Shell.Help
{
    public partial class HelpControl : UserControl, ITheme
    {

        public HelpControl()
        {
            InitializeComponent();
        }

        public void SetFirstFocusControl()
        {
            m_txtHelp.Focus();
        }

        public void SetFocus()
        {
            m_txtHelp.Focus();
        }

        public void SetFocusedControl(object sender)
        {
        }

        public string Text
        {
            set
            {
                m_txtHelp.Text = value;
            }
        }

        #region ITheme Members

        public void ApplyTheme()
        {
            ThemeUtils.ApplyColors(ThemeUtils.ActiveBackColor, ThemeUtils.ActiveForeColor);
            ThemeUtils.ApplyColors(ThemeUtils.ActiveBackColor, ThemeUtils.ActiveForeColor);
            m_txtHelp.BackColor = Color.White;
        }

        #endregion
    }
}
