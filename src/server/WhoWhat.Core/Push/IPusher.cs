using System;

namespace WhoWhat.Core.Push
{
    public interface IPusher
    {
        void ToastWinPhone(string title, string body, string navigatePath, Uri endpointUri);
    }
}
