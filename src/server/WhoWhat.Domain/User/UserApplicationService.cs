using Platform.Dispatching;
using Platform.Domain;
using WhoWhat.Domain.User.Commands;

namespace WhoWhat.Domain.User
{
    public class UserApplicationService : ApplicationService<UserAggregate>,
                                          IMessageHandler<RegisterUser>,
                                          IMessageHandler<UpdateUserRegistration>,
                                          IMessageHandler<SubscribeUserToPushups>,
                                          IMessageHandler<ChangeUserReputation>,
                                          IMessageHandler<UnsubscribeUserFromPushups>
    {
        public UserApplicationService(AggregateRepository<UserAggregate> aggregateRepository, IEntityIdGenerator idGenerator, EventBus eventBus)
            : base(aggregateRepository, idGenerator, eventBus)
        {
        }

        public void Handle(RegisterUser message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(UpdateUserRegistration message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(ChangeUserReputation message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }
        
        public void Handle(SubscribeUserToPushups message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(UnsubscribeUserFromPushups message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }
    }
}