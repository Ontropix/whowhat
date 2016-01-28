using System;
using Platform.Domain;

namespace WhoWhat.Api.Test.Mocks
{
    public class EntityIdGenerator : IEntityIdGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString();
        }
    }
}