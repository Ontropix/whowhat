using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/{QuestionId}/unvote", "POST", Summary = "Upvote the given question.")]
    public class UnvoteQuestionRequest : BaseRequest<VoteQuestionResponse>
    {
        public string QuestionId { get; set; }
    }
}
