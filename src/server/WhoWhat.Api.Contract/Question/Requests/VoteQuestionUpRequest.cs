using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/{QuestionId}/vote-up", "POST", Summary = "Vote for question.")]
    public class VoteQuestionUpRequest : BaseRequest<VoteQuestionResponse>
    {
        public string QuestionId { get; set; }
    }
}
