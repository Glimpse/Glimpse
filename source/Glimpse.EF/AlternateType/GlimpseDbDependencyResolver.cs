#if EF6Plus
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure; 
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Reflection;

namespace Glimpse.EF.AlternateType
{
    public class GlimpseDbDependencyResolver : IDbDependencyResolver
    {
        private readonly IDbDependencyResolver rootResolver;
         
        public GlimpseDbDependencyResolver(DbConfiguration originalDbConfiguration)
        {
            // Get the original resolver
            var internalConfigProp = originalDbConfiguration.GetType().GetProperty("InternalConfiguration", BindingFlags.Instance | BindingFlags.NonPublic);
            var internalConfig = internalConfigProp.GetValue(originalDbConfiguration, null);
            var rootResolverProp = internalConfig.GetType().GetProperty("RootResolver", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            rootResolver = (IDbDependencyResolver)rootResolverProp.GetValue(internalConfig, null);
        }

        public object GetService(Type type, object key)
        {
            if (type == typeof(IDbProviderFactoryResolver))
            {
                var innerFactoryService = (IDbProviderFactoryResolver)rootResolver.GetService(type, key);
                if (!(innerFactoryService is GlimpseDbProviderFactoryResolver))
                {
                    return new GlimpseDbProviderFactoryResolver(innerFactoryService);
                }

                return innerFactoryService;
            }

            if (type == typeof(DbProviderServices))
            {
                // Would love to be able to wrap this but the ProviderServices is sealed and an "internal virual" 
                // method needs to be overriden to return the innter type rather than the inherited type.
                if ((string)key == "System.Data.Entity.Core.EntityClient.EntityProviderFactory")
                {
                    return ((IServiceProvider)System.Data.Entity.Core.EntityClient.EntityProviderFactory.Instance).GetService(type);
                }

                var innerProviderServices = (DbProviderServices)rootResolver.GetService(type, key);  
                if (!(innerProviderServices is GlimpseDbProviderServices))
                {
                    return new GlimpseDbProviderServices(innerProviderServices);
                }

                return innerProviderServices; 
            }

            if (type == typeof(IProviderInvariantName))
            {
                
            }

            return rootResolver.GetService(type, key);  
        }


        public IEnumerable<object> GetServices(Type type, object key)
        {
            return rootResolver.GetServices(type, key);  
        }
    }
}
#endif