using ServiceStack.ServiceHost;
using WhoWhat.Domain.User;

namespace WhoWhat.Api.Contract.User
{
    [Route("/users/me/push-subscribe", "POST", Summary = "Register for push notifications")]
    public class SubscribeToPushupsRequest : BaseRequest<PushupsResponse>
    {
        public string DeviceId { get; set; }
        
        public DeviceOS DeviceOs { get; set; }

        /// <summary>
        /// Token for iOs, Device registration Id for Android, Channel Uri for Windows Phone
        /// </summary>
        public string Token { get; set; }
    }
}
