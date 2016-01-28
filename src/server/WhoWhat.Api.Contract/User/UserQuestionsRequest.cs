using ServiceStack.ServiceHost;
using WhoWhat.Api.Contract.Question;

namespace WhoWhat.Api.Contract.User
{
    [Route("/users/{UserId}/questions", "GET", Summary = "Question asked by the user")]
    public class UserQuestionsRequest : PageableRequest<QuestionSummariesResponse>
    {
        public string UserId { get; set; }
    }
}
