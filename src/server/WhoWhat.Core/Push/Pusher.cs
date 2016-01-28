using System;
using NLog;
using PushSharp;
using PushSharp.Core;
using PushSharp.WindowsPhone;

namespace WhoWhat.Core.Push
{
    /// <summary>
    /// Provides an ability to send push notifications to clients.
    /// </summary>
    public class Pusher : IPusher
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly PushBroker broker;

        public Pusher()
        {
            broker = new PushBroker();

            broker.OnChannelException += BrokerOnOnChannelException;
            broker.OnNotificationFailed += BrokerOnOnNotificationFailed;
            broker.OnChannelCreated += BrokerOnOnChannelCreated;
            broker.OnNotificationSent += BrokerOnOnNotificationSent;

            broker.RegisterWindowsPhoneService();
        }

        private void BrokerOnOnNotificationSent(object sender, INotification notification)
        {
            Logger.Info("Pusher.NotificationSent");
        }

        private void BrokerOnOnChannelCreated(object sender, IPushChannel pushChannel)
        {
            Logger.Info("Pusher.ChannelCreated");
        }

        private void BrokerOnOnNotificationFailed(object sender, INotification notification, Exception error)
        {
            Logger.Error("Pusher.NotificationFailed", error);
        }

        private void BrokerOnOnChannelException(object sender, IPushChannel pushChannel, Exception error)
        {
            Logger.Error("Pusher.ChannelException", error);
        }

        public void ToastWinPhone(string title, string body, string navigatePath, Uri endpointUri)
        {

            string logMessage = string.Format("Title => {0}, Body => {1}, Navigation Path => {2}, Endpoint => {3}",
                title, body, navigatePath, endpointUri);

            Logger.Debug(logMessage);

            broker.QueueNotification(new WindowsPhoneToastNotification()
                .ForEndpointUri(endpointUri)
                .ForOSVersion(WindowsPhoneDeviceOSVersion.Eight)
                .WithBatchingInterval(BatchingInterval.Immediate)
                .WithNavigatePath(navigatePath)
                .WithText1(title)
                .WithText2(body)
            );
        }
    }
}
