using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/{QuestionId}/vote-down", "POST", Summary = "Vote for question.")]
    public class VoteQuestionDownRequest : BaseRequest<VoteQuestionResponse>
    {
        public string QuestionId { get; set; }
    }
}
