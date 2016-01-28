using System.Collections.Generic;
using WhoWhat.Api.Contract.Payload;
using WhoWhat.View.Payload;

namespace WhoWhat.Api.Contract.Question
{
    /// <summary>
    /// Represent a question with answers and votes
    /// </summary>
    public class QuestionDetailsResponse : BaseResponse
    {
        public QuestionDto Question { get; set; }

        public List<AnswerDto> Answers { get; set; }
        public Dictionary<string, VoteDirection> Votes { get; set; } 
    }
}