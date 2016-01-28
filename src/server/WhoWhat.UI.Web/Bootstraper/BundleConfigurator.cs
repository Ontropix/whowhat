using System.Web.Optimization;
using StructureMap;
using WhoWhat.Core;

namespace WhoWhat.UI.Web.Bootstraper
{
    public class BundleConfigurator : IConfigurator
    {
        public void Configure(IContainer container)
        {
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Content/bootstrap/js/bootstrap.js",
                "~/Content/bootstrap/js/jquery-1.9.1.js"
                ));

            BundleTable.Bundles.Add(new StyleBundle("~/theme").Include(
                         "~/Content/bootstrap/css/bootstrap.css",
                         "~/Content/theme/bootstrap-theme.css"));
        }
    }
}