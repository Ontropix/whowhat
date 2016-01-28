using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.User
{
    [Route("/users/me/push-unsubscribe", "POST", Summary = "Unregister from push notifications")]
    public class UnsubscribeFromPushupsRequest : BaseRequest<PushupsResponse>
    {
        public string DeviceId { get; set; }
    }
}