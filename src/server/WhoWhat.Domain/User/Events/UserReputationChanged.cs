using Platform.Domain;

namespace WhoWhat.Domain.User.Events
{
    public class UserReputationChanged : Event
    {
        public int Shift { get; set; }
    }
}