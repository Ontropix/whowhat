using System;
using System.Collections.Generic;
using Platform.Uniform;
using WhoWhat.View.Payload;

namespace WhoWhat.View.Documents
{
    public class AnswerDocument: IDocument
    {
        public AnswerDocument()
        {
            this.Votes = new Dictionary<string, VotePayload>();
        }

        public string Id { get; set; }

        public string QuestionId { get; set; }
        public string AuthorId { get; set; }

        public string Body { get; set; }
        public int Rating { get; set; }
        public bool IsAccepted { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }

        /// <summary>
        /// Key: VoterId, Value: VoteDirection
        /// </summary>
        public Dictionary<string, VotePayload> Votes { get; set; }
        public int VotesCount { get; set; }
    }
}
