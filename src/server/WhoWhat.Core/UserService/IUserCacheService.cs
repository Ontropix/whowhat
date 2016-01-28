using System;
using System.Collections.Generic;

namespace WhoWhat.Core.UserService
{
    public interface IUserCacheService
    {
        UserCache Get(string key);
        IEnumerable<UserCache> Get(IEnumerable<string> keys);

        void Set(string key, UserCache user);
        void Set(params Tuple<string, UserCache>[] values);

        void Update(string key, Action<UserCache> updater);

        void Remove(string key);
        void Remove(IEnumerable<string> keys);

        long Count();
    }
}
