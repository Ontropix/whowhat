using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platform.Domain;
using Platform.EventStore.InMemory;
using Platform.Testing.Domain;
using WhoWhat.Core;

namespace WhoWhat.Tests.Domain
{
    [TestClass]
    public class BehaviourTestBase<TAggregate> : AggregateBehaviourValidator<TAggregate> where TAggregate : AggregateRoot, new()
    {
        protected readonly IEntityIdGenerator _idGenerator;

        public BehaviourTestBase() : base(new AggregateRepository<TAggregate>(new InMemoryEventStore()))
        {
            _idGenerator = new GuidEntityIdGenerator();
        }

        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestCleanup]
        public void TestCleanup()
        {
        }
    }
}