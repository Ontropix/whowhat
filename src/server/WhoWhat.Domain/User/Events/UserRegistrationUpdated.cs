using Platform.Domain;

namespace WhoWhat.Domain.User.Events
{
    public class UserRegistrationUpdated : Event
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string AccessToken { get; set; }
        public string PhotoSmallUri { get; set; }
        public string PhotoBigUri { get; set; }
    }
}