using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Answer
{
    [Route("/answers/{AnswerId}/change-body", "POST", Summary = "Change the answer.")]
    public class ChangeAnswerRequest : BaseRequest<AnswerStatusResponse>
    {
        public string AnswerId { get; set; }
        public string Body { get; set; }
    }
}
