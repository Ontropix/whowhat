using System;
using Platform.Domain;

namespace WhoWhat.Domain.WorkflowHandlers
{
    [HandlingStarted]
    public abstract class WorkflowHandler
    {
        private readonly ICommandBus _commandBus;
        protected readonly ReadOnlyViewContext _context;
        protected readonly IEntityIdGenerator _idGenerator;

        protected WorkflowHandler(ReadOnlyViewContext context, ICommandBus commandBus, IEntityIdGenerator idGenerator)
        {
            _context = context;
            _commandBus = commandBus;
            _idGenerator = idGenerator;
        }

        protected void SendCommand(Command command, string userId)
        {
            if (string.IsNullOrWhiteSpace(command.Metadata.SenderId))
            {
                command.Metadata.SenderId = userId;
            }

            command.Metadata.CreatedDate = DateTime.UtcNow;
            command.Metadata.CommandId = _idGenerator.Generate();

            _commandBus.Send(command);
        }
    }
}