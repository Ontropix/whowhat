using StructureMap;

namespace WhoWhat.Core.Configuration
{
    public class DebugConfig : IConfigurator
    {
        public void Configure(IContainer container)
        {
            new CoreConfigurator().RegisterSettings(container)
                                  .RegisterMongoIdGenerator(container)
                                  .RegisterFakePusher(container)
                                  .RegisterFakeImageStorage(container);
            
            new MongoDbConfigurator().RegisterMongoDbConventions();

            new LoggingConfigurator().RegisterMongoDomainLogger(container);

            new EventStoreConfigurator().RegisterMongoEventStore(container)
                                        .RegisterAggregateRepository(container);

            new UserCacheConfigurator().RegisterInMemoryUserCache(container);

            new UniformConfigurator().RegisterMongoUniform(container)
                                     .RegisterDocuments(container)
                                     .RegisterDocumentStores(container)
                                     .RegisterViewContext(container);

            new TransportConfigurator().RegisterDispatcher(container, enableLogs: false)
                                       .RegisterServiceBus(container)
                                       .RegisterDomainBuses(container);
        }
    }
}