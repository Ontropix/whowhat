using Platform.Dispatching;
using Platform.Uniform;
using WhoWhat.Domain.Question.Events;
using WhoWhat.View.Documents;
using WhoWhat.View.Payload;

namespace WhoWhat.View.ViewHandlers
{
    public class AnswerViewHandler : ViewHandler<AnswerDocument>,
                                     IMessageHandler<QuestionAnswered>,
                                     IMessageHandler<QuestionRemoved>,
                                     IMessageHandler<AnswerBodyChanged>,
                                     IMessageHandler<AnswerAccepted>,
                                     IMessageHandler<AnswerUnaccepted>,
                                     IMessageHandler<AnswerVotedUp>,
                                     IMessageHandler<AnswerVotedDown>,
                                     IMessageHandler<AnswerUnvoted>,
                                     IMessageHandler<AnswerRemoved>
    {
        public AnswerViewHandler(IDocumentStore<AnswerDocument> documentStore, ViewContext viewContext)
            : base(documentStore, viewContext)
        {
        }

        public void Handle(QuestionAnswered message)
        {
            var answerDocument = new AnswerDocument()
            {
                Id = message.AnswerId,
                
                AuthorId = message.AuthorId,
                QuestionId = message.AggregateId,

                Body = message.Body,
                CreatedAt = message.Metadata.CreatedAt
            };

            DocumentStore.Insert(answerDocument);
        }

        public void Handle(QuestionRemoved message)
        {
            DocumentStore.Delete(x => x.QuestionId == message.AggregateId);
        }

        public void Handle(AnswerBodyChanged message)
        {
            DocumentStore.Update(message.AnswerId, answer =>
            {
                answer.Body = message.NewBody;
                answer.EditedAt = message.Metadata.CreatedAt;
            });
        }

        public void Handle(AnswerAccepted message)
        {
            DocumentStore.Update(message.AnswerId, document => document.IsAccepted = true);
        }

        public void Handle(AnswerUnaccepted message)
        {
            DocumentStore.Update(message.AnswerId, document => document.IsAccepted = false);
        }

        public void Handle(AnswerVotedUp message)
        {
            DocumentStore.Update(message.AnswerId, answer =>
            {
                answer.Rating += message.RatingShift;
                answer.Votes.Add(message.VoterId, new VotePayload(VoteDirection.Up, message.RatingShift));
            });
        }

        public void Handle(AnswerVotedDown message)
        {
            DocumentStore.Update(message.AnswerId, answer =>
            {
                answer.Rating += message.RatingShift;
                answer.Votes.Add(message.VoterId, new VotePayload(VoteDirection.Down, message.RatingShift));
            });
        }

        public void Handle(AnswerUnvoted message)
        {
            DocumentStore.Update(message.AnswerId, answer =>
            {
                VotePayload vote = answer.Votes[message.VoterId];
                answer.Rating -= vote.RatingShift;
                answer.Votes.Remove(message.VoterId);
            });
        }

        public void Handle(AnswerRemoved message)
        {
            DocumentStore.Delete(message.AnswerId);
        }
    }
}