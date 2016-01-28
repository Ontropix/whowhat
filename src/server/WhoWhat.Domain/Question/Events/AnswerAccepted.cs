using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class AnswerAccepted : Event
    {
        public string AnswerId { get; set; }
    }
}