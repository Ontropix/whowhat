using Caliburn.Micro;
using Microsoft.Phone.Controls;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels;
using WhoWhat.UI.WindowsPhone.Resources;
using WhoWhat.UI.WindowsPhone.Services;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.ViewModels.Question
{
    public class AnswerQuestionViewModel : TaskViewModel
    {
        private readonly INavigationService navigationService;
        private readonly QuestionRestService questionRestService;
        private readonly AppSettings appSettings;
        private readonly ToastService toastService;

        //From navigation
        public Services.Model.Question Question { get; set; }

        public AnswerQuestionViewModel(
            INavigationService navigationService,
            QuestionRestService questionRestService,
            AppSettings appSettings,
            ToastService toastService)
        {
            this.navigationService = navigationService;
            this.questionRestService = questionRestService;
            this.appSettings = appSettings;
            this.toastService = toastService;
        }

        private string answerBody;
        public string AnswerBody
        {
            get { return answerBody; }
            set
            {
                answerBody = value;
                NotifyOfPropertyChange(() => AnswerBody);
            }
        }

        public async void Answer()
        {
            if (string.IsNullOrEmpty(AnswerBody))
            {
                toastService.ShowWarning( 
                    AppResources.Validation_AnswerBodyRequired
                );

                return;
            }

            if (AnswerBody.Length < appSettings.MinAnswerLength)
            {
                toastService.ShowWarning(string.Format(
                    AppResources.Validation_BodyValidationFrmt,
                    appSettings.MinAnswerLength,
                    AnswerBody.Length
                ));

                return;
            }


            //Hide the keyboard
            ((PhoneApplicationPage)this.GetView()).Focus();

            await RunTaskAsync(async () =>
                {
                    IsBusy = true;
                    await questionRestService.AnswerQuestion(Question.QuestionId, AnswerBody);
                    navigationService.GoBack();
                    IsBusy = false;
                },
                (value) => { },
                exception => IsBusy = false
           );
        }
    }
}
