using Platform.Domain;
using WhoWhat.Domain.User.Commands;
using WhoWhat.Domain.User.Events;

namespace WhoWhat.Domain.User
{
    public class UserAggregate : AggregateRoot<UserState>
    {
        [AggregateCtor]
        public void When(RegisterUser command)
        {
            ApplyEvent<UserRegistered>(evnt => evnt.InjectFromCommand(command));
        }

        public void When(UpdateUserRegistration command)
        {
            ApplyEvent<UserRegistrationUpdated>(evnt => evnt.InjectFromCommand(command));
        }

        public void When(ChangeUserReputation command)
        {
            ApplyEvent<UserReputationChanged>(evnt => evnt.InjectFromCommand(command));
        }

        public void When(SubscribeUserToPushups command)
        {
            ApplyEvent<UserSubscribedToPushups>(evnt => evnt.InjectFromCommand(command));
        }

        public void When(UnsubscribeUserFromPushups command)
        {
            ApplyEvent<UserUnsubscribedFromPushups>(evnt => evnt.InjectFromCommand(command));
        }
    }
}