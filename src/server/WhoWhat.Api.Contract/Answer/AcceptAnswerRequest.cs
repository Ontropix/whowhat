using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Answer
{
    [Route("/answers/{AnswerId}/mark-accepted", "POST", Summary = "Mark the answer as accepted")]
    public class AcceptAnswerRequest : BaseRequest<AnswerStatusResponse>
    {
        public string AnswerId { get; set; }
    }
}
