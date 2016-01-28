using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Answer
{
    [Route("/answers/{AnswerId}/unmark-accepted", "POST", Summary = "Unmark the answer as accepted")]
    public class UnacceptAnswerRequest : BaseRequest<AnswerStatusResponse>
    {
        public string AnswerId { get; set; }
    }
}
