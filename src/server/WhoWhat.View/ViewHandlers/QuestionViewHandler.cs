using Platform.Dispatching;
using Platform.Uniform;
using WhoWhat.Domain.Question.Events;
using WhoWhat.View.Documents;
using WhoWhat.View.Payload;

namespace WhoWhat.View.ViewHandlers
{
    public class QuestionViewHandler : ViewHandler<QuestionDocument>,
                                       IMessageHandler<QuestionAsked>,
                                       IMessageHandler<QuestionAnswered>,
                                       IMessageHandler<QuestionRemoved>,
                                       IMessageHandler<QuestionClosed>,
                                       IMessageHandler<QuestionVotedUp>,
                                       IMessageHandler<QuestionVotedDown>,
                                       IMessageHandler<QuestionUnvoted>,
                                       IMessageHandler<QuestionInfoChanged>,
                                       IMessageHandler<QuestionImageChanged>,
                                       IMessageHandler<AnswerBodyChanged>,
                                       IMessageHandler<AnswerAccepted>,
                                       IMessageHandler<AnswerUnaccepted>,
                                       IMessageHandler<AnswerVotedUp>,
                                       IMessageHandler<AnswerVotedDown>,
                                       IMessageHandler<AnswerUnvoted>,
                                       IMessageHandler<AnswerRemoved>
    {
        public QuestionViewHandler(IDocumentStore<QuestionDocument> documentStore, ViewContext viewContext) : base(documentStore, viewContext)
        {
        }

        public void Handle(QuestionAsked message)
        {
            DocumentStore.Insert(message.AggregateId, question =>
            {
                question.Body = message.Body;
                question.Tags = message.Tags;
                question.AuthorId = message.AuthorId;
                question.Tags = message.Tags;
                question.CreatedAt = message.Metadata.CreatedAt;

                question.ImageUri = message.ImageUri;
                question.ThumbnailUri = message.ThumbnailUri;
            });
        }

        public void Handle(QuestionAnswered message)
        {
            var answerPayload = new AnswerPayload()
            {
                Id = message.AnswerId,
                AuthorId = message.AuthorId,

                Body = message.Body,
                Rating = 0,
                IsAccepted = false,
                CreatedAt = message.Metadata.CreatedAt
            };

            DocumentStore.Update(message.AggregateId, question =>
            {
                question.AnswersCount++;
                question.Answers.Add(message.AnswerId, answerPayload);
            });
        }

        public void Handle(QuestionRemoved message)
        {
            DocumentStore.Delete(message.AggregateId);
        }

        public void Handle(QuestionClosed message)
        {
            DocumentStore.Update(message.AggregateId, question => question.IsClosed = true);
        }

        public void Handle(QuestionVotedUp message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                question.Votes.Add(message.VoterId, new VotePayload(VoteDirection.Up, message.RatingShift));
                question.Rating += message.RatingShift;
                question.VotesCount++;
            });
        }

        public void Handle(QuestionVotedDown message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                question.Votes.Add(message.VoterId, new VotePayload(VoteDirection.Down, message.RatingShift));
                question.Rating += message.RatingShift;
                question.VotesCount++;
            });
        }

        public void Handle(QuestionUnvoted message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                VotePayload vote = question.Votes[message.VoterId];
                question.Rating -= vote.RatingShift;
                question.Votes.Remove(message.VoterId);
                question.VotesCount--;
            });
        }

        public void Handle(QuestionInfoChanged message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                question.Body = message.Body;
                question.Tags = message.Tags;
                question.EditedAt = message.Metadata.CreatedAt;
            });
        }

        public void Handle(QuestionImageChanged message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                question.ImageUri = message.ImageUri;
                question.ThumbnailUri = message.ThumbnailUri;
                question.EditedAt = message.Metadata.CreatedAt;
            });
        }

        public void Handle(AnswerBodyChanged message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                question.Answers[message.AnswerId].Body = message.NewBody;
                question.Answers[message.AnswerId].EditedAt = message.Metadata.CreatedAt;
            });
        }

        public void Handle(AnswerAccepted message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                question.IsResolved = true;
                question.Answers[message.AnswerId].IsAccepted = true;
            });
        }

        public void Handle(AnswerUnaccepted message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                question.IsResolved = false;
                question.Answers[message.AnswerId].IsAccepted = false;
            });
        }

        public void Handle(AnswerVotedUp message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                AnswerPayload answer = question.Answers[message.AnswerId];
                answer.Rating += message.RatingShift;
                answer.Votes.Add(message.VoterId, new VotePayload(VoteDirection.Up, message.RatingShift));
            });
        }

        public void Handle(AnswerVotedDown message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                AnswerPayload answer = question.Answers[message.AnswerId];
                answer.Rating += message.RatingShift;
                answer.Votes.Add(message.VoterId, new VotePayload(VoteDirection.Down, message.RatingShift));
            });
        }

        public void Handle(AnswerUnvoted message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                AnswerPayload answer = question.Answers[message.AnswerId];
                VotePayload vote = answer.Votes[message.VoterId];
                answer.Rating -= vote.RatingShift;
                answer.Votes.Remove(message.VoterId);
            });
        }

        public void Handle(AnswerRemoved message)
        {
            DocumentStore.Update(message.AggregateId, question =>
            {
                AnswerPayload answer = question.Answers[message.AnswerId];
                if (answer.IsAccepted)
                {
                    question.IsResolved = false;
                }

                question.Answers.Remove(message.AnswerId);
                question.AnswersCount--;
            });
        }
    }
}