using System;
using System.Collections.Generic;

namespace WhoWhat.Api.Contract.Payload
{
    public class QuestionDto
    {
        public string QuestionId { get; set; }
     
        public string Body { get; set; }
        public int Rating { get; set; }

        public string Thumbnail { get; set; }
        public string ImageUri { get; set; }
        
        public HashSet<string> Tags { get; set; }

        public bool IsResolved { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }

        public int VotesCount { get; set; }
        public int AnswersCount { get; set; }

        public Author Author { get; set; }
    }
}
