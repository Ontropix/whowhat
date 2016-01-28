using System.Collections.Generic;
using WhoWhat.Domain.Notification.Data;
using WhoWhat.View.Documents;

namespace WhoWhat.Api
{
    public class NotificationTypeComparer : IEqualityComparer<NotificationDocument>
    {
        public bool Equals(NotificationDocument x, NotificationDocument y)
        {
            switch (x.Type)
            {
                case NotificationType.AnswerVotedUp:
                case NotificationType.AnswerVotedDown:
                {
                    return x.Type == NotificationType.AnswerVotedDown || x.Type == NotificationType.AnswerVotedUp;
                }

                case NotificationType.QuestionVotedUp:
                case NotificationType.QuestionVotedDown:
                {
                    return x.Type == NotificationType.QuestionVotedUp || x.Type == NotificationType.QuestionVotedDown;
                }
                default:
                {
                    return x.Type == y.Type;
                }
            }
        }

        public int GetHashCode(NotificationDocument obj)
        {
            return obj.QuestionId.GetHashCode() & obj.TargetUserId.GetHashCode() & obj.ProducerUserId.GetHashCode();
        }
    }
}