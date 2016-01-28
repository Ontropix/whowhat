using System.Windows;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.Infrastructure.TemplateSelectors
{
    public class NotificationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RatingChanged { get; set; }
        public DataTemplate AnswerMarkedAsAccepted { get; set; }
        public DataTemplate QuestionAnswered { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Notification notification = (Notification)item;
            switch (notification.Type)
            {
                case NotificationType.AnswerVotedDown:
                case NotificationType.AnswerVotedUp:
                case NotificationType.QuestionVotedDown:
                case NotificationType.QuestionVotedUp:
                    return RatingChanged;


                case NotificationType.AnswerMarkedAsAccepted:
                    return AnswerMarkedAsAccepted;
                case NotificationType.QuestionAnswered:
                    return QuestionAnswered;

            }

            return null;
        }
    }
}
