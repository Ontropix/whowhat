using System.Collections.Generic;
using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class QuestionAsked : Event
    {
        public string Body { get; set; }

        public string ImageUri { get; set; }

        public string ThumbnailUri { get; set; }

        public HashSet<string> Tags { get; set; }

        public string AuthorId { get; set; }
    }
}
