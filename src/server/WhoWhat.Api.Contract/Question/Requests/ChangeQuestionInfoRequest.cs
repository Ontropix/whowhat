using System.Collections.Generic;
using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/{QuestionId}/change-info", "POST", Summary = "Change question body and tags")]
    public class ChangeQuestionInfoRequest : BaseRequest<ChangeQuestionInfoResponse>
    {
        public string QuestionId { get; set; }
        public string Body { get; set; }
        public HashSet<string> Tags { get; set; }
    }
}