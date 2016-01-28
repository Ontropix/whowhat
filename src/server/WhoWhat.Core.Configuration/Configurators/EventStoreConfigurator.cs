using Platform.Domain;
using Platform.EventStore;
using Platform.EventStore.InMemory;
using Platform.EventStore.MongoDB;
using Platform.MongoDb;
using StructureMap;

namespace WhoWhat.Core.Configuration
{
    internal class EventStoreConfigurator
    {
        internal EventStoreConfigurator RegisterInMemoryEventStore(IContainer container)
        {
            var eventStore = new InMemoryEventStore(EventStoreSettings.GetDefault());
            container.Configure(config => config.For<IEventStore>().Singleton().Use(eventStore));

            return this;
        }

        internal EventStoreConfigurator RegisterMongoEventStore(IContainer container)
        {
            var appSettings = container.GetInstance<AppSettings>();
            var mongoEvents = new MongoInstance(appSettings.EventsConnectionString);
            var eventStore = new MongoEventStore(mongoEvents.GetDatabase(), EventStoreSettings.GetDefault());
            container.Configure(config => config.For<IEventStore>().Singleton().Use(eventStore));
            
            return this;
        }

        internal EventStoreConfigurator RegisterAggregateRepository(IContainer container)
        {
            container.Configure(config => config.For(typeof(AggregateRepository<>)).Singleton().Use(typeof(AggregateRepository<>)));
            return this;
        }
    }
}