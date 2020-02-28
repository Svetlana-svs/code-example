using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DC22.Shell.Controls
{
    interface IHelp
    {
        void SetFocus(FocusControl focusControl);
        void SetFocusedControl(object sender);

        void GetHelp(object sender);
    }

    public enum FocusControl
    {
        FirstControl,
        LastFocusedControl
    }
}
