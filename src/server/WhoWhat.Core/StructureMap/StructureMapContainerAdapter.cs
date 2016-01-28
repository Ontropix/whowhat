using ServiceStack.Configuration;
using StructureMap;

namespace WhoWhat.Core.StructureMap
{
    public class StructureMapContainerAdapter : IContainerAdapter
    {
        public T TryResolve<T>()
        {
            return ObjectFactory.Container.TryGetInstance<T>();
        }

        public T Resolve<T>()
        {
            return ObjectFactory.Container.TryGetInstance<T>();
        }
    }
}
