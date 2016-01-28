using System;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels;
using WhoWhat.UI.WindowsPhone.Services;

namespace WhoWhat.UI.WindowsPhone.ViewModels.Login
{
    public class FacebookLoginViewModel : AuthViewModelBase
    {
        public FacebookLoginViewModel(
            INavigationService navigationService, 
            AppSettings appSettings, 
            UsersRestService usersRestService)
            : base(navigationService, appSettings, usersRestService)
        {     
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            LoginUrl = new Uri(AppSettings.FacebookAuthUri);
        }

        public override void Navigating(NavigatingEventArgs e)
        {
            if (!e.Uri.Host.Contains("facebook") && !e.Uri.Host.Contains(Host))
            {
                //The user navigated out of facebook.

                e.Cancel = true;
                NavigationService.GoBack();

                return;
            }

            base.Navigating(e);
        }
    }
}
