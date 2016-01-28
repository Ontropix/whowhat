using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Cache
{
    [Route("/cache/fill", "POST", Summary = "Fill cache")]
    public class FillCacheRequest : BaseRequest<FillCacheResponse>
    {
    }

    public class FillCacheResponse : BaseResponse
    {
    }
}
