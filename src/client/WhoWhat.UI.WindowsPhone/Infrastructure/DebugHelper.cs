using System;
using System.Windows;

namespace WhoWhat.UI.WindowsPhone.Infrastructure
{
    public static class DebugHelper
    {
        public static void RevealException(Exception ex)
        {
#if DEBUG
            string message = string.Format("{0} -> {1}", ex.Message, ex.StackTrace);
            MessageBox.Show(message);
#endif
        }
    }
}
