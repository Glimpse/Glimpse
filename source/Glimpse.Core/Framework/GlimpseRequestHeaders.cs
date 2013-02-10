using System;
using Glimpse.Core.Resource;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Used to return a subset of <see cref="GlimpseRequest"/> properties by <see cref="HistoryResource"/>.
    /// </summary>
    public class GlimpseRequestHeaders
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRequestHeaders" /> class.
        /// </summary>
        /// <param name="glimpseRequest">The glimpse request.</param>
        public GlimpseRequestHeaders(GlimpseRequest glimpseRequest)
        {
            GlimpseRequest = glimpseRequest;
        }

        /// <summary>
        /// Gets the client id.
        /// </summary>
        /// <value>
        /// The client id.
        /// </value>
        public string ClientId
        {
            get { return GlimpseRequest.ClientId; }
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public DateTime DateTime
        {
            get { return GlimpseRequest.DateTime; }
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration
        {
            get { return GlimpseRequest.Duration; }
        }

        /// <summary>
        /// Gets the parent request id.
        /// </summary>
        /// <value>
        /// The parent request id.
        /// </value>
        public Guid? ParentRequestId
        {
            get { return GlimpseRequest.ParentRequestId; }
        }

        /// <summary>
        /// Gets the request id.
        /// </summary>
        /// <value>
        /// The request id.
        /// </value>
        public Guid RequestId
        {
            get { return GlimpseRequest.RequestId; }
        }

        /// <summary>
        /// Gets a value indicating whether [request is ajax].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [request is ajax]; otherwise, <c>false</c>.
        /// </value>
        public bool RequestIsAjax
        {
            get { return GlimpseRequest.RequestIsAjax; }
        }

        /// <summary>
        /// Gets the request HTTP method.
        /// </summary>
        /// <value>
        /// The request HTTP method.
        /// </value>
        public string RequestHttpMethod
        {
            get { return GlimpseRequest.RequestHttpMethod; }
        }

        /// <summary>
        /// Gets the request URI.
        /// </summary>
        /// <value>
        /// The request URI.
        /// </value>
        public string RequestUri
        {
            get { return GlimpseRequest.RequestUri; }
        }

        /// <summary>
        /// Gets the type of the response content.
        /// </summary>
        /// <value>
        /// The type of the response content.
        /// </value>
        public string ResponseContentType
        {
            get { return GlimpseRequest.ResponseContentType; }
        }

        /// <summary>
        /// Gets the response status code.
        /// </summary>
        /// <value>
        /// The response status code.
        /// </value>
        public int ResponseStatusCode
        {
            get { return GlimpseRequest.ResponseStatusCode; }
        }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        public string UserAgent
        {
            get { return GlimpseRequest.UserAgent; }
        }

        private GlimpseRequest GlimpseRequest { get; set; }
    }
}