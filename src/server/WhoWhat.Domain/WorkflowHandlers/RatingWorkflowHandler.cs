using Platform.Domain;
using WhoWhat.Domain.Question;
using WhoWhat.Domain.Question.Events;
using WhoWhat.Domain.User.Commands;
using WhoWhat.Domain.WorkflowHandlers;
using WhoWhat.View.Documents;

namespace WhoWhat.View.WorkflowHandlers
{
    public class RatingWorkflowHandler : WorkflowHandler,
        IMessageHandler<QuestionVotedUp>,
        IMessageHandler<QuestionVotedDown>,
        IMessageHandler<QuestionUnvoted>,
        IMessageHandler<AnswerMarkedAsAccepted>,
        IMessageHandler<AnswerUnmarkedAsAccepted>,
        IMessageHandler<AnswerVotedUp>,
        IMessageHandler<AnswerVotedDown>,
        IMessageHandler<AnswerUnvoted>
    {
        public RatingWorkflowHandler(ReadOnlyViewContext context, ICommandBus commandBus, IEntityIdGenerator idGenerator)
            : base(context, commandBus, idGenerator)
        {
        }

        private static RaiseUserReputation BuildRaiseUserReputationCommand(string userId, int score)
        {
            var command = new RaiseUserReputation
            {
                AggregateId = userId,
                Score = score
            };

            return command;
        }

        private static ReduceUserReputation BuildReduceUserReputationCommand(string userId, int score)
        {
            var command = new ReduceUserReputation
            {
                AggregateId = userId,
                Score = score
            };

            return command;
        }

        public void Handle(QuestionVotedUp message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            var command = BuildRaiseUserReputationCommand(question.Author.UserId, ScoreTable.QuestionVotedUp);
            SendCommand(command, message.Metadata.SenderId);
        }

        public void Handle(QuestionVotedDown message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            var command = BuildReduceUserReputationCommand(question.Author.UserId, ScoreTable.QuestionVotedDown);
            SendCommand(command, message.Metadata.SenderId);
        }

        public void Handle(QuestionUnvoted message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            //if we cancel positive vote -- we'll subtract score, otherwise append score
            if (question.Votes[message.VoterId] == VoteDirection.Up)
            {
                var command = BuildReduceUserReputationCommand(question.Author.UserId, ScoreTable.QuestionVotedUp);
                SendCommand(command, message.Metadata.SenderId);
            }
            else
            {
                var command = BuildRaiseUserReputationCommand(question.Author.UserId, ScoreTable.QuestionVotedDown);
                SendCommand(command, message.Metadata.SenderId);
            }
        }

        public void Handle(AnswerMarkedAsAccepted message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);
            var questionAuthor = BuildRaiseUserReputationCommand(question.Author.UserId, ScoreTable.QuestionMarkedAsAnswered);
            SendCommand(questionAuthor, message.Metadata.SenderId);

            AnswerDocument answer = _context.Answers.GetById(message.AnswerId);
            var answerAuthor = BuildRaiseUserReputationCommand(answer.Author.UserId, ScoreTable.AnswerMarkedAsAccepted);
            SendCommand(answerAuthor, message.Metadata.SenderId);
        }

        public void Handle(AnswerUnmarkedAsAccepted message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);
            var questionAuthor = BuildReduceUserReputationCommand(question.Author.UserId, ScoreTable.QuestionMarkedAsAnswered);
            SendCommand(questionAuthor, message.Metadata.SenderId);

            AnswerDocument answer = _context.Answers.GetById(message.AnswerId);
            var answerAuthor = BuildReduceUserReputationCommand(answer.Author.UserId, ScoreTable.QuestionMarkedAsAnswered);
            SendCommand(answerAuthor, message.Metadata.SenderId);
        }

        public void Handle(AnswerVotedUp message)
        {
            AnswerDocument answer = _context.Answers.GetById(message.AnswerId);
            var answerAuthor = BuildRaiseUserReputationCommand(answer.Author.UserId, ScoreTable.AnswerVotedUp);
            SendCommand(answerAuthor, message.Metadata.SenderId);
        }

        public void Handle(AnswerVotedDown message)
        {
            AnswerDocument answer = _context.Answers.GetById(message.AnswerId);
            var answerAuthor = BuildReduceUserReputationCommand(answer.Author.UserId, ScoreTable.AnswerVotedDown);
            SendCommand(answerAuthor, message.Metadata.SenderId);
        }

        public void Handle(AnswerUnvoted message)
        {
            AnswerDocument answer = _context.Answers.GetById(message.AnswerId);

            //if we cancel positive vote -- we'll subtract score, otherwise append score
            if (answer.Votes[message.VoterId] == VoteDirection.Up)
            {
                var command = BuildReduceUserReputationCommand(answer.Author.UserId, ScoreTable.AnswerVotedUp);
                SendCommand(command, message.Metadata.SenderId);
            }
            else
            {
                var command = BuildRaiseUserReputationCommand(answer.Author.UserId, ScoreTable.AnswerVotedDown);
                SendCommand(command, message.Metadata.SenderId);
            }
        }
    }
}