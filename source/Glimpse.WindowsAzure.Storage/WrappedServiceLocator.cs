using System;
using System.Collections.Generic;
using Glimpse.Core.Framework;
using Glimpse.WindowsAzure.Storage.Infrastructure.Core;

namespace Glimpse.WindowsAzure.Storage
{
    public class WrappedServiceLocator
        : IServiceLocator
    {
        private IServiceLocator innerServiceLocator;

        public WrappedServiceLocator()
        {
            var innerServiceLocatorType = Type.GetType("Glimpse.AspNet.AspNetServiceLocator, Glimpse.AspNet", false, true);
            if (innerServiceLocatorType != null)
            {
                innerServiceLocator = Activator.CreateInstance(innerServiceLocatorType) as IServiceLocator;
            }
            
        }
        public T GetInstance<T>() where T : class
        {
            if (typeof(T) == typeof(IPersistenceStore))
            {
                return new WindowsAzureStoragePersistenceStore() as T;
            }

            if (innerServiceLocator != null)
            {
                return innerServiceLocator.GetInstance<T>();
            }
            return null;
        }

        public ICollection<T> GetAllInstances<T>() where T : class
        {

            if (innerServiceLocator != null)
            {
                return innerServiceLocator.GetAllInstances<T>();
            }
            return null;
        }
    }
}