using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class QuestionAnswered : Event
    {
        public string AuthorId { get; set; }
        public string Body { get; set; }
        public string AnswerId { get; set; }
    }
}
