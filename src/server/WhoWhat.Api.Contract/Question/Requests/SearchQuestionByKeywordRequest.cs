using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/search/keyword/{Keyword}", "GET", Summary = "Search question by term in question body.")]
    public class SearchQuestionByKeywordRequest : PageableRequest<QuestionSummariesResponse>
    {
        public string Keyword { get; set; }
    }
}
