using Glimpse.Core.Extensibility;

namespace Glimpse.Core.ResourceResult
{
    /// <summary>
    /// The <see cref="IResourceResult"/> implementation responsible returning simple status code/message pairs to a client.
    /// </summary>
    /// <remarks>
    /// <see cref="StatusCodeResourceResult"/> is typically used to convey Http error conditions to client, as is typical of Rest style architectures.
    /// </remarks>
    public class StatusCodeResourceResult : IResourceResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodeResourceResult" /> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        public StatusCodeResourceResult(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        /// <summary>
        /// Gets or sets the message to return to the client.
        /// </summary>
        /// <value>
        /// The message the message to return to the client.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the status code to return to the client.
        /// </summary>
        /// <value>
        /// The status code to return to the client.
        /// </value>
        public int StatusCode { get; set; }

        /// <summary>
        /// Executes the resource result with the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.FrameworkProvider;

            frameworkProvider.WriteHttpResponse(Message);
            frameworkProvider.SetHttpResponseStatusCode(StatusCode);
        }
    }
}