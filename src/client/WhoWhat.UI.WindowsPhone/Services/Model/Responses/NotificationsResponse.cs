using System;
using System.Collections.Generic;

namespace WhoWhat.UI.WindowsPhone.Services.Model
{
    public class NotificationsResponse : RestResponse
    {
        public List<Notification> Notifications { get; set; }

        public DateTime LatestNotificationChecking { get; set; }
    }
}
