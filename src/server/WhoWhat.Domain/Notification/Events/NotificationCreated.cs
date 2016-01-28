using System;
using Platform.Domain;
using WhoWhat.Domain.Notification.Data;

namespace WhoWhat.Domain.Notification.Events
{
    public class NotificationCreated : Event
    {
        public string ProducerUserId { get; set; }
        public string TargetUserId { get; set; }
        public NotificationType Type { get; set; }

        public string QuestionId { get; set; }
        public string QuestionBody { get; set; }
        public string QuestionThumbnailUri { get; set; }

        public int RatingShift { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}