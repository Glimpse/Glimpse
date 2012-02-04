using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class Factory
    {
        internal IServiceLocator UserServiceLocator { get; set; }
        internal IServiceLocator ProviderServiceLocator { get; set; }

        public Factory():this(null){}

        public Factory(IServiceLocator providerServiceLocator):this(providerServiceLocator, null){}

        public Factory(IServiceLocator providerServiceLocator, IServiceLocator userServiceLocator)
        {
            //TODO: load in config values
            //TODO: Try to lookup/load user service locator from config if null
            ProviderServiceLocator = providerServiceLocator;
            UserServiceLocator = userServiceLocator;
        }

        public IGlimpseRuntime InstantiateRuntime()
        {
            IGlimpseRuntime result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            //TODO: Finish me!
            throw new NotImplementedException();
        }

        public IFrameworkProvider InstantiateFrameworkProvider()
        {
            IFrameworkProvider result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            //TODO: Turn this string into a resource and provide better information
            throw new GlimpseException("Unable to create Framework Provider.");
        }


        private bool TrySingleInstanceFromServiceLocators<T>(out T instance) where T: class
        {
            if (UserServiceLocator != null)
            {
                instance = UserServiceLocator.GetInstance<T>();
                if (instance != null) return true;
            }

            if (ProviderServiceLocator != null)
            {
                instance = ProviderServiceLocator.GetInstance<T>();
                if (instance != null) return true;
            }

            instance = null;
            return false;
        }

        private bool TryAllInstancesFromServiceLocators<T>(out IList<T> instance) where T : class
        {
            IEnumerable<T> result;
            if (UserServiceLocator != null)
            {
                result = UserServiceLocator.GetAllInstances<T>();
                if (result != null && result.Any())
                {
                    instance = result as IList<T>;
                    return true;
                }
            }

            if (ProviderServiceLocator != null)
            {
                result = ProviderServiceLocator.GetAllInstances<T>();
                if (result != null && result.Any())
                {
                    instance = result as IList<T>;
                    return true;
                }
            }

            instance = null;
            return false;
        }

        public ResourceEndpointConfiguration InstantiateEndpointConfiguration()
        {
            ResourceEndpointConfiguration result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            //TODO: Turn this string into a resource and provide better information
            throw new GlimpseException("Unable to create Endpoint Configuration.");
        }

        public IList<IClientScript> InstantiateClientScripts()
        {
            Contract.Ensures(Contract.Result<IList<IClientScript>>()!=null);

            IList<IClientScript> result;
            if (TryAllInstancesFromServiceLocators(out result)) return result;

            //TODO: Load via reflection

            return new List<IClientScript>();
        }
    }
}