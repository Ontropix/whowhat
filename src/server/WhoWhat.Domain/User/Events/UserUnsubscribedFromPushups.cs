using Platform.Domain;

namespace WhoWhat.Domain.User.Events
{
    public class UserUnsubscribedFromPushups : Event
    {
        public string DeviceId { get; set; }
    }
}