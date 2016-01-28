using System;
using Platform.Uniform;
using WhoWhat.Domain.Notification.Data;

namespace WhoWhat.View.Documents
{
    public class NotificationDocument : IDocument
    {
        public string Id { get; set; }

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