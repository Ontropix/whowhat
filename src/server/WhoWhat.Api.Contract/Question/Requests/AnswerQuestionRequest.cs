using ServiceStack.ServiceHost;
using WhoWhat.Api.Contract.Answer;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/{QuestionId}/answer", "POST", Summary = "Answer to the question.")]
    public class AnswerQuestionRequest : BaseRequest<AnswerStatusResponse>
    {
        public string QuestionId { get; set; }

        public string Body { get; set; }
    }
}
