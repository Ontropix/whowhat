using ServiceStack.ServiceHost;
using WhoWhat.Api.Contract.Question;

namespace WhoWhat.Api.Contract.User
{
    [Route("/users/{UserId}/answers", "GET", Summary = "Answers of the user")]
    public class UserAnswersRequest : PageableRequest<QuestionSummariesResponse>
    {
        public string UserId { get; set; }
    }
}
