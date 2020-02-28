using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DC22.Shell.PL;
using System.Threading;
using DC22.Shell.Controls.Common;
using DC22.Shell.Controls;

namespace DC22.Shell.Help
{
    public partial class HelpPage : UserControl, IPage
    {
        public HelpPage()
        {
            InitializeComponent();

            ApplyTheme();
            SignKeyProcessIter(this);
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
                m_helpControl.Text = (string)value;
                SetFocus(FocusControl.FirstControl);
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
            m_helpControl.Focus();
        }
 
        public void SetFocus(FocusControl focusControl)
        {
            m_helpControl.Focus();
        }

        public void SetFocusedControl()
        {
        }

        public static string Id
        {
            get { return "help"; }
        }
        #endregion IPage

        public void ApplyTheme()
        {
            ThemeUtils.ApplyColors(ThemeUtils.ActiveBackColor, ThemeUtils.ActiveForeColor, this);
            foreach (Control control in this.Controls)
            {
                if (control is CItemsUpDown)
                {
                    ((CItemsUpDown)control).BackColor = ThemeUtils.ActiveBackColor;
                    ((CItemsUpDown)control).ForeColor = ThemeUtils.ActiveForeColor;
                }
                if (control is Label || control is Button)
                {
                    control.BackColor = ThemeUtils.ActiveBackColor;
                    control.ForeColor = ThemeUtils.ActiveForeColor;
                }
                if (control is CNumericUpDown)
                {
                    ((CNumericUpDown)control).BackColor = ThemeUtils.ActiveBackColor;
                    ((CNumericUpDown)control).ForeColor = ThemeUtils.ActiveForeColor;
                }
            }
            ThemeUtils.ApplyColors(ThemeUtils.SpecialBackColor, ThemeUtils.SpecialForeColor);
            
        }

        private void SignKeyProcessIter(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                SignKeyProcessIter(ctrl);
            }
        }

        void ctrl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Exit(this, EventArgs.Empty);
                    break;
            }
        }
    }
}
