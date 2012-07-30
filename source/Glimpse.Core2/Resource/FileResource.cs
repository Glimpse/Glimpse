using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.ResourceResult;

namespace Glimpse.Core2.Resource
{
    public abstract class FileResource:IResource
    {
        protected virtual int CacheDuration { get { return 12960000; /*150 days*/ } }

        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
        public string Name { get; protected set; }

        public virtual IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { ResourceParameter.VersionNumber }; }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var assembly = Assembly.GetExecutingAssembly();
             
            using (var resourceStream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (resourceStream != null)
                {
                    var content = new byte[resourceStream.Length];
                    resourceStream.Read(content, 0, content.Length);
                    return new FileResourceResult(content, ResourceType, CacheDuration, CacheSetting.Public);
                }
            }

            return new StatusCodeResourceResult(404);
        } 
    }
}