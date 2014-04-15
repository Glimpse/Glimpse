#if EF6Plus
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Linq;
using Glimpse.Ado.Inspector.Core;

namespace Glimpse.EF.AlternateType
{
    public class InvariantNameResolver : IDbDependencyResolver
    {
        private readonly object registeredFactoriesLock = new object();
        private Dictionary<string, string> registeredFactories;

        private Dictionary<string, string> RegisteredFactories
        {
            get
            {
                if (registeredFactories == null)
                {
                    lock (registeredFactoriesLock)
                    {
                        if (registeredFactories == null)
                        {
                            registeredFactories = DbProviderFactoriesExecutionTask.Factories
                                .ToDictionary(
                                    x =>
                                    {
                                        var type = x.Value;
                                        if (!string.IsNullOrEmpty(type))
                                        {
                                            var typeIndex = type.IndexOf(',');
                                            type = type.Substring(0, typeIndex < 0 ? type.Length : typeIndex);
                                        }

                                        return type;
                                    },
                                    x => x.Key);
                        }
                    }
                }

                return registeredFactories;
            }
        } 

        public virtual object GetService(Type type, object key)
        {
            if (type == typeof(IProviderInvariantName))
            { 
                var factoryType = key.GetType().FullName;

                var factoryName = (string)null;
                if (RegisteredFactories.TryGetValue(factoryType, out factoryName))
                {
                    return new ProviderInvariantName(factoryName);
                }  
            }

            return null;
        }

        public IEnumerable<object> GetServices(Type type, object key)
        {
            var service = GetService(type, key);

            return service == null ? Enumerable.Empty<object>() : new[] {service};
        }

        private class ProviderInvariantName : IProviderInvariantName
        {
            public ProviderInvariantName(string name)
            {
                Name = name;
            }

            public string Name { get; private set; }
        }
    }
}
#endif