using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/live", "GET", Summary = "Gets live stream of questions.")]
    public class RecentQuestionsRequest : PageableRequest<QuestionSummariesResponse>
    {
    }
}
