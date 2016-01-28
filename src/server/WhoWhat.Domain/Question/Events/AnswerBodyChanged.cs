using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class AnswerBodyChanged : Event
    {
        public string AnswerId { get; set; }
        public string NewBody { get; set; }
    }
}