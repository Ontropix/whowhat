using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/unanswered", "GET", Summary = "Gets all the unanswered questions on the site.")]
    public class UnansweredQuestionsRequest : PageableRequest<QuestionSummariesResponse>
    {
    }
}
