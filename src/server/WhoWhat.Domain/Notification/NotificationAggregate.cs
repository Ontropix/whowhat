using Platform.Domain;
using WhoWhat.Domain.Notification.Commands;
using WhoWhat.Domain.Notification.Events;

namespace WhoWhat.Domain.Notification
{
    public class NotificationAggregate : AggregateRoot<NotificationState>
    {
        [AggregateCtor]
        public void When(CreateNotification command)
        {
            ApplyEvent<NotificationCreated>(evnt => evnt.InjectFromCommand(command));
        }

        public void When(RemoveNotification command)
        {
            ApplyEvent<NotificationRemoved>(evnt => evnt.InjectFromCommand(command));
        }
    }
}