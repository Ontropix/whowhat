using Platform.Dispatching;
using Platform.Domain;
using WhoWhat.Domain.Notification.Commands;
using IEntityIdGenerator = Platform.Domain.IEntityIdGenerator;

namespace WhoWhat.Domain.Notification
{
    public class NotificationApplicationService : ApplicationService<NotificationAggregate>,
        IMessageHandler<CreateNotification>,
        IMessageHandler<RemoveNotification>
    {
        public NotificationApplicationService(AggregateRepository<NotificationAggregate> aggregateRepository, IEntityIdGenerator idGenerator, EventBus eventBus)
            : base(aggregateRepository, idGenerator, eventBus)
        {
        }

        public void Handle(CreateNotification message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(RemoveNotification message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }
    }
}