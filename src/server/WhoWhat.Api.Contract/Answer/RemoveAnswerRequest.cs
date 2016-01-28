using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Answer
{
    [Route("/answers/{AnswerId}/remove", "POST", Summary = "Remove the answer.")]
    public class RemoveAnswerRequest : BaseRequest<AnswerStatusResponse>
    {
        public string AnswerId { get; set; }
    }
}
