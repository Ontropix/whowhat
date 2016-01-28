using System.Collections.Generic;
using System.Linq;
using System;
using NLog;
using WhoWhat.Api.Cache;
using WhoWhat.Api.Contract.Cache;
using WhoWhat.Core.UserService;
using WhoWhat.View.Documents;

namespace WhoWhat.Api
{
    [NoCache]
    public class CacheService : CommandService
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public FillCacheResponse Post(FillCacheRequest request)
        {
            if (UserCacheService.Count() > 0)
            {
                Logger.Debug("CacheService. Redis is alive");

                //Redis is alive
                return new FillCacheResponse();
            }

            Logger.Info("Redis is dead. Filling cache");

            //TODO move this code to the separate class
            List<UserDocument> users = ViewContext.Users.AsQueryable().ToList();
            IEnumerable<Tuple<string, UserCache>> tuples = users.Select(x => new Tuple<string, UserCache>(x.Id, new UserCache()
            {
                UserId = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Reputation = x.Reputation,
                AvatarUri = x.PhotoSmallUri,
                NotificationsCheckedAt = DateTime.Now
            }));

            UserCacheService.Set(tuples.ToArray());

            Logger.Info("Cache is filled up");

            return new FillCacheResponse();
        }
    }
}
