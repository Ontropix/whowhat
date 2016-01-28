using System.Collections.Generic;
using Platform.Uniform;
using WhoWhat.Domain.User;

namespace WhoWhat.View.Documents
{
    public class UserDocument : IDocument
    {
        public UserDocument()
        {
            this.PushupsSettings = new Dictionary<string, PushupsPayload>();
        }

        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ThirdPartyId { get; set; }
        public string AccessToken { get; set; }
        public UserLoginType LoginType { get; set; }

        public string PhotoSmallUri { get; set; }
        public string PhotoBigUri { get; set; }

        public int Reputation { get; set; }
        public AccessRole Role { get; set; }

        /// <summary>
        /// Key: DeviceId, Value: Settings
        /// </summary>
        public Dictionary<string, PushupsPayload> PushupsSettings { get; set; }
    }
}