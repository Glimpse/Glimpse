using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> abstraction which makes returning static resources embedded into an assembly easier.
    /// </summary>
    public abstract class FileResource : IResource
    {
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

            var embeddedResourceInfo = this.GetEmbeddedResourceInfo(context);
            if (embeddedResourceInfo == null)
            {
                return new StatusCodeResourceResult(404, string.Format("Could not get embedded resource information."));
            }

            using (var resourceStream = embeddedResourceInfo.Assembly.GetManifestResourceStream(embeddedResourceInfo.Name))
            {
                if (resourceStream != null)
                {
                    var content = new byte[resourceStream.Length];
                    resourceStream.Read(content, 0, content.Length);

                    return new CacheControlDecorator(CacheDuration, CacheSetting.Public, new FileResourceResult(content, embeddedResourceInfo.ContentType));
                }
            }

            return new StatusCodeResourceResult(404, string.Format("Could not locate embedded resource with name '{0}' inside assembly '{1}'.", embeddedResourceInfo.Name, embeddedResourceInfo.Assembly.FullName));
        }

        /// <summary>
        /// Returns, based on the resource context, the embedded resource that will be returned during the execution of the <see cref="FileResource"/>
        /// </summary>
        /// <param name="context">The resource context</param>
        /// <returns>Information about the embedded resource</returns>
        protected abstract EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context);

        /// <summary>
        /// Contains the details of an embedded resource
        /// </summary>
        public class EmbeddedResourceInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EmbeddedResourceInfo"/> class
            /// </summary>
            /// <param name="resourceAssembly">The assembly containing the embedded resource</param>
            /// <param name="resourceName">The name of the embedded resource</param>
            /// <param name="contentType">The content type of the embedded resource</param>
            public EmbeddedResourceInfo(Assembly resourceAssembly, string resourceName, string contentType)
            {
                if (resourceAssembly == null)
                {
                    throw new ArgumentNullException("resourceAssembly");
                }

                if (resourceName == null)
                {
                    throw new ArgumentNullException("resourceName");
                }

                if (contentType == null)
                {
                    throw new ArgumentNullException("contentType");
                }

                Assembly = resourceAssembly;
                Name = resourceName;
                ContentType = contentType;
            }

            /// <summary>
            /// Gets the assembly containing the embedded resource.
            /// </summary>
            /// <value>
            /// The assembly containing the embedded resource.
            /// </value>
            public Assembly Assembly { get; private set; }

            /// <summary>
            /// Gets the name of the embedded resource.
            /// </summary>
            /// <value>
            /// The name of the embedded resource.
            /// </value>
            public string Name { get; private set; }

            /// <summary>
            /// Gets the content type of the embedded resource.
            /// </summary>
            /// <value>
            /// The content type of the embedded resource.
            /// </value>
            public string ContentType { get; private set; }
        }
    }
}