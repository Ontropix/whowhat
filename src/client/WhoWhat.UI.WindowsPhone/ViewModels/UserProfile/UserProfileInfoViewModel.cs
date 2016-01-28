using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels;
using WhoWhat.UI.WindowsPhone.Resources;
using WhoWhat.UI.WindowsPhone.Services;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.ViewModels.UserProfile
{
    public class UserProfileInfoViewModel : WhoWhatViewModel
    {
        public string UserId { get; set; }

        private readonly UsersRestService usersRestService;
        private readonly AppSettings appSettings;

        private bool Me
        {
            get { return UserId == appSettings.UserId; }
        }

        public UserProfileInfoViewModel(UsersRestService usersRestService, AppSettings appSettings)
        {
            this.usersRestService = usersRestService;
            this.appSettings = appSettings;
            DisplayName = AppResources.Profile_Info;
        }

        private ProfileResponse profile;
        public ProfileResponse Profile
        {
            get { return profile; }
            set
            {
                if (Equals(value, profile)) return;
                profile = value;
                NotifyOfPropertyChange(() => Profile);
            }
        }

        protected async override void OnInitialize()
        {
            base.OnInitialize();

            try
            {
                IsBusy = true;

                if (Me)
                {
                    Profile = await usersRestService.ProfileMe();
                }
                else
                {
                    Profile = await usersRestService.Profile(UserId);
                }
            }
            finally
            {
                IsBusy = false;
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
    }
}
