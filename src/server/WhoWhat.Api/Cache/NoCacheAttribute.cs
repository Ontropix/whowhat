using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace WhoWhat.Api.Cache
{
    /// <summary>
    /// Should be applied to each service or method that should not be cached on the client.
    /// </summary>
    public class NoCacheAttribute : RequestFilterAttribute
    {
        public override void Execute(IHttpRequest req, IHttpResponse res, object responseDto)
        {
            res.AddHeader(HttpHeaders.CacheControl, "no-store,must-revalidate,no-cache,max-age=0");
        }
    }
}
