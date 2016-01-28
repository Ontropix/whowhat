using System;
using System.Threading.Tasks;
using Microsoft.Phone.Notification;

namespace WhoWhat.UI.WindowsPhone.Services
{
    public class PushSharpClient
    {
        private readonly UsersRestService usersRestService;

        public PushSharpClient(UsersRestService usersRestService)
        {
            this.usersRestService = usersRestService;
        }

        public async Task RegisterForToast()
        {
            // The name of our push channel.
            const string channelName = "WhoWhat.Notifications";

            // Try to find the push channel.
            HttpNotificationChannel pushChannel = HttpNotificationChannel.Find(channelName);

            // If the channel was not found, then create a new connection to the push service.
            if (pushChannel == null)
            {
                pushChannel = new HttpNotificationChannel(channelName);
                pushChannel.Open();
            }

            Subscribe(pushChannel);

            if (pushChannel.ChannelUri != null)
            {
                await PushSubscribe(pushChannel.ChannelUri);
            }    
        }

        private void Subscribe(HttpNotificationChannel pushChannel)
        {
            // The channel was already open, so just register for all the events.
            pushChannel.ChannelUriUpdated += async (sender, e) =>
            {
                await PushSubscribe(e.ChannelUri);
            };

            pushChannel.ErrorOccurred += (sender, e) => 
                System.Diagnostics.Debug.WriteLine("PushChannel Error: " + e.ErrorType.ToString() + " -> " + e.ErrorCode + " -> " + e.Message + " -> " + e.ErrorAdditionalData);

            // Bind this new channel for toast events.
            if (pushChannel.IsShellToastBound)
                System.Diagnostics.Debug.WriteLine("Already Bound to Toast");
            else
                pushChannel.BindToShellToast();

            if (pushChannel.IsShellTileBound)
                System.Diagnostics.Debug.WriteLine("Already Bound to Tile");
            else
                pushChannel.BindToShellTile();
        }

        private async Task PushSubscribe(Uri token)
        {
            try
            {
                await usersRestService.PushSubscribe(token);

                //Updated uri
                System.Diagnostics.Debug.WriteLine("PushChannel URI Updated: " + token.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("PushSubscribe failed." + ex.Message);
            }
        }

        public async Task UnregisterFromToast()
        {
            await usersRestService.PushUnsubscribe();
        }
    }
}
