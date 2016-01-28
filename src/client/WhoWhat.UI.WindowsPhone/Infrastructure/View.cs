using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using WhoWhat.UI.WindowsPhone.ViewModels.Feeds;
using WhoWhat.UI.WindowsPhone.Views.Feeds;

namespace WhoWhat.UI.WindowsPhone.Infrastructure
{
    public class View : PhoneApplicationPage
    {
        public View()
        {
            this.Loaded += (s, e) =>
            {
                if (this.ApplicationBar != null)
                {
                    this.ApplicationBar.BackgroundColor = (Color)Application.Current.Resources["WW.Color.Accent"];
                    this.ApplicationBar.ForegroundColor = Colors.White;
                }
            };

            this.BackKeyPress += OnBackKeyPress;
        }

        private void OnBackKeyPress(object sender, CancelEventArgs cancelEventArgs)
        {
           INavigationService navigationService =  IoC.Get<INavigationService>();

           //The user should close the app only from FeedsView - main screen of the app 
            if (!navigationService.CanGoBack)
            {
                if (!(this is FeedsView))
                {
                    navigationService.UriFor<FeedsViewModel>().Navigate();

                    //Remove this page
                    navigationService.RemoveBackEntry();
                }
            }
        }
    }
}
