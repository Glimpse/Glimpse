using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.ResourceResult
{
    /// <summary>
    /// The <see cref="IResourceResult"/> implementation responsible returning binary files to a client.
    /// </summary>
    public class FileResourceResult : IResourceResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileResourceResult" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <exception cref="System.ArgumentNullException">Exception thrown if either <paramref name="content"/> or <paramref name="contentType"/> are <c>null</c>.</exception>
        public FileResourceResult(byte[] content, string contentType)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException("contentType");
            }

            Content = content;
            ContentType = contentType;
        }

        /// <summary>
        /// Gets or sets the content to send to the client.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public byte[] Content { get; set; }

        /// <summary>
        /// Gets or sets the content type of the Content.
        /// </summary>
        /// <value>
        /// The content type.
        /// </value>
        public string ContentType { get; set; }

        /// <summary>
        /// Executes the resource result with the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.RequestResponseAdapter;

            frameworkProvider.SetHttpResponseHeader("Content-Type", ContentType);

            frameworkProvider.WriteHttpResponse(Content);
        }
    }
}