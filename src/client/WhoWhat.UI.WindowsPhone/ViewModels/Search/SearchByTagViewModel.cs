using System.Collections.ObjectModel;
using Caliburn.Micro;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Services;

namespace WhoWhat.UI.WindowsPhone.ViewModels.Search
{
    public class SearchByTagViewModel : QuestionListViewModel
    {
        private readonly AppSettings appSettings;
        public string Tag { get; set; }

        public SearchByTagViewModel(
            SearchRestService searchRestService, 
            INavigationService navigationService, 
            AppSettings appSettings)
            : base(navigationService)
        {
            this.appSettings = appSettings;
            this.LoadQuery = (skip, take) => searchRestService.SearchByTag(Tag, skip, take);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            SetTitle();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            if (Questions == null)
            {
                Questions = new ObservableCollection<Services.Model.Question>();
            }
        }

        private void SetTitle()
        {
            Title = string.Format("Results for #" + Tag);
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (value == title) return;
                title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        public void Me()
        {
            NavigationService.UriFor<UserProfile.UserProfileViewModel>()
                .WithParam(x => x.UserId, appSettings.UserId)
                .Navigate();
        }

        public void AskQuestion()
        {
            if (IsAuthenticated)
            {
                NavigationService.UriFor<Question.AskQuestionViewModel>().Navigate();
            }
            else
            {
                NavigationService.UriFor<Login.LoginViewModel>().Navigate();
            }
        }

        public void About()
        {
            NavigationService.UriFor<AboutViewModel>().Navigate();
        }

        public void Settings()
        {
            NavigationService.UriFor<SettingsViewModel>().Navigate();
        }

        public async override void SearchTag(Controls.TagTappedEvenArgs args)
        {
            Tag = args.Tag;
            SetTitle();

            Questions = new ObservableCollection<Services.Model.Question>();
            CurrentIndex = 0;
            IsVirtualizationEnabledProperty = true;
            await LoadRequestedData();
        }
    }
}
