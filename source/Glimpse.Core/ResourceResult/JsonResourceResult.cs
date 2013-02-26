using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.ResourceResult
{
    /// <summary>
    /// The <see cref="IResourceResult"/> implementation responsible returning Json data to clients.
    /// </summary>
    public class JsonResourceResult : IResourceResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResourceResult" /> class without a JsonP callback function.
        /// </summary>
        /// <param name="data">The data.</param>
        public JsonResourceResult(object data) : this(data, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResourceResult" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="callback">The callback.</param>
        /// <exception cref="System.ArgumentNullException">An exception is thrown is <paramref name="data"/> is <c>null</c>.</exception>
        public JsonResourceResult(object data, string callback)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Data = data;
            Callback = callback;
            ContentType = string.IsNullOrEmpty(callback) ? @"application/json" : @"application/x-javascript";
        }

        /// <summary>
        /// Gets or sets the JsonP callback function.
        /// </summary>
        /// <value>
        /// The JsonP callback function. Option.
        /// </value>
        public string Callback { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        /// <value>
        /// The content type.
        /// </value>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the data to serialize into Json.
        /// </summary>
        /// <value>
        /// The data to serialize.
        /// </value>
        public object Data { get; set; }

        /// <summary>
        /// Executes the resource result with the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.FrameworkProvider;
            var serializer = context.Serializer;

            var result = serializer.Serialize(Data);

            frameworkProvider.SetHttpResponseHeader("Content-Type", ContentType);

            var toWrite = string.IsNullOrEmpty(Callback) ? result : string.Format("{0}({1});", Callback, result);

            frameworkProvider.WriteHttpResponse(toWrite);
        }
    }
}