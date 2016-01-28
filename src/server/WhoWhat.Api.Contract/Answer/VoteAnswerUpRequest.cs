using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Answer
{
    [Route("/answers/{AnswerId}/vote-up", "POST", Summary = "Vote up for the answer.")]
    public class VoteAnswerUpRequest : BaseRequest<VoteAnswerResponse>
    {
        public string AnswerId { get; set; }
    }
}
