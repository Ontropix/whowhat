using System.ComponentModel;

namespace WhoWhat.Domain.Notification.Data
{
    public enum NotificationType
    {
        [Description("Question Answered")]
        QuestionAnswered = 1,

        [Description("Question Voted Up")]
        QuestionVotedUp = 2,

        [Description("Question Voted Down")]
        QuestionVotedDown = 3,

        [Description("Answer Voted Up")]
        AnswerVotedUp = 21,

        [Description("Answer Voted Down")]
        AnswerVotedDown = 22,

        [Description("Answer Accepted")]
        AnswerMarkedAsAccepted = 23
    }
}