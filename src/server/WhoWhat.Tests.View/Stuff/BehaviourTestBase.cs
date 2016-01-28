using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platform.Common.AppSettings;
using Platform.Dispatching;
using Platform.Domain;
using Platform.Testing.ViewModel;
using Platform.Uniform;
using Platform.Uniform.InMemory;
using StructureMap;
using WhoWhat.Core;
using WhoWhat.Core.StructureMap;
using WhoWhat.View;

namespace WhoWhat.Tests.View
{
    [TestClass]
    public class BehaviourTestBase<TDocument> : DocumentBehaviourValidator<TDocument> where TDocument : class, IDocument, new()
    {
        [TestInitialize]
        public void TestInitialize()
        {
            IContainer container = ObjectFactory.Container;
            var dispatcher = Dispatcher.Create(config =>
            {
                config.SetMessageHandlerMarker(typeof (IMessageHandler<>))
                      .SetServiceLocator(new StructureMapServiceProvider(container))
                      .EnableHandlingPriority()
                      .AddScanRule(typeof (Namespace_ViewProject).Assembly, new List<string>
                      {
                          "WhoWhat.View.ViewHandlers"
                      });

                return config;
            });
            
            var uniformContext = new InMemoryContext();
            var viewContext = new ViewContext(uniformContext);
            container.Configure(config => config.For<ViewContext>().Singleton().Use(viewContext));

            container.Configure(config => config.Scan(scan =>
            {
                scan.Assembly(typeof (Namespace_ViewProject).Assembly);
                scan.WithDefaultConventions();
                scan.AddAllTypesOf(typeof(IDocument));
            }));

            foreach (IDocument document in container.GetAllInstances<IDocument>())
            {
                Type documentType = document.GetType();
                Type storeType = typeof(IDocumentStore<>).MakeGenericType(documentType);
                
                MethodInfo method = uniformContext.GetType().GetMethod("GetDocumentStore").MakeGenericMethod(documentType);
                container.Configure(config => config.For(storeType).Use(method.Invoke(uniformContext, null)));
            }

            base.Initialize(dispatcher, uniformContext, new GuidEntityIdGenerator());
        }
        
        [TestCleanup]
        public void TestCleanup()
        {
        }
    }
}