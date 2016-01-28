using StructureMap;

namespace WhoWhat.Core
{
    public interface IConfigurator
    {
        void Configure(IContainer container);
    }
}
