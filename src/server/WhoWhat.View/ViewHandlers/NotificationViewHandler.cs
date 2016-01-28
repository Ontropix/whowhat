using Platform.Dispatching;
using Platform.Uniform;
using WhoWhat.Domain.Notification.Events;
using WhoWhat.View.Documents;

namespace WhoWhat.View.ViewHandlers
{
    public class NotificationViewHandler : ViewHandler<NotificationDocument>,
                                           IMessageHandler<NotificationCreated>,
                                           IMessageHandler<NotificationRemoved>
    {
        public NotificationViewHandler(IDocumentStore<NotificationDocument> documentStore, ViewContext viewContext) : base(documentStore, viewContext)
        {
        }

        public void Handle(NotificationCreated message)
        {
            DocumentStore.Insert(message.AggregateId, notification =>
            {
                notification.ProducerUserId = message.ProducerUserId;
                notification.TargetUserId = message.TargetUserId;
                notification.Type = message.Type;

                notification.QuestionId = message.QuestionId;
                notification.QuestionBody = message.QuestionBody;
                notification.QuestionThumbnailUri = message.QuestionThumbnailUri;

                notification.CreatedAt = message.CreatedAt;
                notification.RatingShift = message.RatingShift;
            });
        }

        public void Handle(NotificationRemoved message)
        {
            DocumentStore.Delete(message.AggregateId);
        }
    }
}