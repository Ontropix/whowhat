using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/{QuestionId}/remove", "POST", Summary = "Remove an existing question.")]
    public class RemoveQuestionRequest : BaseRequest<QuestionStatusResponse>
    {
        public string QuestionId { get; set; }
    }
}
