using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Caliburn.Micro;
using WhoWhat.UI.WindowsPhone.Controls;
using WhoWhat.UI.WindowsPhone.Infrastructure;
using WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels;
using WhoWhat.UI.WindowsPhone.Resources;
using WhoWhat.UI.WindowsPhone.Services;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.ViewModels
{
    public delegate Task<QuestionSummaryResponse> LoadQuery(int skip, int take);

    public class QuestionListViewModel : WhoWhatViewModel
    {
        public const int PAGE_SIZE = 20;

        protected readonly INavigationService NavigationService;

        protected int CurrentIndex = 0;

        public LoadQuery LoadQuery { get; set; }

        public QuestionListViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            IsVirtualizationEnabledProperty = true;
        }

        protected async override void OnActivate()
        {
            base.OnActivate();
            if (Questions == null)
            {
                IsInitialLoading = true;
                Questions = new OrderedObservableCollection<Services.Model.Question>((x, y) => y.CreatedAt.CompareTo(x.CreatedAt));
                await LoadRequestedData();
            }
        }

        private bool isInitialLoading;
        public bool IsInitialLoading
        {
            get { return isInitialLoading; }
            set
            {
                isInitialLoading = value;
                NotifyOfPropertyChange(() => IsInitialLoading);
            }
        }

        private bool isVirtualizationEnabledProperty;
        public bool IsVirtualizationEnabledProperty
        {
            get { return isVirtualizationEnabledProperty; }
            set
            {
                isVirtualizationEnabledProperty = value;
                NotifyOfPropertyChange(() => IsVirtualizationEnabledProperty);
            }
        }

        private bool isPullToRefreshLoadingCompleted;
        public bool IsPullToRefreshLoadingCompleted
        {
            get { return isPullToRefreshLoadingCompleted; }
            set
            {
                isPullToRefreshLoadingCompleted = value;
                NotifyOfPropertyChange(() => IsPullToRefreshLoadingCompleted);
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

        public async virtual Task LoadRequestedData()
        {
            if (this.IsBusy)
            {
                return;
            }

            IsBusy = true;

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                await LoadQuestions();
            }
            else
            {
                IsInitialLoading = false;
                IoC.Get<ToastService>().ShowError(AppResources.Message_NoInternet);
            }

            this.IsBusy = false;
        }

        protected virtual async Task LoadQuestions(bool merge = false)
        {
            if (!IsVirtualizationEnabledProperty && !merge) return;

            try
            {
                if (merge)
                {
                    CurrentIndex = 0;
                }

                QuestionSummaryResponse response = await LoadQuery(CurrentIndex, PAGE_SIZE);

                if (merge)
                {
                    MergeQuestions(response.Questions);
                    CurrentIndex = response.Questions.Count; 
                }
                else
                {
                    foreach (var question in response.Questions)
                    {
                        Questions.Add(question);
                    }

                    CurrentIndex += response.Questions.Count;       
                }

                if (response.Questions.Count < PAGE_SIZE)
                {
                    //All data loaded
                    IsVirtualizationEnabledProperty = false;
                }

                IsInitialLoading = false;
            }
            catch (WebException)
            {
                IsInitialLoading = false;
                IoC.Get<ToastService>().ShowError(AppResources.Message_NoInternet);
            }
            catch (RestException ex)
            {
                DebugHelper.RevealException(ex);

                IoC.Get<ToastService>()
                    .ShowError(ex.StatusCode == HttpStatusCode.NotFound
                        ? AppResources.Message_NoInternet
                        : AppResources.Message_ServerError
                );

                FlurryWP8SDK.Api.LogError("QuestionListViewModel -> RestException", ex);
            }
            catch (System.Exception ex)
            {
                DebugHelper.RevealException(ex);

                IsInitialLoading = false;
                IoC.Get<ToastService>().ShowError(AppResources.Message_Unspecified);
                FlurryWP8SDK.Api.LogError("QuestionListViewModel -> Unspecified", ex);
            }
        }


        private void MergeQuestions(IList<Services.Model.Question> remote)
        {
            List<string> localIds = Questions.Select(q => q.QuestionId).ToList();
            List<string> remoteIds = remote.Select(q => q.QuestionId).ToList();

            //Get new questions
            List<Services.Model.Question> @new = remote.Where(question => !localIds.Contains(question.QuestionId)).ToList();
            List<Services.Model.Question> @removed = Questions.Where(question => !remoteIds.Contains(question.QuestionId)).ToList();

            //Remove
            foreach (Services.Model.Question question in @removed)
            {
                Questions.Remove(question);
            }

            //Update
            foreach (Services.Model.Question localQuestion in Questions)
            {
                Services.Model.Question remoteQuestion = remote.FirstOrDefault(q => q.QuestionId == localQuestion.QuestionId);
                if (remoteQuestion != null)
                {
                    UpdateQuestion(localQuestion, remoteQuestion);
                }
            }

            //Add
            foreach (Services.Model.Question question in @new)
            {
                Questions.Add(question);
            }

            //Remove questions with index after > remote.Count, there will be reloaded during scrolling.
            List<Services.Model.Question> clone = Questions.ToList();
            for (int i = remote.Count; i < Questions.Count; i ++)
            {
                Services.Model.Question question = clone[i];
                Questions.Remove(question);
            }
        }

        private void UpdateQuestion(Services.Model.Question localQuestion, Services.Model.Question remoteQuestion)
        {
            //Workaround: to notify CreatedAt changed
            localQuestion.CreatedAt = System.DateTime.MinValue;

            localQuestion.Author = remoteQuestion.Author;
            localQuestion.Body = remoteQuestion.Body;
            localQuestion.ImageUri = remoteQuestion.ImageUri;
            localQuestion.AnswersCount = remoteQuestion.AnswersCount;
            localQuestion.CreatedAt = remoteQuestion.CreatedAt;
            localQuestion.IsResolved = remoteQuestion.IsResolved;
            localQuestion.Rating = remoteQuestion.Rating;
            localQuestion.Thumbnail = remoteQuestion.Thumbnail;
            localQuestion.Tags = remoteQuestion.Tags;
            localQuestion.VotesCount = remoteQuestion.VotesCount;
        }

        private ObservableCollection<Services.Model.Question> questions;
        public ObservableCollection<Services.Model.Question> Questions
        {
            get { return questions; }
            set
            {
                questions = value;
                NotifyOfPropertyChange(() => Questions);
            }
        }

        public async void PullToRefresh()
        {
            IsPullToRefreshLoadingCompleted = false;
            await LoadQuestions(merge: true);

            //Hide the pull to refresh indicator
            IsPullToRefreshLoadingCompleted = true;

            //Enable loading the rest of the pages
            IsVirtualizationEnabledProperty = true; //For paging
        }

        public virtual void QuetionDetails(Services.Model.Question summary)
        {
            NavigationService.UriFor<Question.ViewQuestionViewModel>()
                .WithParam(vm => vm.QuestionId, summary.QuestionId)
                .Navigate();
        }

        public virtual void OpenUserProfile(Services.Model.Question summary)
        {
            if (IsAuthenticated)
            {
                NavigationService.UriFor<UserProfile.UserProfileViewModel>()
                                 .WithParam(vm => vm.UserId, summary.Author.UserId)
                                 .Navigate();
            }
            else
            {

                NavigationService.UriFor<Login.LoginViewModel>()
                                 .Navigate();
            }

        }

        public virtual void SearchTag(TagTappedEvenArgs args)
        {
            NavigationService.UriFor<Search.SearchByTagViewModel>()
               .WithParam(vm => vm.Tag, args.Tag)
               .Navigate();
        }
    }
}
