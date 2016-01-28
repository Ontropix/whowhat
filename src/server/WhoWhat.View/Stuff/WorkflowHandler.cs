using System;
using Platform.Dispatching;
using Platform.Domain;

namespace WhoWhat.View
{
    [HandlingStarted]
    public abstract class WorkflowHandler
    {
        private readonly CommandBus _commandBus;
        protected readonly ViewContext _context;
        protected readonly IEntityIdGenerator _idGenerator;

        protected WorkflowHandler(ViewContext context, CommandBus commandBus, IEntityIdGenerator idGenerator)
        {
            _context = context;
            _commandBus = commandBus;
            _idGenerator = idGenerator;
        }

        protected void SendCommand(Command command, string senderId, string tenantId = null)
        {
            _commandBus.Send(command, senderId, tenantId);
        }
    }
}