using Platform.Logging;
using Platform.MongoDb;
using Platform.MongoDb.Logging;
using StructureMap;

namespace WhoWhat.Core.Configuration
{
    internal class LoggingConfigurator
    {
        public LoggingConfigurator RegisterMongoDomainLogger(IContainer container)
        {
            var appSettings = container.GetInstance<AppSettings>();

            var mongoLogs = new MongoInstance(appSettings.LogsConnectionString);
            var domainLogs = mongoLogs.GetDatabase().GetCollection<MongoDomainLogRecord>("domain_logs");

            var logManager = new MongoDomainLogManager(domainLogs);
            container.Configure(config => config.For<IDomailLogManager>().Singleton().Use(logManager));
            
            return this;
        }
    }
}