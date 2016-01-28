using ServiceStack.CacheAccess;
using ServiceStack.Configuration;
using ServiceStack.Logging.NLogger;
using ServiceStack.Redis;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.WebHost.Endpoints;
using StructureMap;
using WhoWhat.Api;
using WhoWhat.Core;
using WhoWhat.Core.StructureMap;

namespace WhoWhat.UI.Web.Bootstraper
{
    public class ServiceStackConfigurator : IConfigurator
    {
        public void Configure(IContainer container)
        {
            ServiceStack.Logging.LogManager.LogFactory = new NLogFactory();
            new AppHost().Init();
        }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("WhoWhat", typeof(Namespace_ApiProject).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            //Resource manager
            container.Register<IResourceManager>(new ConfigurationResourceManager());
            var appSettings = container.Resolve<IResourceManager>();

            Plugins.Add(new SessionFeature());

            string redis = appSettings.GetString("redis_connection_string");
            container.Register<IRedisClientsManager>(c => new BasicRedisClientManager(redis));

            // Register storage for user sessions 
            container.Register<ICacheClient>(c => c.Resolve<IRedisClientsManager>().GetCacheClient()).ReusedWithin(Funq.ReuseScope.None);
            container.Register<ISessionFactory>(c => new SessionFactory(c.Resolve<ICacheClient>()));

            // Set JSON web services to return idiomatic JSON camelCase properties
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

            // Validation
            Plugins.Add(new ValidationFeature());
            container.RegisterValidators(typeof(Namespace_ApiProject).Assembly);

            Plugins.Add(new AuthFeature(() => new CustomUserSession(),
                                        new IAuthProvider[]
                                        {
                                            new FbOAuth2Provider(appSettings),
                                            new VkOAuth2Provider(appSettings),
                                        }));
            
            //container.Register<ICacheClient>(new MemoryCacheClient());
            Plugins.Add(new ServiceStack.Api.Swagger.SwaggerFeature());

            container.Adapter = new StructureMapContainerAdapter();
        }
    }
}