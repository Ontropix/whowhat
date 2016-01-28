using System.Linq;
using Caliburn.Micro;
using WhoWhat.UI.WindowsPhone.Core;

namespace WhoWhat.UI.WindowsPhone.Services
{
    public class SingOuter
    {
        private readonly INavigationService navigationService;
        private readonly AppSettings appSettings;

        public SingOuter(INavigationService navigationService, AppSettings appSettings)
        {
            this.navigationService = navigationService;
            this.appSettings = appSettings;
        }

        public void SignOut()
        {
            appSettings.SsId = null;
            appSettings.SsPid = null;
            appSettings.UserId = null;
            appSettings.NotificationsCount = 0;

            //Clear back stack
            while (navigationService.BackStack.Count() != 0)
            {
                navigationService.RemoveBackEntry();
            }

            //Navigate to the main view
            navigationService.UriFor<ViewModels.Feeds.FeedsViewModel>().Navigate();
        }
    }
}
