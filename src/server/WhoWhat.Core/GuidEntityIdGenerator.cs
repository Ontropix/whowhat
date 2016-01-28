using System;
using Platform.Domain;

namespace WhoWhat.Core
{
    public class GuidEntityIdGenerator : IEntityIdGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString();
        }
    }
}