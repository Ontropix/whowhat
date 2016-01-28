using Caliburn.Micro;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Resources;
using WhoWhat.UI.WindowsPhone.Services;

namespace WhoWhat.UI.WindowsPhone.ViewModels.UserProfile
{
    public class UserProfileQuestionsViewModel : QuestionListViewModel
    {
        private readonly AppSettings appSettings;
        public string UserId { get; set; }

        public bool Me
        {
            get { return UserId == appSettings.UserId; }
        } 

        public UserProfileQuestionsViewModel(UsersRestService usersRestService, AppSettings appSettings, INavigationService navigationService)
            :base(navigationService)
        {
            this.appSettings = appSettings;
            DisplayName = AppResources.Profile_Questions;

            this.LoadQuery = async (skip, take) =>
            {
                if (Me)
                {
                    return await usersRestService.QuestionsMe(skip, take);
                }
                else
                {
                    return await usersRestService.Questions(UserId, skip, take);
                }
            };
        }
    }
}
