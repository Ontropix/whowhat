using System.Collections.Generic;

namespace WhoWhat.UI.WindowsPhone.Services.Model
{
    public class QuestionDetailsResponse : RestResponse
    {
        public Question Question { get; set; }
        public Dictionary<string, VoteDirection> Votes { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
