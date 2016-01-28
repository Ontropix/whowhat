using Platform.Dispatching;
using Platform.Uniform;
using WhoWhat.Domain.User;
using WhoWhat.Domain.User.Events;
using WhoWhat.View.Documents;

namespace WhoWhat.View.ViewHandlers
{
    public class UserViewHandler : ViewHandler<UserDocument>,
                                   IMessageHandler<UserRegistered>,
                                   IMessageHandler<UserRegistrationUpdated>,
                                   IMessageHandler<UserReputationChanged>,
                                   IMessageHandler<UserSubscribedToPushups>,
                                   IMessageHandler<UserUnsubscribedFromPushups>
    {
        public UserViewHandler(IDocumentStore<UserDocument> documentStore, ViewContext viewContext) : base(documentStore, viewContext)
        {
        }

        public void Handle(UserRegistered message)
        {
            DocumentStore.Insert(message.AggregateId, user =>
            {
                user.FirstName = message.FirstName;
                user.LastName = message.LastName;

                user.AccessToken = message.AccessToken;
                user.PhotoBigUri = message.PhotoBigUri;
                user.PhotoSmallUri = message.PhotoSmallUri;
                user.ThirdPartyId = message.ThirdPartyId;

                user.LoginType = message.LoginType;

                user.Role = AccessRole.User;
            });
        }

        public void Handle(UserRegistrationUpdated message)
        {
            DocumentStore.Update(message.AggregateId, user =>
            {
                user.FirstName = message.FirstName;
                user.LastName = message.LastName;

                user.AccessToken = message.AccessToken;
                user.PhotoBigUri = message.PhotoBigUri;
                user.PhotoSmallUri = message.PhotoSmallUri;

                user.Role = AccessRole.User;
            });
        }

        public void Handle(UserReputationChanged message)
        {
            DocumentStore.Update(message.AggregateId, user => user.Reputation += message.Shift);
        }

        public void Handle(UserSubscribedToPushups message)
        {
            DocumentStore.Update(message.AggregateId, user => user.PushupsSettings.Add(message.DeviceId, new PushupsPayload
            {
                DeviceOs = message.DeviceOs,
                Token = message.Token
            }));
        }

        public void Handle(UserUnsubscribedFromPushups message)
        {
            DocumentStore.Update(message.AggregateId, user => user.PushupsSettings.Remove(message.DeviceId));
        }
    }
}