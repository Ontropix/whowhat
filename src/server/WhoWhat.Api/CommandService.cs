using Platform.Domain;
using ServiceStack.ServiceInterface;
using StructureMap.Attributes;
using WhoWhat.Core.UserService;
using WhoWhat.View;

namespace WhoWhat.Api
{
    public abstract class CommandService : Service
    {
        [SetterProperty]
        public IUserCacheService UserCacheService { get; set; }

        [SetterProperty]
        public ITypedSessionProvider TypedSessionProvider { get; set; }

        [SetterProperty]
        public CommandBus Bus { get; set; }

        [SetterProperty]
        public IEntityIdGenerator IdGenerator { get; set; }

        [SetterProperty]
        public ViewContext ViewContext { get; set; }

        public CustomUserSession TypedSession
        {
            get
            {
                return TypedSessionProvider != null
                           ? TypedSessionProvider.Session
                           : this.SessionAs<CustomUserSession>();
            }
        }

        protected void SendCommand(Command command)
        {
            Bus.Send(command, TypedSession.UserId);
        }
    }
}
