using NLog;
using ServiceStack.Redis;
using StructureMap;
using WhoWhat.Api.Cache;
using WhoWhat.Core.UserService;

namespace WhoWhat.Core.Configuration
{
    internal class UserCacheConfigurator
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        internal void RegisterRedisUserCache(IContainer container)
        {
            var appSettings = container.GetInstance<AppSettings>();
            var redisClientManager = new BasicRedisClientManager(appSettings.RedisConnectionString);

            _logger.Debug(appSettings.RedisConnectionString);
            _logger.Debug(redisClientManager);

            container.Configure(config =>
            {
                config.For<IRedisClientsManager>().Singleton().Use(redisClientManager);
                config.For<IUserCacheService>().Singleton().Use<RedisUserCacheService>();
            });
        }

        internal void RegisterInMemoryUserCache(IContainer container)
        {
            container.Configure(config => config.For<IUserCacheService>().Singleton().Use<InMemoryUserCacheService>());
        }
    }
}