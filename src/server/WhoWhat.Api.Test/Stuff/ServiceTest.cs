using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platform.Domain;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceInterface.Testing;
using StructureMap;
using StructureMap.Graph;
using WhoWhat.Api.Test.Mocks;
using WhoWhat.Core;
using WhoWhat.Core.Configuration;
using WhoWhat.Core.Content;
using WhoWhat.Core.Push;
using WhoWhat.Core.StructureMap;
using WhoWhat.Domain.User;
using WhoWhat.Domain.User.Commands;

namespace WhoWhat.Api.Test
{
    [TestClass]
    public class ServiceTest
    {
        protected BasicAppHost AppHost { get; private set; }
        protected Funq.Container Container { get; private set; }
        protected MockPusher Pusher { get; private set; }
        protected IEntityIdGenerator IdGenerator { get; private set; }
        protected QuestionHelper QuestionHelper { get; private set; }

        private readonly TypedSessionProvider _sessionProvider = new TypedSessionProvider();

        protected void SetSession(CustomUserSession session)
        {
            _sessionProvider.Session = session;
        }

        protected void OpenSession(string userId)
        {
            _sessionProvider.Session = new CustomUserSession()
            {
                UserId = userId
            };
        }

        protected void CreateInMemoryUser(string userId)
        {
            var commandBus = AppHost.Container.Resolve<CommandBus>();

            var command = new RegisterUser
            {
                AggregateId = userId,
                FirstName = "Tolik",
                LastName = "Anabolik",
                AccessToken = "Fake_token_1",
                ThirdPartyId = "Third_party_id",

                PhotoSmallUri = "http://code9.biz/img/small_test.jpg",
                PhotoBigUri = "http://code9.biz/img/big_test.jpg",

                LoginType = UserLoginType.Vk
            };

            commandBus.Send(command, userId);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            AppHost = new BasicAppHost().Init();
            Container = AppHost.Container;
            Pusher = new MockPusher();
            IdGenerator = new GuidEntityIdGenerator();
            QuestionHelper = new QuestionHelper(Container);

            ObjectFactory.Initialize(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            }));

            Container.Adapter = new StructureMapContainerAdapter();
            
            Container.Register<ISessionFactory>(x => new InMemorySessionFactory());
            Container.Register<IImageStorageService>(x => new MockImageStorageService());
            Container.Register<ITypedSessionProvider>(x => _sessionProvider);
            
            //Register all services
            Container.RegisterAutoWired<UsersService>();
            Container.RegisterAutoWired<QuestionService>();
            Container.RegisterAutoWired<AnswerService>();
            Container.RegisterAutoWired<SearchService>();

            new TestConfig().Configure(ObjectFactory.Container);
            ObjectFactory.Container.Configure(config => config.For<IPusher>().Use(Pusher).Singleton());
            ObjectFactory.Container.AssertConfigurationIsValid();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            AppHost.Dispose();
            AppHost = null;

            ObjectFactory.Container.Dispose();
        }
    }
}
