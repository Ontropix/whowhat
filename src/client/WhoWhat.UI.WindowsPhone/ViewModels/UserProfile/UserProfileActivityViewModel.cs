using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels;
using WhoWhat.UI.WindowsPhone.Resources;
using WhoWhat.UI.WindowsPhone.Services;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.ViewModels.UserProfile
{
    public class UserProfileActivityViewModel : TaskViewModel
    {
        private readonly UsersRestService usersRestService;
        private readonly INavigationService navigationService;
        private readonly AppSettings appSettings;

        public UserProfileActivityViewModel(UsersRestService usersRestService, INavigationService navigationService, AppSettings appSettings)
        {
            this.usersRestService = usersRestService;
            this.navigationService = navigationService;
            this.appSettings = appSettings;
            DisplayName = AppResources.Profile_Activity;
        }

        protected async override void OnInitialize()
        {
            base.OnInitialize();

            //The user has already seen the notifications
            appSettings.NotificationsCount = 0;

            await RunTaskAsync(async () =>
            {
                NotificationsResponse response = await usersRestService.NotificationsMe(0, 100); //Get 100 latest
                Notifications = new ObservableCollection<Notification>(response.Notifications.OrderByDescending(n => n.CreatedAt));
            });
        }


        private ObservableCollection<Notification> notifications;
        public ObservableCollection<Notification> Notifications
        {
            get { return notifications; }
            set
            {
                if (Equals(value, notifications)) return;
                notifications = value;
                NotifyOfPropertyChange(() => Notifications);
            }
        }

        public void OpenQuestion(Notification notification)
        {
            navigationService.UriFor<Question.ViewQuestionViewModel>()
                .WithParam(vm => vm.QuestionId, notification.QuestionId)
                .WithParam(vm => vm.HighlightAnswer, true)
           .Navigate();
        }

        public void Open(Notification notification)
        {
            switch (notification.Type)
            {
                case NotificationType.AnswerMarkedAsAccepted:
                case NotificationType.AnswerVotedDown:
                case NotificationType.AnswerVotedUp:

                    navigationService.UriFor<Question.ViewQuestionViewModel>()
                                     .WithParam(vm => vm.FocusAnswer, true)
                                     .WithParam(vm => vm.QuestionId, notification.QuestionId)
                                     .WithParam(vm => vm.HighlightAnswer, true)
                                     .Navigate();
                    break;
                case NotificationType.QuestionAnswered:
                case NotificationType.QuestionVotedDown:
                case NotificationType.QuestionVotedUp:

                    navigationService.UriFor<Question.ViewQuestionViewModel>()
                                     .WithParam(vm => vm.QuestionId, notification.QuestionId)
                                     .WithParam(vm => vm.HighlightAnswer, true)
                                     .Navigate();
                    break;

            }


        }
    }
}
