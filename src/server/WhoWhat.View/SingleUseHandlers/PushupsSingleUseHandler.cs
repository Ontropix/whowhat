using System;
using NLog;
using Platform.Dispatching;
using WhoWhat.Core;
using WhoWhat.Core.Helpers;
using WhoWhat.Core.Push;
using WhoWhat.Domain.Notification.Events;
using WhoWhat.Domain.User;
using WhoWhat.View.Documents;

namespace WhoWhat.View.SingleUseHandlers
{
    public class PushupsSingleUseHandler : 
        IMessageHandler<NotificationCreated>
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IPusher _pusher;
        private readonly ViewContext _viewContext;

        public PushupsSingleUseHandler(IPusher pusher, ViewContext viewContext)
        {
            _pusher = pusher;
            _viewContext = viewContext;

            _logger.Debug(pusher);
        }
        
        public void Handle(NotificationCreated message)
        {
            UserDocument user = _viewContext.Users.GetById(message.TargetUserId);

            foreach (PushupsPayload settings in user.PushupsSettings.Values)
            {
                if (settings != null && settings.DeviceOs == DeviceOS.WP)
                {
                    string title = StructHelper.GetEnumDescription(message.Type);

                    string body = message.QuestionBody.Left(64);
                    if (message.QuestionBody.Length > 64)
                    {
                        body += "...";
                    }

                    SendWindowsPhonePushup(title, body, message.QuestionId, settings.Token);
                }
            }
        }

        private void SendWindowsPhonePushup(string title, string body, string questionId, string userToken)
        {
            _pusher.ToastWinPhone(title,
                                  body,
                                  String.Format("/Views/Feeds/FeedsView.xaml?QuestionId={0}", questionId), //Root xaml
                                  new Uri(userToken)
                );
        }
    }
}
