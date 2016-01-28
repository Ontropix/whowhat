using System;

namespace WhoWhat.UI.WindowsPhone.Services.Model
{

    [PropertyChanged.ImplementPropertyChanged]
    public class Notification
    {
        public string Id { get; set; }

        public string ProducerUserId { get; set; }
        public string TargetUserId { get; set; }
        public NotificationType Type { get; set; }

        public string QuestionId { get; set; }
        public string QuestionBody { get; set; }
        public string QuestionThumbnailUri { get; set; }

        public DateTime CreatedAt { get; set; }

        public int RatingShift { get; set; }
    }

    public enum NotificationType
    {
        QuestionAnswered = 1,
        QuestionVotedUp = 2,
        QuestionVotedDown = 3,

        AnswerVotedUp = 21,
        AnswerVotedDown = 22,
        AnswerMarkedAsAccepted = 23
    }
}