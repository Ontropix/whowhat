using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Answer
{
    [Route("/answers/{AnswerId}/unvote", "POST", Summary = "Unvote the given answer.")]
    public class UnvoteAnswerRequest : BaseRequest<VoteAnswerResponse>
    {
        public string AnswerId { get; set; }
    }
}
