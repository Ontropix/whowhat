using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Services;

namespace WhoWhat.UI.WindowsPhone.ViewModels.Search
{
    public class SearchByKeywordViewModel : QuestionListViewModel
    {
        private readonly AppSettings appSettings;

        public SearchByKeywordViewModel(
            SearchRestService searchRestService, 
            INavigationService navigationService, 
            AppSettings appSettings): base(navigationService)
        {
            this.appSettings = appSettings;
            this.LoadQuery = (skip, take) => searchRestService.SearchByKeyword(SearchText, skip, take);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            HasResults = true;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            if (Questions == null)
            {
                Questions = new ObservableCollection<Services.Model.Question>();
            }
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (value == searchText) return;
                searchText = value;
                NotifyOfPropertyChange(() => SearchText);
                NotifyOfPropertyChange(() => IsSearchEnabled);
            }
        }


        public async void Search()
        {
            CurrentIndex = 0;
            Questions.Clear();
            IsVirtualizationEnabledProperty = true;
            await LoadRequestedData();
        }

        protected async override Task LoadQuestions(bool refresh = false)
        {
            await base.LoadQuestions();
            HasResults = Questions.Count != 0;
        }

        public void Search(KeyEventArgs args)
        {
            if (args.Key == Key.Enter && SearchText.Length > 2)
            {
                //Hide the keyboard
                ((PhoneApplicationPage)this.GetView()).Focus();

                Search();
            }
        }

        public void Clear()
        {
            SearchText = string.Empty;
            HasResults = true;
            base.Questions.Clear();
        }

        public bool IsSearchEnabled
        {
            get { return !string.IsNullOrEmpty(SearchText) && SearchText.Length > 2 && !IsBusy; }
        }

        private bool hasResults;
        public bool HasResults
        {
            get { return hasResults; }
            set
            {
                hasResults = value;
                NotifyOfPropertyChange(() => HasResults);
            }
        }

        public async override Task LoadRequestedData()
        {
            if (!IsSearchEnabled) return;
            await base.LoadRequestedData();
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
    }
}
