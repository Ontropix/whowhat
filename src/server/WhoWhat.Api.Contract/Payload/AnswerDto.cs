using System;
using System.Collections.Generic;
using WhoWhat.View.Payload;

namespace WhoWhat.Api.Contract.Payload
{
    public class AnswerDto
    {
        public AnswerDto()
        {
            this.Votes = new Dictionary<string, VoteDirection>();
        }
        
        public string AnswerId { get; set; }

        public string QuestionId { get; set; }

        public string Body { get; set; }
        public bool IsAccepted { get; set; }
        public Author Author { get; set; }
        public int Rating { get; set; }

        //Dates
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }

        public Dictionary<string, VoteDirection> Votes { get; set; }
    }
}
