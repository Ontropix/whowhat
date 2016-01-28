using System.Linq;
using Caliburn.Micro;
using WhoWhat.UI.WindowsPhone.Core;

namespace WhoWhat.UI.WindowsPhone.ViewModels.UserProfile
{
    public class UserProfileViewModel : Conductor<Screen>.Collection.OneActive
    {
        private readonly AppSettings appSettings;

        //Set from Navigation
        public string UserId { get; set; }

        //Set from Navigation
        public bool IsNotificationsActive { get; set; }

        public UserProfileViewModel(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            UserProfileInfoViewModel info = IoC.Get<UserProfileInfoViewModel>();
            info.UserId = UserId;
            Items.Add(info);

            UserProfileQuestionsViewModel questions = IoC.Get<UserProfileQuestionsViewModel>();
            questions.UserId = UserId;
            Items.Add(questions);

            UserProfileAnswersViewModel answers = IoC.Get<UserProfileAnswersViewModel>();
            answers.UserId = UserId;
            Items.Add(answers);

            if (appSettings.IsAuthenticated && UserId == appSettings.UserId)
            {
                //The user can only see only his own notifications.
                var notifications = IoC.Get<UserProfileActivityViewModel>();
                Items.Add(notifications);

                if (IsNotificationsActive)
                {
                    ActivateItem(notifications);
                    return;
                }
            }

            ActivateItem(info);
        }

        public void OpenNotificationTab()
        {
            var notifications = Items.FirstOrDefault(x => x.GetType() == typeof (UserProfileActivityViewModel));
            if (notifications != null)
            {
                ActivateItem(notifications);
            }
        }
    }
}
