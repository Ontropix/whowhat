using System;
using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.User
{
    [Route("/users/me/notifications", "GET", Summary = "Returns notifications of the current user")]
    public class NotificationsRequest : PageableRequest<NotificationsResponse>
    {
    }
}
