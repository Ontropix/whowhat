using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using StructureMap;

namespace WhoWhat.Core.StructureMap
{
    public class StructureMapServiceLocator : ServiceLocatorImplBase
    {
        private readonly IContainer container;

        public StructureMapServiceLocator(IContainer container)
        {
            this.container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return string.IsNullOrEmpty(key)
                ? container.GetInstance(serviceType)
                : container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            foreach (object obj in container.GetAllInstances(serviceType))
            {
                yield return obj;
            }
        }
    }

}
