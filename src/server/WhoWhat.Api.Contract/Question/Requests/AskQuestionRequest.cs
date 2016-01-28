using System.Collections.Generic;
using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Contract.Question
{
    [Route("/questions/ask", "POST", Summary = "Ask a question.")]
    public class AskQuestionRequest : BaseRequest<AskQuestionResponse>
    {
        public string Body { get; set; }

        public List<string> Tags { get; set; }

        public string Bytes { get; set; }
    }
}
