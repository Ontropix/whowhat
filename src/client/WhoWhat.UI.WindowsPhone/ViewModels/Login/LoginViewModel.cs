using Caliburn.Micro;

namespace WhoWhat.UI.WindowsPhone.ViewModels.Login
{
    public class LoginViewModel: Screen
    {
        private readonly INavigationService navigationService;

        public LoginViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public void Facebook()
        {
            navigationService.UriFor<FacebookLoginViewModel>().Navigate();
        }

        public void Vkontakte()
        {
            navigationService.UriFor<VkontakteLoginViewModel>().Navigate();
        }
    }
}
