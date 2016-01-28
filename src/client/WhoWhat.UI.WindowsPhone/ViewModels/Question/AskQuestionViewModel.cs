using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels;
using WhoWhat.UI.WindowsPhone.Resources;
using WhoWhat.UI.WindowsPhone.Services;
using WhoWhat.UI.WindowsPhone.Services.Model;
using TaskResult = Microsoft.Phone.Tasks.TaskResult;

namespace WhoWhat.UI.WindowsPhone.ViewModels.Question
{
    public class AskQuestionViewModel : TaskViewModel, IHandle<TaskCompleted<PhotoResult>>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly INavigationService navigationService;
        private readonly QuestionRestService questionRestService;
        private readonly AppSettings appSettings;
        private readonly ToastService toastService;

        public AskQuestionViewModel(
            IEventAggregator eventAggregator, INavigationService navigationService,
            QuestionRestService questionRestService, AppSettings appSettings, ToastService toastService)
        {
            this.eventAggregator = eventAggregator;
            this.navigationService = navigationService;
            this.questionRestService = questionRestService;
            this.appSettings = appSettings;
            this.toastService = toastService;
        }

        protected override void OnActivate()
        {
            eventAggregator.Subscribe(this);
            base.OnActivate();
        }

        protected override void OnDeactivate(bool close)
        {
            eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        public void ChoosePhoto()
        {
            eventAggregator.RequestTask<PhotoChooserTask>(c =>
            {
                c.ShowCamera = true;
                c.PixelHeight = 600;
                c.PixelWidth = 600;
            });
        }

        public void Handle(TaskCompleted<PhotoResult> message)
        {
            if (message.Result.TaskResult == TaskResult.OK)
            {
                byte[] bytes = new byte[message.Result.ChosenPhoto.Length];
                message.Result.ChosenPhoto.Read(bytes, 0, bytes.Length);
                ImageBytes = bytes;
            }
        }

        private byte[] imageBytes;
        public byte[] ImageBytes
        {
            get { return imageBytes; }
            set
            {
                if (Equals(value, imageBytes)) return;
                imageBytes = value;
                NotifyOfPropertyChange(() => ImageBytes);
            }
        }

        private string body = string.Empty;
        public string Body
        {
            get { return body; }
            set
            {
                if (value == body) return;
                body = value;
                NotifyOfPropertyChange(() => Body);
            }
        }

        #region Validation

        private bool Validate()
        {

            if (string.IsNullOrEmpty(Body))
            {
                toastService.ShowWarning(
                    AppResources.Validation_QuestionBodyRequired
                );

                return false;
            }

            Body = Body.Trim();

            if (Body.Length < appSettings.MinQuestionLength)
            {
                toastService.ShowWarning(string.Format(
                    AppResources.Validation_BodyValidationFrmt,
                    appSettings.MinQuestionLength,
                     Body.Length
                ));

                return false;
            }

            if (ImageBytes == null || ImageBytes.Length == 0)
            {
                toastService.ShowWarning(AppResources.Validation_Image);
                return false;
            }

            if (Tags.Count == 0)
            {
                toastService.ShowWarning(AppResources.Validation_TagRequired);
                return false;
            }

            return true;
        }

        #endregion

        private List<string> tags = new List<string>();
        public List<string> Tags
        {
            get { return tags; }
            set
            {
                if (Equals(value, tags)) return;
                tags = value;
                NotifyOfPropertyChange(() => Tags);
            }
        }

        public void Cancel()
        {
            navigationService.GoBack();
        }

        public async void Ask()
        {
            //Removal of focus from the TagsLine to the Page will force the bind.
            ((PhoneApplicationPage)this.GetView()).Focus();

            // Wait till the next UI thread tick so that the binding gets updated
            await Task.Yield();


            //Running on emulator
            if (Microsoft.Devices.Environment.DeviceType == Microsoft.Devices.DeviceType.Emulator)
            {
                ImageBytes = new byte[]
                {
                    0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a, 0x00, 0x00, 0x00, 0x0d, 0x49, 0x48, 0x44, 0x52,
                    0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x08, 0x06, 0x00, 0x00, 0x00, 0x1f, 0x15, 0xc4,
                    0x89, 0x00, 0x00, 0x00, 0x01, 0x73, 0x52, 0x47, 0x42, 0x00, 0xae, 0xce, 0x1c, 0xe9, 0x00, 0x00,
                    0x00, 0x0b, 0x49, 0x44, 0x41, 0x54, 0x08, 0xd7, 0x63, 0x60, 0x00, 0x02, 0x00, 0x00, 0x05, 0x00,
                    0x01, 0xe2, 0x26, 0x05, 0x9b, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4e, 0x44, 0xae, 0x42, 0x60,
                    0x82
                };
            }

            bool valid = Validate();

            if (!valid)
            {
                return;
            }

            //Hide the keyboard
            ((PhoneApplicationPage)this.GetView()).Focus();

            await RunTaskAsync(async () =>
            {

                IsBusy = true;

                AskQuestionResponse result = await questionRestService.AskQuestion(Body, Tags, ImageBytes);

                navigationService
                    .UriFor<ViewQuestionViewModel>()
                    .WithParam(vm => vm.QuestionId, result.QuestionId)
                    .Navigate();

                App.ReloadFeeds = true;

                navigationService.RemoveBackEntry();
            },
            (state) =>
            {
                //Do nothing
            },
            ex =>
            {
                IsBusy = false;
            });

        }

    }
}
