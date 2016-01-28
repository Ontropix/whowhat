using System.Collections.Generic;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceHost;

namespace WhoWhat.Api.Test.Mocks
{
    public class InMemorySessionFactory : ISessionFactory
    {
        public class MockSession : ISession
        {
            private Dictionary<string, object> storage = new Dictionary<string, object>();

            public void Set<T>(string key, T value)
            {
                storage[key] = value;
            }

            public T Get<T>(string key)
            {
                return (T)storage[key];
            }

            public object this[string key]
            {
                get { return storage[key]; }
                set { storage[key] = value; }
            }
        }

        private readonly MockSession session = new MockSession();

        public ISession GetOrCreateSession(IHttpRequest httpReq, IHttpResponse httpRes)
        {
            return session;
        }

        public ISession GetOrCreateSession()
        {
            return session;
        }
    }
}
