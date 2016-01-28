using System;
using Platform.Dispatching;
using Platform.Domain;
using Platform.Logging;
using Platform.ServiceBus;
using Platform.Validation;
using Platform.Validation.Domain;
using StructureMap;
using WhoWhat.Domain;
using WhoWhat.View;

namespace WhoWhat.Core.Configuration
{
    internal class TransportConfigurator
    {
        internal TransportConfigurator RegisterDispatcher(IContainer container, bool enableLogs)
        {
            var serviceProvider = container.GetInstance<IServiceProvider>();
            Dispatcher dispatcher = Dispatcher.Create(config =>
            {
                config.SetMessageHandlerMarker(typeof (IMessageHandler<>))
                      .SetServiceLocator(serviceProvider)
                      .EnableHandlingPriority()
                      .AddScanRule(typeof (Namespace_ViewProject).Assembly)
                      .AddScanRule(typeof (Namespace_DomainProject).Assembly);

                if (enableLogs)
                {
                    config.AddInterceptor<CommandMessageLoggingInterceptor>();
                    config.AddInterceptor<EventMessageLoggingInterceptor>();
                }

                return config;
            });

            container.Configure(config => config.For<CommandMessageLoggingInterceptor>().Use<CommandMessageLoggingInterceptor>());
            container.Configure(config => config.For<EventMessageLoggingInterceptor>().Use<EventMessageLoggingInterceptor>());
            container.Configure(config => config.For<Dispatcher>().Singleton().Use(dispatcher));

            return this;
        }

        internal TransportConfigurator RegisterServiceBus(IContainer container)
        {
            var dispatcher = container.GetInstance<Dispatcher>();
            var messageListener = new DispatcherEndpointListener(dispatcher);

            ITransport transport = TransportPool.GetInMemory();
            var endpoint = transport.CreateEndpoint(new EndpointAddress("whowhat.input", Environment.MachineName), messageListener.Dispatch);

            ServiceBus bus = ServiceBus.Create(c => c.SetName("whowhat.bus")
                                                     .AddEntryPoint(endpoint)
                                                     .AddEndpoint(endpoint)
                );

            bus.Run();

            container.Configure(config => config.For<ServiceBus>().Singleton().Use(bus));

            return this;
        }

        internal TransportConfigurator RegisterDomainBuses(IContainer container)
        {
            var serviceBus = container.GetInstance<ServiceBus>();
            var serviceProvider = container.GetInstance<IServiceProvider>();
            var entityIdGenerator = container.GetInstance<IEntityIdGenerator>();
            var validationRegistry = new FluentValidatorsRegistry(serviceProvider);
            validationRegistry.Register(typeof(Namespace_DomainProject).Assembly);

            var commandValidationFactory = new FluentCommandValidationFacade(validationRegistry);
            var commandBus = new CommandBus(serviceBus, EndpointAddress.Create("whowhat.input", Environment.MachineName), entityIdGenerator, commandValidationFactory);
            var eventBus = new EventBus(serviceBus, EndpointAddress.Create("whowhat.input", Environment.MachineName));

            container.Configure(config =>
            {
                config.For<CommandBus>().Singleton().Use(commandBus);
                config.For<EventBus>().Singleton().Use(eventBus);
            });

            return this;
        }
    }
}