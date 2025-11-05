using System;
using System.Windows.Forms;

namespace EX.Desktop.Helpers
{
    public static class UIHelper
    {

        public static void ThreadInvoke(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

    }
}
