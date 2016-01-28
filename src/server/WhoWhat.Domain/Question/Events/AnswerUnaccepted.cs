using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class AnswerUnaccepted : Event
    {
        public string AnswerId { get; set; }
    }
}