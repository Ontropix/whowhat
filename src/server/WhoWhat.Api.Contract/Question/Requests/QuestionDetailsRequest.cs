using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/{QuestionId}/details", "GET", Summary = "Question with answers and votings")]
    public class QuestionDetailsRequest : BaseRequest<QuestionDetailsResponse>
    {
        public string QuestionId { get; set; }
    }
}
