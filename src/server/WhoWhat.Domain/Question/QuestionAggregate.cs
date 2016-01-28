using Platform.Domain;
using WhoWhat.Domain.Question.Commands;
using WhoWhat.Domain.Question.Events;

namespace WhoWhat.Domain.Question
{
    public class QuestionAggregate : AggregateRoot<QuestionState>
    {
        [AggregateCtor]
        public void When(AskQuestion command)
        {
            If(state => state.IsClosed).FailWith("Question is closed.");
            ApplyEvent<QuestionAsked>(@event => @event.InjectFromCommand(command));
        }
        
        public void When(ChangeQuestionInfo command)
        {
            If(state => state.IsClosed).FailWith("Question is closed.");
            ApplyEvent<QuestionInfoChanged>(@event => @event.InjectFromCommand(command));
        }

        public void When(ChangeQuestionImage command)
        {
            If(state => state.IsClosed).FailWith("Question is closed.");
            ApplyEvent<QuestionInfoChanged>(@event => @event.InjectFromCommand(command));
        }
        
        public void When(VoteQuestionUp command)
        {
            If(state => state.AuthorId == command.VoterId).FailWith("User cannot vote for his own question.");
            
            if (State.Votes.ContainsKey(command.VoterId))
            {
                If(state => state.Votes[command.VoterId].Direction == VoteDirection.Up).FailWith("User has already upvoted.");

                ApplyEvent(new QuestionUnvoted
                {
                    AggregateId = command.AggregateId,
                    VoterId = command.VoterId,
                });
            }

            ApplyEvent(new QuestionVotedUp()
            {
                AggregateId = command.AggregateId,
                VoterId = command.VoterId,
                RatingShift = QuestionScoreTable.QuestionVotedUp
            });
        }

        public void When(VoteQuestionDown command)
        {
            If(state => state.AuthorId == command.VoterId).FailWith("User cannot vote for his own question.");

            if (State.Votes.ContainsKey(command.VoterId))
            {
                If(state => state.Votes[command.VoterId].Direction == VoteDirection.Down).FailWith("User has already downvoted.");
                
                ApplyEvent(new QuestionUnvoted
                {
                    AggregateId = command.AggregateId,
                    VoterId = command.VoterId,
                });
            }

            ApplyEvent(new QuestionVotedDown()
            {
                AggregateId = command.AggregateId,
                VoterId = command.VoterId,
                RatingShift = QuestionScoreTable.QuestionVotedDown
            });
        }

        public void When(UnvoteQuestion command)
        {
            If(state => state.AuthorId == command.VoterId).FailWith("User cannot vote for his own question.");
            If(state => !state.Votes.ContainsKey(command.VoterId)).FailWith("Can't unvote the question. User has not voted.");

            ApplyEvent<QuestionUnvoted>(@event => @event.InjectFromCommand(command));
        }

        public void When(CloseQuestion command)
        {
            If(state => state.IsClosed).FailWith("Question is already closed.");
            ApplyEvent<QuestionClosed>(@event => @event.InjectFromCommand(command));
        }

        public void When(RemoveQuestion command)
        {
            ApplyEvent<QuestionRemoved>(@event => @event.InjectFromCommand(command));
        }

        #region Answering

        public void When(AnswerQuestion command)
        {
            ApplyEvent<QuestionAnswered>(@event => @event.InjectFromCommand(command));
        }

        public void When(ChangeAnswerBody command)
        {
            ApplyEvent<AnswerBodyChanged>(@event => @event.InjectFromCommand(command));
        }

        public void When(AcceptAnswer command)
        {
            If(() => State.IsResolved).FailWith("Question is already marked as resolved.");
            If(() => State.Answers[command.AnswerId].AuthorId == State.AuthorId).FailWith("User cannot accept his own question.");
            ApplyEvent<AnswerAccepted>(@event => @event.InjectFromCommand(command));
        }

        public void When(UnacceptAnswer command)
        {
            If(state => !state.IsResolved).FailWith("Question is not resolved.");
            ApplyEvent<AnswerUnaccepted>(@event => @event.InjectFromCommand(command));
        }

        public void When(VoteAnswerUp command)
        {
            If(state => !state.Answers.ContainsKey(command.AnswerId)).FailWith("Answer was not found in the question.");

            Answer answer = State.Answers[command.AnswerId];
            If(() => answer.AuthorId == command.VoterId).FailWith("User cannot vote for his own answer.");

            if (answer.Votes.ContainsKey(command.VoterId))
            {
                If(() => answer.Votes[command.VoterId] == VoteDirection.Up).FailWith("User has already upvoted the answer.");

                ApplyEvent(new AnswerUnvoted()
                {
                    AggregateId = command.AggregateId,
                    AnswerId = command.AnswerId,
                    VoterId = command.VoterId
                });
            }

            ApplyEvent(new AnswerVotedUp()
            {
                AggregateId = command.AggregateId,
                VoterId = command.VoterId,
                AnswerId = command.AnswerId,
                RatingShift = QuestionScoreTable.AnswerVotedUp
            });
        }

        public void When(VoteAnswerDown command)
        {
            If(state => !state.Answers.ContainsKey(command.AnswerId)).FailWith("Answer was not found in the question.");

            Answer answer = State.Answers[command.AnswerId];
            If(() => answer.AuthorId == command.VoterId).FailWith("User cannot vote for his own answer.");

            if (answer.Votes.ContainsKey(command.VoterId))
            {
                If(() => answer.Votes[command.VoterId] == VoteDirection.Down).FailWith("User has already downvoted the answer.");

                ApplyEvent(new AnswerUnvoted()
                {
                    AggregateId = command.AggregateId,
                    AnswerId = command.AnswerId,
                    VoterId = command.VoterId
                });
            }

            ApplyEvent(new AnswerVotedDown()
            {
                AggregateId = command.AggregateId,
                VoterId = command.VoterId,
                AnswerId = command.AnswerId,
                RatingShift = QuestionScoreTable.AnswerVotedDown
            });
        }

        public void When(UnvoteAnswer command)
        {
            If(() => !State.Answers.ContainsKey(command.AnswerId)).FailWith("Answer was not found in the question.");
            ApplyEvent<AnswerUnvoted>(@event => @event.InjectFromCommand(command));
        }

        public void When(RemoveAnswer command)
        {
            If(() => !State.Answers.ContainsKey(command.AnswerId)).FailWith("Answer was not found in the question.");
            ApplyEvent<AnswerRemoved>(@event => @event.InjectFromCommand(command));
        }

        #endregion
    }
}
