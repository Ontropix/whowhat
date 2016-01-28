using StructureMap;

namespace WhoWhat.Core.Configuration
{
    public class TestConfig : IConfigurator
    {
        public void Configure(IContainer container)
        {
            new CoreConfigurator().RegisterSettings(container)
                                  .RegisterGuidIdGenerator(container)
                                  .RegisterFakePusher(container)
                                  .RegisterFakeImageStorage(container);

            new EventStoreConfigurator().RegisterInMemoryEventStore(container)
                                        .RegisterAggregateRepository(container);

            new LoggingConfigurator().RegisterMongoDomainLogger(container);

            new UserCacheConfigurator().RegisterInMemoryUserCache(container);

            new UniformConfigurator().RegisterInMemoryUniform(container)
                                     .RegisterDocuments(container)
                                     .RegisterDocumentStores(container)
                                     .RegisterViewContext(container);

            new TransportConfigurator().RegisterDispatcher(container, enableLogs: false)
                                       .RegisterServiceBus(container)
                                       .RegisterDomainBuses(container);
        }
    }
}