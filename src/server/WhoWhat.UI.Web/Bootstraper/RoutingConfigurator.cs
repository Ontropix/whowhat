using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;
using WhoWhat.Core;

namespace WhoWhat.UI.Web.Bootstraper
{
    public class RoutingConfigurator : IConfigurator
    {
        public void Configure(IContainer container)
        {
            RouteCollection routes = RouteTable.Routes;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("api/{*pathInfo}");

            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("robots.txt");

            routes.MapRoute(
              name: "Sucess",
              url: "success",
              defaults: new { controller = "Entry", action = "Success" }
           );

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Entry", action = "Entry", id = UrlParameter.Optional }
           );
        }
    }
}