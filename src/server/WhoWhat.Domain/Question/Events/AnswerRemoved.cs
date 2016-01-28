using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class AnswerRemoved : Event
    {
        public string AnswerId { get; set; }
    }
}