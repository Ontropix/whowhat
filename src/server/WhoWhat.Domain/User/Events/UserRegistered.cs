using Platform.Domain;

namespace WhoWhat.Domain.User.Events
{
    public class UserRegistered : Event
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ThirdPartyId { get; set; }
        public string AccessToken { get; set; }
        public string PhotoSmallUri { get; set; }
        public string PhotoBigUri { get; set; }

        public UserLoginType LoginType { get; set; }
    }
}
