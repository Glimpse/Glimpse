using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> abstraction which makes returning static resources embedded into a Dll easier.
    /// </summary>
    public abstract class FileResource : IResource
    {
        /// <summary>
        /// Gets or sets the name of the embedded Dll resource.
        /// </summary>
        /// <value>
        /// The name of the embedded Dll resource.
        /// </value>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the content type of the embedded Dll resource.
        /// </summary>
        /// <value>
        /// The content type of the embedded Dll resource.
        /// </value>
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <remarks>
        /// Resource name's should be unique across a given web application. If multiple <see cref="IResource" /> implementations contain the same name, Glimpse may throw an exception resulting in an Http 500 Server Error.
        /// </remarks>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets the required or optional parameters that a resource needs as processing input.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public virtual IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { ResourceParameter.Hash }; }
        }

        /// <summary>
        /// Gets the duration of the cache, in seconds.
        /// </summary>
        /// <value>
        /// The duration of the cache, in seconds.
        /// </value>
        /// <remarks>
        /// CacheDuration will be set as the <c>max-age</c> parameter of the <c>Cache-Control</c> Http response header.
        /// </remarks>
        protected virtual int CacheDuration
        {
            get { return 12960000; /*150 days*/ }
        }

        /// <summary>
        /// Executes the specified resource with the specific context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <see cref="IResourceResult" /> that can be executed when the Http response is ready to be returned.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Throws an exception if <paramref name="context "/> is <c>null</c>.</exception>
        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var assembly = GetResourcesAssembly();
            if (assembly == null) {
                return new StatusCodeResourceResult(404, string.Format("Could not locate assembly for resource with ResourceName '{0}'.", ResourceName));
            }
 
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
        
        /// <summary>
        /// Gets the assembly in which the resource is embedded
        /// </summary>
        /// <returns>Assembly to get resource stream from</returns>
        protected virtual Assembly GetResourcesAssembly() {
            return GetType().Assembly;
        }
    }
}