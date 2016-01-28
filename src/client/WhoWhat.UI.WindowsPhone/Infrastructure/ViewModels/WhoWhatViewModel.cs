using Caliburn.Micro;
using WhoWhat.UI.WindowsPhone.Core;

namespace WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels
{
    public class WhoWhatViewModel: Screen
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();

            //Simply get value by IoC, prevent overburden viewmodels 
            IsAuthenticated = IoC.Get<AppSettings>().IsAuthenticated;
        }

        private bool isAuthenticated;
        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set
            {
                if (value.Equals(isAuthenticated)) return;
                isAuthenticated = value;
                NotifyOfPropertyChange(() => IsAuthenticated);
            }
        }
    }
}
