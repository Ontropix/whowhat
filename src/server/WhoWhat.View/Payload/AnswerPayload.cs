using System;
using System.Collections.Generic;

namespace WhoWhat.View.Payload
{
    public class AnswerPayload
    {
        public AnswerPayload()
        {
            this.Votes = new Dictionary<string, VotePayload>();
        }

        public string Id { get; set; }
        public string AuthorId { get; set; }

        public string Body { get; set; }
        public int Rating { get; set; }
        public bool IsAccepted { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }

        /// <summary>
        /// Key: VoterId, Value: VotePayload
        /// </summary>
        public Dictionary<string, VotePayload> Votes { get; set; }
    }
}
