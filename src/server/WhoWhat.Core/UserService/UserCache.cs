using System;

namespace WhoWhat.Core.UserService
{
    public class UserCache
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUri { get; set; }
        public int Reputation { get; set; }

        public DateTime NotificationsCheckedAt { get; set; }
    }
}
