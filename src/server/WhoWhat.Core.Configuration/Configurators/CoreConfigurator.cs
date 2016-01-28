using System;
using Platform.Common.AppSettings;
using Platform.Domain;
using Platform.MongoDb;
using StructureMap;
using WhoWhat.Core.Content;
using WhoWhat.Core.Push;
using WhoWhat.Core.StructureMap;

namespace WhoWhat.Core.Configuration
{
    internal class CoreConfigurator
    {
        public CoreConfigurator RegisterSettings(IContainer container)
        {
            var settings = SettingsMapper.Map<AppSettings>();
            container.Configure(config => config.For<AppSettings>().Singleton().Use(settings));

            var serviceProvider = new StructureMapServiceProvider(container);
            container.Configure(config => config.For<IServiceProvider>().Singleton().Use(serviceProvider));
            
            return this;
        }

        public CoreConfigurator RegisterMongoIdGenerator(IContainer container)
        {
            container.Configure(config => config.For<IEntityIdGenerator>().Use<MongoEntityIdGenerator>());
            return this;
        }

        public CoreConfigurator RegisterGuidIdGenerator(IContainer container)
        {
            container.Configure(config => config.For<IEntityIdGenerator>().Use<GuidEntityIdGenerator>());
            return this;
        }

        public CoreConfigurator RegisterImageService(IContainer container)
        {
            var appSettings = container.GetInstance<AppSettings>();
            ImageStorageService imageStorageService = new ImageStorageService(appSettings.AzureConnectionString);
            container.Configure(config => config.For<IImageStorageService>().Singleton().Use(imageStorageService));

            return this;
        }

        public CoreConfigurator RegisterFakeImageStorage(IContainer container)
        {
            IImageStorageService imageStorageService = new FakeImageStorageService();
            container.Configure(config => config.For<IImageStorageService>().Singleton().Use(imageStorageService));

            return this;
        }

        public CoreConfigurator RegisterPushBroker(IContainer container)
        {
            container.Configure(config => config.For<IPusher>().Singleton().Use<Pusher>());
            return this;
        }

        public CoreConfigurator RegisterFakePusher(IContainer container)
        {
            container.Configure(config => config.For<IPusher>().Singleton().Use<MockPusher>());
            return this;
        }
    }
}