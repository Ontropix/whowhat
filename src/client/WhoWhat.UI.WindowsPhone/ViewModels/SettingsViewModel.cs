using System;
using Caliburn.Micro;
using Microsoft.Phone.Tasks;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels;
using WhoWhat.UI.WindowsPhone.Services;

namespace WhoWhat.UI.WindowsPhone.ViewModels
{
    public class SettingsViewModel : WhoWhatViewModel
    {
        private readonly PushSharpClient pushSharpClient;
        private readonly ToastService toastService;
        private readonly AppSettings appSettings;
        private readonly IEventAggregator eventAggregator;
        private readonly SingOuter singOuter;

        public SettingsViewModel(AppSettings appSettings, PushSharpClient pushSharpClient, 
            ToastService toastService, IEventAggregator eventAggregator, SingOuter singOuter)
        {
            this.appSettings = appSettings;
            this.pushSharpClient = pushSharpClient;
            this.toastService = toastService;
            this.eventAggregator = eventAggregator;
            this.singOuter = singOuter;
        }

        public bool IsPushEnabled
        {
            get { return appSettings.IsPushEnabled; }
            set
            {
                appSettings.IsPushEnabled = value;
                NotifyOfPropertyChange(() => IsPushEnabled);
                if (value)
                {
                    RegisterForToast();
                }
                else
                {
                    UnregisterFromToast();
                }
            }
        }

        private async void UnregisterFromToast()
        {
            try
            {
                await pushSharpClient.UnregisterFromToast();
            }
            catch (Exception ex)
            {
                FlurryWP8SDK.Api.LogError("Failed to unregister from push notification", ex);
                toastService.ShowError("Unable to unregister");
            }
        }

        private async void RegisterForToast()
        {
            await pushSharpClient.RegisterForToast();
        }

        public void SendFeedback()
        {
            eventAggregator.RequestTask<EmailComposeTask>(task =>
            {
                task.To = "whowhat@code9.biz";
                task.Subject = "Feedback for WhoWhat";
            });
        }

        public void SingOut()
        {
            singOuter.SignOut();
        }

        public void RateUs()
        {
            eventAggregator.RequestTask<MarketplaceReviewTask>();
        }
    }
}
