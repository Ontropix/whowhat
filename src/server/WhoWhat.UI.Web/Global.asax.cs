using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;
using StructureMap.Graph;
using WhoWhat.Core;
using WhoWhat.Core.Configuration;
using WhoWhat.Core.UserService;
using WhoWhat.UI.Web.Bootstraper;
using WhoWhat.View.Documents;

namespace WhoWhat.UI.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Global IoC
            ObjectFactory.Initialize(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            }));

            //Run configurators
            List<IConfigurator> tasks = new List<IConfigurator>()
            {
                new ReleaseConfig(),
                new ServiceStackConfigurator(),
                new RoutingConfigurator(),
                new BundleConfigurator(),
                new UserCacheManager()
            };

            foreach (IConfigurator task in tasks)
            {
                task.Configure(ObjectFactory.Container);
            }

            //Other
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }
    }

    public class UserCacheManager : IConfigurator
    {
        public void Configure(IContainer container)
        {
            var userCache = container.GetInstance<IUserCacheService>();
            var viewContext = container.GetInstance<View.ViewContext>();

            List<UserDocument> users = viewContext.Users.AsQueryable().ToList();
            IEnumerable<Tuple<string, UserCache>> tuples = users.Select(x => new Tuple<string, UserCache>(x.Id, new UserCache()
            {
                UserId = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Reputation = x.Reputation,
                AvatarUri = x.PhotoSmallUri,
                NotificationsCheckedAt = DateTime.Now
            }));

            //TODO paging
            userCache.Set(tuples.ToArray());
        }
    }
}