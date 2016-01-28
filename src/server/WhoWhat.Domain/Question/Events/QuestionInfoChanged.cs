using System.Collections.Generic;
using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class QuestionInfoChanged : Event
    {
        public string Body { get; set; }
        public HashSet<string> Tags { get; set; }
    }
}