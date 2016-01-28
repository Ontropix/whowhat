using System.Xml.Linq;
using Caliburn.Micro;
using Microsoft.Phone.Tasks;

namespace WhoWhat.UI.WindowsPhone.ViewModels
{
    public class AboutViewModel: Screen
    {
        private readonly IEventAggregator eventAggregator;

        public AboutViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Version = XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
        }

        private string version;
        public string Version
        {
            get { return version; }
            set
            {
                if (value == version) return;
                version = value;
                NotifyOfPropertyChange(() => Version);
            }
        }

        public void Support()
        {
            eventAggregator.RequestTask<EmailComposeTask>(task =>
            {
                task.To = "support@code9.biz";
                task.Subject = "Support WhoWhat";
            });
        }
    }
}
