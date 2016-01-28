using System.Threading.Tasks;
using Platform.Dispatching;
using Platform.ServiceBus;

namespace WhoWhat.Core
{
    public class DispatcherEndpointListener
    {
        private readonly Dispatcher _dispatcher;

        public DispatcherEndpointListener(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void DispatchAsync(TransportMessage message)
        {
            Task.Factory.StartNew(() => Dispatch(message));
        }

        public void Dispatch(TransportMessage message)
        {
            _dispatcher.Dispatch(message.Message);
        }
    }
}
