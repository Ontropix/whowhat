using System;
using System.Collections.Generic;
using WhoWhat.Domain.Question.Events;

namespace WhoWhat.Domain.Question
{
    public class QuestionState
    {
        public QuestionState()
        {
            this.Votes = new Dictionary<string, Vote>();
            this.Answers = new Dictionary<string, Answer>();
        }

        public string Id { get; set; }
        public string AuthorId { get; set; }

        public bool IsClosed { get; set; }

        public bool IsRemoved { get; set; }

        public bool IsResolved { get; set; }
        
        public int Rating { get; set; }
        
        internal Dictionary<string, Answer> Answers { get; set; }

        internal Dictionary<string, Vote> Votes { get; set; }


        public void Apply(QuestionAsked evnt)
        {
            this.Id = evnt.AggregateId;
            this.AuthorId = evnt.AuthorId;
        }

        public void Apply(QuestionInfoChanged evnt)
        {
        }

        public void Apply(QuestionVotedUp evnt)
        {
            this.Votes.Add(evnt.VoterId, new Vote(VoteDirection.Up, evnt.RatingShift));
        }

        public void Apply(QuestionVotedDown evnt)
        {
            this.Votes.Add(evnt.VoterId, new Vote(VoteDirection.Down, evnt.RatingShift));
        }

        public void Apply(QuestionUnvoted evnt)
        {
            this.Votes.Remove(evnt.VoterId);
        }

        public void Apply(QuestionClosed evnt)
        {
            IsClosed = true;
        }

        public void Apply(QuestionRemoved evnt)
        {
            this.IsRemoved = true;
        }


        #region Answering

        public void Apply(QuestionAnswered evnt)
        {
            this.Answers.Add(evnt.AnswerId, new Answer
            {
                Id = evnt.AnswerId,
                AuthorId = evnt.AuthorId,
                QuestionId = evnt.AggregateId,

                Body = evnt.Body,
                CreatedAt = DateTime.UtcNow
            });
        }

        public void Apply(AnswerAccepted evnt)
        {
            this.IsResolved = true;
        }

        public void Apply(AnswerUnaccepted evnt)
        {
            this.IsResolved = false;
        }

        public void Apply(AnswerVotedUp evnt)
        {
            this.Answers[evnt.AnswerId].Votes.Add(evnt.VoterId, VoteDirection.Up);
        }

        public void Apply(AnswerVotedDown evnt)
        {
            this.Answers[evnt.AnswerId].Votes.Add(evnt.VoterId, VoteDirection.Down);
        }

        public void Apply(AnswerUnvoted evnt)
        {
            this.Answers[evnt.AnswerId].Votes.Remove(evnt.VoterId);
        }

        public void Apply(AnswerRemoved evnt)
        {
            Answer answer = Answers[evnt.AnswerId];
            if (answer.IsAccepted)
            {
                this.IsResolved = false;
            }

            this.Answers.Remove(evnt.AnswerId);
        }

        #endregion
    }
}