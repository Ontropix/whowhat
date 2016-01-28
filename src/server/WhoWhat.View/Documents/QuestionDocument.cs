using System;
using System.Collections.Generic;
using Platform.Uniform;
using WhoWhat.View.Payload;

namespace WhoWhat.View.Documents
{
    public class QuestionDocument : IDocument
    {
        public QuestionDocument()
        {
            this.Answers = new Dictionary<string, AnswerPayload>();
            this.Votes = new Dictionary<string, VotePayload>();
            this.Tags = new HashSet<string>();
        }

        public string Id { get; set; }

        public string Body { get; set; }

        public int Rating { get; set; }

        public int AnswersCount { get; set; }
        public int VotesCount { get; set; }

        public bool IsClosed { get; set; }
        public bool IsResolved { get; set; }
        
        public string ImageUri { get; set; }
        public string ThumbnailUri { get; set; }
        public HashSet<string> Tags { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }

        public string AuthorId { get; set; }

        /// <summary>
        /// Key: AnswerId, Value: AnswerPayload
        /// </summary>
        public Dictionary<string, AnswerPayload> Answers { get; set; }

        /// <summary>
        /// Key: VoterId, Value: VotePayload
        /// </summary>
        public Dictionary<string, VotePayload> Votes { get; set; }
    }
}