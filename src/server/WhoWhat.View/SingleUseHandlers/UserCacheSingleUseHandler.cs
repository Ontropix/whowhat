using System;
using Platform.Dispatching;
using WhoWhat.Core.UserService;
using WhoWhat.Domain.User.Events;

namespace WhoWhat.View.SingleUseHandlers
{
    public class UserCacheSingleUseHandler :
        IMessageHandler<UserRegistered>,
        IMessageHandler<UserRegistrationUpdated>,
        IMessageHandler<UserReputationChanged>
    {
        private readonly IUserCacheService _userCacheService;

        public UserCacheSingleUseHandler(IUserCacheService userCacheService)
        {
            _userCacheService = userCacheService;
        }

        public void Handle(UserRegistered message)
        {
            _userCacheService.Set(message.AggregateId, new UserCache()
            {
                UserId = message.AggregateId,
                FirstName = message.FirstName,
                LastName = message.LastName,
                Reputation = 0,
                AvatarUri = message.PhotoSmallUri,
                NotificationsCheckedAt = DateTime.UtcNow
            });
        }

        public void Handle(UserRegistrationUpdated message)
        {
            _userCacheService.Update(message.AggregateId, cache =>
            {
                cache.UserId = message.AggregateId;
                cache.FirstName = message.FirstName;
                cache.LastName = message.LastName;
                cache.AvatarUri = message.PhotoSmallUri;
            });
        }

        public void Handle(UserReputationChanged message)
        {
            _userCacheService.Update(message.AggregateId, cache => cache.Reputation += message.Shift);
        }
    }
}
