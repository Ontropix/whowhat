using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Answer
{
    [Route("/answers/{AnswerId}/vote-down", "POST", Summary = "Vote down for the answer.")]
    public class VoteAnswerDownRequest : BaseRequest<VoteAnswerResponse>
    {
        public string AnswerId { get; set; }
    }
}
