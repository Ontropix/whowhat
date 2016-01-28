using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/popular", "GET", Summary = "Gets all the popular questions on the site.")]
    public class PopularQuestionsRequest : PageableRequest<QuestionSummariesResponse>
    {

    }
}
