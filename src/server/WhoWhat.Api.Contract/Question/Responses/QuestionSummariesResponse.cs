using System.Collections.Generic;
using WhoWhat.Api.Contract.Payload;

namespace WhoWhat.Api.Contract.Question
{
    public class QuestionSummariesResponse : BaseResponse
    {
        public IEnumerable<QuestionDto> Questions { get; set; }
    }
}