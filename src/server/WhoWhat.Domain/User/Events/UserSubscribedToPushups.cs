using Platform.Domain;

namespace WhoWhat.Domain.User.Events
{
    public class UserSubscribedToPushups : Event
    {
        public string DeviceId { get; set; }
        public DeviceOS DeviceOs { get; set; }
        public string Token { get; set; }
    }
}