using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.User
{
    [Route("/users/me/notifications-count", "GET", Summary = "Returns count of notifications of the current user")]
    public class NotificationsCountRequest : BaseRequest<NotificationsCountResponse>
    {
    }
}