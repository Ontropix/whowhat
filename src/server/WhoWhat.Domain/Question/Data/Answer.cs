using System;
using System.Collections.Generic;

namespace WhoWhat.Domain.Question
{
    internal class Answer
    {
        public Answer()
        {
            this.Votes = new Dictionary<string, VoteDirection>();
        }

        public string Id { get; set; }
        public string QuestionId { get; set; }
        public string AuthorId { get; set; }

        public string Body { get; set; }
        public bool IsAccepted { get; set; }
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }

        public Dictionary<string, VoteDirection> Votes { get; set; }
    }
}