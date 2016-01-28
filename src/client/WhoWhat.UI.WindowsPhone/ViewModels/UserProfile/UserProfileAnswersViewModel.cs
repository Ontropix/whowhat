using Caliburn.Micro;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Resources;
using WhoWhat.UI.WindowsPhone.Services;

namespace WhoWhat.UI.WindowsPhone.ViewModels.UserProfile
{
    public class UserProfileAnswersViewModel: QuestionListViewModel
    {
        private readonly AppSettings appSettings;
        public string UserId { get; set; }

        public bool Me
        {
            get { return UserId == appSettings.UserId; }
        }

        public UserProfileAnswersViewModel(UsersRestService usersRestService, AppSettings appSettings, INavigationService navigationService)
            :base(navigationService)
        {
            this.appSettings = appSettings;
            DisplayName = AppResources.Profile_Answers;

            this.LoadQuery = async (skip, take) =>
            {
                if (Me)
                {
                    return await usersRestService.AnswersMe(skip, take);
                }
                else
                {
                    return await usersRestService.Answers(UserId, skip, take);
                }
            };
        }

        public override void QuetionDetails(Services.Model.Question summary)
        {
            NavigationService.UriFor<Question.ViewQuestionViewModel>()
                .WithParam(vm => vm.FocusAnswer, true)
                .WithParam(vm => vm.QuestionId, summary.QuestionId)
                .WithParam(vm => vm.HighlightAnswer, true)
                .Navigate();
        }
    }
}
