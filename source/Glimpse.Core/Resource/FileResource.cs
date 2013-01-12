using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    public abstract class FileResource : IResource
    {
        public string ResourceName { get; set; }
        
        public string ResourceType { get; set; }

        public string Name { get; protected set; }

        public virtual IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { ResourceParameter.VersionNumber }; }
        }

        protected virtual int CacheDuration
        {
            get { return 12960000; /*150 days*/ }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var assembly = Assembly.GetExecutingAssembly();
             
            using (var resourceStream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (resourceStream != null)
                {
                    var content = new byte[resourceStream.Length];
                    resourceStream.Read(content, 0, content.Length);

                    return new CacheControlDecorator(CacheDuration, CacheSetting.Public, new FileResourceResult(content, ResourceType));
                }
            }

            return new StatusCodeResourceResult(404, string.Format("Could not locate file with ResourceName '{0}'.", ResourceName));
        } 
    }
}