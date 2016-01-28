using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class QuestionImageChanged : Event
    {
        public string ImageUri { get; set; }
        public string ThumbnailUri { get; set; }
    }
}