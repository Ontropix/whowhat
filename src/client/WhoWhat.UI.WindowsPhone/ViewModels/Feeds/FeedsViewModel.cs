using System.Windows;
using Caliburn.Micro;
using Telerik.Windows.Controls;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Resources;
using WhoWhat.UI.WindowsPhone.Services;

namespace WhoWhat.UI.WindowsPhone.ViewModels.Feeds
{
    public class FeedsViewModel : Conductor<Screen>.Collection.OneActive
    {
        private readonly AppSettings appSettings;
        private readonly INavigationService navigationService;
        private readonly QuestionRestService questionRestService;
        private readonly PushSharpClient pushSharpClient;

        public FeedsViewModel(AppSettings appSettings, INavigationService navigationService, QuestionRestService questionRestService, PushSharpClient pushSharpClient)
        {
            this.appSettings = appSettings;
            this.navigationService = navigationService;
            this.questionRestService = questionRestService;
            this.pushSharpClient = pushSharpClient;
        }

        private bool isAuthenticated;
        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set
            {
                isAuthenticated = value;
                NotifyOfPropertyChange(() => IsAuthenticated);
            }
        }

        private bool isFirstLaunch;
        public bool IsFirstLaunch
        {
            get { return isFirstLaunch; }
            set
            {
                isFirstLaunch = value;
                NotifyOfPropertyChange(() => IsFirstLaunch);
            }
        }

        protected async override void OnInitialize()
        {
            base.OnInitialize();

            IsAuthenticated = appSettings.IsAuthenticated;

            //Live
            QuestionListViewModel live = IoC.Get<QuestionListViewModel>();
            live.LoadQuery = (skip, take) => questionRestService.Live(skip, take);
            live.DisplayName = AppResources.Feed_Live;
            Items.Add(live);

            //Popular
            QuestionListViewModel popular = IoC.Get<QuestionListViewModel>();
            popular.LoadQuery = (skip, take) => questionRestService.Popular(skip, take);
            popular.DisplayName = AppResources.Feed_Popular;
            Items.Add(popular);

            //Unanswered
            QuestionListViewModel unanswered = IoC.Get<QuestionListViewModel>();
            unanswered.LoadQuery = (skip, take) => questionRestService.Unanswered(skip, take);
            unanswered.DisplayName = AppResources.Feed_Unanswered;
            Items.Add(unanswered);

            ActivateItem(live);

            IsFirstLaunch = appSettings.IsFirstLaunch;

            //On first launch
            if (IsFirstLaunch)
            {
                MessageBoxResult result = MessageBox.Show(
                    AppResources.Prompt_Notifications,
                    AppResources.Prompt_Notifications_Title,
                    MessageBoxButton.OKCancel
                );

                if (result == MessageBoxResult.OK)
                {
                    appSettings.IsPushEnabled = true;
                }

                appSettings.IsFirstLaunch = false;
            }

            if (IsAuthenticated && appSettings.IsPushEnabled)
            {
                await pushSharpClient.RegisterForToast();
            }


            //Rate us reminder
            RadRateApplicationReminder reminder = new RadRateApplicationReminder();
            reminder.RecurrencePerUsageCount = 25;
            reminder.AllowUsersToSkipFurtherReminders = true;
            reminder.Notify();
        }


        protected override void OnActivate()
        {
            base.OnActivate();
            if (App.ReloadFeeds)
            {
                App.ReloadFeeds = false;
                foreach (QuestionListViewModel screen in Items)
                {
                    screen.PullToRefresh();
                }
            }
        }

        public void Search()
        {
            navigationService.UriFor<Search.SearchByKeywordViewModel>().Navigate();
        }

        public void Me()
        {
            navigationService.UriFor<UserProfile.UserProfileViewModel>()
                .WithParam(x => x.UserId, appSettings.UserId)
                .Navigate();
        }

        public void AskQuestion()
        {
            if (IsAuthenticated)
            {
                navigationService.UriFor<Question.AskQuestionViewModel>().Navigate();
            }
            else
            {
                navigationService.UriFor<Login.LoginViewModel>().Navigate();
            }
        }

        public void About()
        {
            navigationService.UriFor<AboutViewModel>().Navigate();
        }

        public void Settings()
        {
            navigationService.UriFor<SettingsViewModel>().Navigate();
        }
    }
}
