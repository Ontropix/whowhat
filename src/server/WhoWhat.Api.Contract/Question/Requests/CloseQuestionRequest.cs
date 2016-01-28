using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/{QuestionId}/close", "POST", Summary = "Close an existing question.")]
    public class CloseQuestionRequest : BaseRequest<CloseQuestionResponse>
    {
        public string QuestionId { get; set; }
    }
}
