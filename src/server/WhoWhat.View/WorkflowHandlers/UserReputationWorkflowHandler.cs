using Platform.Dispatching;
using Platform.Domain;
using WhoWhat.Domain.Question.Events;
using WhoWhat.Domain.User;
using WhoWhat.Domain.User.Commands;
using WhoWhat.View.Documents;
using WhoWhat.View.Payload;

namespace WhoWhat.View.WorkflowHandlers
{
    public class UserReputationWorkflowHandler : WorkflowHandler,
                                         IMessageHandler<QuestionVotedUp>,
                                         IMessageHandler<QuestionVotedDown>,
                                         IMessageHandler<QuestionUnvoted>,
                                         IMessageHandler<AnswerAccepted>,
                                         IMessageHandler<AnswerUnaccepted>,
                                         IMessageHandler<AnswerVotedUp>,
                                         IMessageHandler<AnswerVotedDown>,
                                         IMessageHandler<AnswerUnvoted>
    {
        public UserReputationWorkflowHandler(ViewContext context, CommandBus commandBus, IEntityIdGenerator idGenerator)
            : base(context, commandBus, idGenerator)
        {
        }

        private void ChangeUserReputation(string userId, int shift, string senderId)
        {
            var command = new ChangeUserReputation()
            {
                AggregateId = userId,
                Shift = shift
            };

            SendCommand(command, senderId);
        }

        public void Handle(QuestionVotedUp message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);
            ChangeUserReputation(question.AuthorId, UserScoreTable.QuestionVotedUp, message.Metadata.SenderId);
        }

        public void Handle(QuestionVotedDown message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);
            ChangeUserReputation(question.AuthorId, UserScoreTable.QuestionVotedDown, message.Metadata.SenderId);
        }

        public void Handle(QuestionUnvoted message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            int shift = -(question.Votes[message.VoterId].Direction == VoteDirection.Up
                ? UserScoreTable.QuestionVotedUp
                : UserScoreTable.QuestionVotedDown);

            ChangeUserReputation(question.AuthorId, shift, message.Metadata.SenderId);
        }

        public void Handle(AnswerAccepted message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);
            ChangeUserReputation(question.AuthorId, UserScoreTable.QuestionResolved, message.Metadata.SenderId);
            ChangeUserReputation(question.Answers[message.AnswerId].AuthorId, UserScoreTable.AnswerAccepted, message.Metadata.SenderId);
        }

        public void Handle(AnswerUnaccepted message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);
            ChangeUserReputation(question.AuthorId, -(UserScoreTable.QuestionResolved), message.Metadata.SenderId);
            ChangeUserReputation(question.Answers[message.AnswerId].AuthorId, -(UserScoreTable.AnswerAccepted), message.Metadata.SenderId);
        }

        public void Handle(AnswerVotedUp message)
        {
            AnswerDocument answer = _context.Answers.GetById(message.AnswerId);
            ChangeUserReputation(answer.AuthorId, UserScoreTable.AnswerVotedUp, message.Metadata.SenderId);
        }

        public void Handle(AnswerVotedDown message)
        {
            AnswerDocument answer = _context.Answers.GetById(message.AnswerId);
            ChangeUserReputation(answer.AuthorId, UserScoreTable.AnswerVotedDown, message.Metadata.SenderId);
        }

        public void Handle(AnswerUnvoted message)
        {
            AnswerDocument answer = _context.Answers.GetById(message.AnswerId);

            int shift = -(answer.Votes[message.VoterId].Direction == VoteDirection.Up
                ? UserScoreTable.AnswerVotedUp
                : UserScoreTable.AnswerVotedDown);

            ChangeUserReputation(answer.AuthorId, shift, message.Metadata.SenderId);
        }
    }
}