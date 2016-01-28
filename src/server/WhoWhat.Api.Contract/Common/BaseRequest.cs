using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract
{
    public abstract class BaseRequest<T> : IReturn<T> where T : BaseResponse
    {
    }
}
