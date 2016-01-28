using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class AnswerUnvoted : Event
    {
        public string VoterId { get; set; }
        public string AnswerId { get; set; }
    }
}