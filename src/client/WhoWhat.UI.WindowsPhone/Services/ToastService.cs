using System.Windows.Media;
using Coding4Fun.Toolkit.Controls;
using WhoWhat.UI.WindowsPhone.Resources;

namespace WhoWhat.UI.WindowsPhone.Services
{
    public class ToastService
    {
        public void ShowError(string message, string title = null)
        {
            ToastPrompt toast = new ToastPrompt();
            toast.Title = !string.IsNullOrEmpty(title) ? title : AppResources.ApplicationTitle;
            toast.Message = message;
            toast.Background = new SolidColorBrush(Colors.Red);
            toast.Show();
        }

        public void ShowWarning(string message, string title = null)
        {
            ToastPrompt toast = new ToastPrompt();
            toast.Title = !string.IsNullOrEmpty(title) ? title : AppResources.ApplicationTitle;
            toast.Message = message;
            toast.Background = new SolidColorBrush(Colors.Orange);
            toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
            toast.Show();
        }
    }
}
