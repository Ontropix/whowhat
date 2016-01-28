using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace WhoWhat.Core.UserService
{
    public class InMemoryUserCacheService : IUserCacheService
    {
        private readonly ConcurrentDictionary<string, UserCache> _users;

        public InMemoryUserCacheService()
        {
            _users = new ConcurrentDictionary<string, UserCache>();
        }

        public UserCache Get(string key)
        {
            UserCache user;
            _users.TryGetValue(key, out user);

            return user;
        }

        public IEnumerable<UserCache> Get(IEnumerable<string> keys)
        {
            List<UserCache> result = _users.Where(element => keys.Contains(element.Key)).Select(element => element.Value).ToList();
            return result;
        }

        public void Set(string key, UserCache user)
        {
            _users[key] = user;
        }

        public void Set(params Tuple<string, UserCache>[] values)
        {
            foreach (Tuple<string, UserCache> tuple in values)
            {
                //insert key -- value pair
                Set(tuple.Item1, tuple.Item2);
            }
        }

        public void Update(string key, Action<UserCache> updater)
        {
            UserCache user = Get(key);
            updater.Invoke(user);
            Set(key, user);
        }

        public void Remove(string key)
        {
            UserCache removedUser;
            _users.TryRemove(key, out removedUser);
        }

        public void Remove(IEnumerable<string> keys)
        {
            foreach (string key in keys)
            {
                UserCache user;
                _users.TryRemove(key, out user);
            }
        }

        public long Count()
        {
            return _users.Count;
        }
    }
}