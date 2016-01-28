using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure;
using WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels;
using WhoWhat.UI.WindowsPhone.Services;

namespace WhoWhat.UI.WindowsPhone.ViewModels.Login
{
    public class VkontakteLoginViewModel : AuthViewModelBase
    {
        public VkontakteLoginViewModel(
            INavigationService navigationService,
            AppSettings appSettings,
            UsersRestService usersRestService)
            : base(navigationService, appSettings, usersRestService)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            LoginUrl = new Uri(AppSettings.VkAuthUri);
        }


        public override void Navigating(NavigatingEventArgs e)
        {
            if (!e.Uri.Host.Contains("vk") && !e.Uri.Host.Contains(Host))
            {
                //The user navigated out of vk.

                e.Cancel = true;
                NavigationService.GoBack();

                return;
            }

            IReadOnlyDictionary<string, string> @params = e.Uri.ParseQueryString();

            if (@params.ContainsKey("error") && @params["error"] == "access_denied")
            {

                //The user hit the Cancel link

                e.Cancel = true;
                NavigationService.GoBack();

                return;
            }

            base.Navigating(e);
        }
    }
}
