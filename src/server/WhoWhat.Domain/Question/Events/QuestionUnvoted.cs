using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class QuestionUnvoted : Event
    {
        public string VoterId { get; set; }
    }
}