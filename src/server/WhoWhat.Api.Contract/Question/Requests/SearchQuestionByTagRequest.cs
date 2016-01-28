using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/search/tagged/{Tag}", "GET", Summary = "Search question by tag.")]
    public class SearchQuestionByTagRequest : PageableRequest<QuestionSummariesResponse>
    {
        public string Tag { get; set; }
    }
}