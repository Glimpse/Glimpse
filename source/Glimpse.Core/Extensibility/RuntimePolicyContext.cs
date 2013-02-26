using System;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// The implementation of <see cref="IRuntimePolicyContext"/> used in the <c>Execute</c> method of <see cref="IRuntimePolicy"/>.
    /// </summary>
    public class RuntimePolicyContext : IRuntimePolicyContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimePolicyContext" /> class.
        /// </summary>
        /// <param name="requestMetadata">The request metadata.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="requestContext">The request context.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if any parameters are <c>null</c>.</exception>
        public RuntimePolicyContext(IRequestMetadata requestMetadata, ILogger logger, object requestContext)
        {
            if (requestMetadata == null)
            {
                throw new ArgumentNullException("requestMetadata");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }
            
            RequestMetadata = requestMetadata;
            Logger = logger;
            RequestContext = requestContext;
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the request metadata.
        /// </summary>
        /// <value>
        /// The request metadata.
        /// </value>
        public IRequestMetadata RequestMetadata { get; set; }
        
        private object RequestContext { get; set; }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <typeparam name="T">Type of result that is expected.</typeparam>
        /// <returns>
        /// The request context that is being used.
        /// </returns>
        public T GetRequestContext<T>() where T : class
        {
            return RequestContext as T;
        }
    }
}