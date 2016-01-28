using System;
using System.Collections.Generic;
using WhoWhat.Api.Contract.Payload;

namespace WhoWhat.Api.Contract.User
{
    public class NotificationsResponse: BaseResponse
    {
        public List<Notification> Notifications { get; set; }

        public DateTime LatestNotificationChecking { get; set; }
    }
}
