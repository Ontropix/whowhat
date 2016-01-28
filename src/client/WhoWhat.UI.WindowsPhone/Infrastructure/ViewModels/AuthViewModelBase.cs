using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Services;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels
{
    public class AuthViewModelBase : Screen
    {

        protected const string Host = "whowhat.azurewebsites.net";

        protected readonly AppSettings AppSettings;
        private readonly UsersRestService usersRestService;
        protected readonly INavigationService NavigationService;

        public AuthViewModelBase(INavigationService navigationService, AppSettings appSettings, UsersRestService usersRestService)
        {
            this.NavigationService = navigationService;
            this.AppSettings = appSettings;
            this.usersRestService = usersRestService;
        }

        private Uri loginUrl;
        public Uri LoginUrl
        {
            get { return loginUrl; }
            set
            {
                if (value == loginUrl) return;
                loginUrl = value;
                NotifyOfPropertyChange(() => LoginUrl);
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (value.Equals(isBusy)) return;
                isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        private CookieCollection cookieCollection;
        public CookieCollection CookieCollection
        {
            get { return cookieCollection; }
            set
            {
                if (Equals(value, cookieCollection)) return;
                cookieCollection = value;
                NotifyOfPropertyChange(() => CookieCollection);
            }
        }

        public virtual async void Navigated(NavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigated to " + e.Uri);

            if (e.Uri == new Uri(AppSettings.AuthSucceessUri))
            {
                await OnAuthenticated();
            }
        }

        protected async Task OnAuthenticated()
        {
            //Success, save cookies
            AppSettings.SsId = CookieCollection["ss-id"].Value;
            AppSettings.SsPid = CookieCollection["ss-pid"].Value;

            ProfileResponse response = await usersRestService.ProfileMe();
            AppSettings.UserId = response.UserId;

            JournalEntry prevPage = NavigationService.BackStack.ElementAtOrDefault(1);

            //Clear back stack
            while (NavigationService.BackStack.Count() != 0)
            {
                NavigationService.RemoveBackEntry();
            }

            if (prevPage != null)
            {
                //If an anonymous user taps the "Ask Question" button, 
                //he should be redirected to the "Ask Question" Page after successful login 
                //The behaviour should be the same for voting and answering a question.
                NavigationService.Navigate(prevPage.Source);
            }
            else
            {
                NavigationService.UriFor<WindowsPhone.ViewModels.Feeds.FeedsViewModel>().Navigate();
            }


            //Remove this (auth) page
            NavigationService.RemoveBackEntry();
        }

        public virtual void Navigating(NavigatingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigating to " + e.Uri);

            IReadOnlyDictionary<string, string> @params = e.Uri.ParseQueryString();

            if (@params.ContainsKey("code"))
            {
                IsBusy = true;
                return;
            }

            if (e.Uri == new Uri(AppSettings.AuthSucceessUri))
            {
                IsBusy = true;
            }
        }
    }
}
