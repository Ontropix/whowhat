using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Redis;
using WhoWhat.Core.UserService;

namespace WhoWhat.Api.Cache
{
    public class RedisUserCacheService : IUserCacheService
    {
        private readonly IRedisClientsManager _redisClientManager;

        private const string Prefix = "user:";

        public RedisUserCacheService(IRedisClientsManager redisClientManager)
        {
            _redisClientManager = redisClientManager;
        }

        private string WrapKey(string userId)
        {
            return Prefix + userId;
        }

        public UserCache Get(string userId)
        {
            userId = WrapKey(userId);

            using (var users = _redisClientManager.GetClient().As<UserCache>())
            {
                return users.GetValue(userId);
            }
        }

        public IEnumerable<UserCache> Get(IEnumerable<string> keys)
        {
            List<string> wrappedKeys = keys.Select(WrapKey).ToList();

            using (var users = _redisClientManager.GetClient().As<UserCache>())
            {
                return users.GetValues(wrappedKeys);
            }
        }

        public void Set(string key, UserCache user)
        {
            using (var redisClient = _redisClientManager.GetClient().As<UserCache>())
            {
                redisClient.SetEntry(WrapKey(key), user);
            }
        }

        public void Set(params Tuple<string, UserCache>[] values)
        {
            using (var redisClient = _redisClientManager.GetClient().As<UserCache>())
            {
                using (var transaction = redisClient.CreateTransaction())
                {
                    foreach (Tuple<string, UserCache> value in values)
                    {
                        Tuple<string, UserCache> iterationElement = value;
                        transaction.QueueCommand(client => client.SetEntry(WrapKey(iterationElement.Item1), iterationElement.Item2));
                    }
                    
                    transaction.Commit();
                }
            }
        }

        public void Update(string userId, Action<UserCache> updater)
        {
            UserCache user = Get(userId);
            updater.Invoke(user);
            Set(userId, user);
        }

        public void Remove(string userId)
        {
            using (var redisClient = _redisClientManager.GetClient().As<UserCache>())
            {
                redisClient.RemoveEntry(WrapKey(userId));
            }
        }

        public void Remove(IEnumerable<string> keys)
        {
            string[] wrappedKeys = keys.Select(WrapKey).ToArray();
            using (var redisClient = _redisClientManager.GetClient().As<UserCache>())
            {
                redisClient.RemoveEntry(wrappedKeys);
            }
        }

        public long Count()
        {
            using (IRedisClient redisClient = _redisClientManager.GetClient())
            {
                return redisClient.SearchKeys(Prefix + "*").LongCount();
            }
        }
    }
}