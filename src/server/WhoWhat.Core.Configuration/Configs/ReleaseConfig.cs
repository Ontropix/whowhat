using StructureMap;

namespace WhoWhat.Core.Configuration
{
    public class ReleaseConfig : IConfigurator
    {
        public void Configure(IContainer container)
        {
            new CoreConfigurator().RegisterSettings(container)
                                  .RegisterMongoIdGenerator(container)
                                  .RegisterImageService(container)
                                  .RegisterPushBroker(container);

            new MongoDbConfigurator().RegisterMongoDbConventions();

            new LoggingConfigurator().RegisterMongoDomainLogger(container);

            new EventStoreConfigurator().RegisterMongoEventStore(container)
                                        .RegisterAggregateRepository(container);

            new UserCacheConfigurator().RegisterRedisUserCache(container);

            new UniformConfigurator().RegisterMongoUniform(container)
                                     .RegisterDocuments(container)
                                     .RegisterDocumentStores(container)
                                     .RegisterViewContext(container)
                                     .CreateIndexes(container);

            new TransportConfigurator().RegisterDispatcher(container, enableLogs: false)
                                       .RegisterServiceBus(container)
                                       .RegisterDomainBuses(container);
        }
    }
}
