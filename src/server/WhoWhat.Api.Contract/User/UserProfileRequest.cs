using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.User
{
    [Route("/users/{UserId}/profile", "GET", Summary = "Return profile of the specific user")]
    public class UserProfileRequest: BaseRequest<UserProfileResponse>
    {
        public string UserId { get; set; }
    }
}
