using System;
using WhoWhat.Core.Push;

namespace WhoWhat.Core
{
    public class MockPusher : IPusher
    {
        public string WpToast { get; set; }

        public void ToastWinPhone(string title, string body, string navigatePath, Uri endpointUri)
        {
            WpToast = string.Format("Title: {0}\nBody:{1}\nNavigationPath: {2}\nEndpoint: {3}\n",
                title, body, navigatePath, endpointUri);
        }
    }
}
