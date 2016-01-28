using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/{QuestionId}/change-image", "POST", Summary = "Change question image")]
    public class ChangeQuestionImageRequest : BaseRequest<ChangeQuestionInfoResponse>
    {
        public string QuestionId { get; set; }
        public string Bytes { get; set; }
    }
}
