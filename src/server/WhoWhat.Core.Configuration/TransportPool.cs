using System.Collections.Generic;
using Platform.ServiceBus;
using Platform.ServiceBus.InMemory;
using Platform.ServiceBus.Msmq;

namespace WhoWhat.Core.Configuration
{
    public class TransportPool
    {
        internal enum ServiceBusTransportType
        {
            MSMQ = 0,
            InMemory = 10,
            InMemoryAsync = 20
        }

        private static readonly Dictionary<ServiceBusTransportType, ITransport> _transportMap = new Dictionary<ServiceBusTransportType, ITransport>
        {
            { ServiceBusTransportType.MSMQ, new MsmqTransport() },
            { ServiceBusTransportType.InMemory, new InMemoryTransport() },
            { ServiceBusTransportType.InMemoryAsync, new InMemoryAsyncTransport() },
        };

        private static ITransport GetTransportByType(ServiceBusTransportType transportType)
        {
            return _transportMap[transportType];
        }

        public static ITransport GetMsmq()
        {
            return GetTransportByType(ServiceBusTransportType.MSMQ);
        }

        public static ITransport GetInMemory()
        {
            return GetTransportByType(ServiceBusTransportType.InMemory);
        }

        public static ITransport GetInMemoryAsync()
        {
            return GetTransportByType(ServiceBusTransportType.InMemoryAsync);
        }
    }
}