using Platform.Dispatching;
using Platform.Domain;
using WhoWhat.Domain.Question.Commands;

namespace WhoWhat.Domain.Question
{
    public class QuestionApplicationService :
        ApplicationService<QuestionAggregate>,

        IMessageHandler<AskQuestion>,

        IMessageHandler<ChangeQuestionInfo>,
        IMessageHandler<ChangeQuestionImage>,
        
        IMessageHandler<RemoveQuestion>,
        IMessageHandler<CloseQuestion>,
        
        IMessageHandler<VoteQuestionUp>,
        IMessageHandler<VoteQuestionDown>,
        IMessageHandler<UnvoteQuestion>,
        
        
        IMessageHandler<AnswerQuestion>,
        
        IMessageHandler<ChangeAnswerBody>,
        IMessageHandler<AcceptAnswer>,
        IMessageHandler<UnacceptAnswer>,

        IMessageHandler<RemoveAnswer>,

        IMessageHandler<VoteAnswerUp>,
        IMessageHandler<VoteAnswerDown>,
        IMessageHandler<UnvoteAnswer>
    {
        public QuestionApplicationService(AggregateRepository<QuestionAggregate> aggregateRepository, IEntityIdGenerator idGenerator, EventBus eventBus)
            : base(aggregateRepository, idGenerator, eventBus)
        {
        }

        public void Handle(AskQuestion message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(AnswerQuestion message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(RemoveQuestion message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(CloseQuestion message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(VoteQuestionUp message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(VoteQuestionDown message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }
        
        public void Handle(UnvoteQuestion message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(ChangeQuestionInfo message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(ChangeQuestionImage message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(AcceptAnswer message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(UnacceptAnswer message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(VoteAnswerUp message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(VoteAnswerDown message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(UnvoteAnswer message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(ChangeAnswerBody message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }

        public void Handle(RemoveAnswer message)
        {
            this.Process(message, (aggregate, command) => aggregate.When(command));
        }
    }
}