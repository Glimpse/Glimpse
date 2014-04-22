using System;
using System.Collections.Generic;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// A class which describes a given Http request, along with the corresponding tab data gathered for that request.
    /// </summary>
    public class GlimpseRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRequest" /> class.
        /// </summary>
        public GlimpseRequest()
        {
            DateTime = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRequest" /> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="requestMetadata">The request metadata.</param>
        /// <param name="tabData">The plugin data.</param>
        /// <param name="displayData">The display data</param>
        /// <param name="duration">The duration.</param>
        public GlimpseRequest(Guid requestId, IRequestMetadata requestMetadata, IDictionary<string, TabResult> tabData, IDictionary<string, TabResult> displayData, TimeSpan duration)
            : this()
        {
            RequestId = requestId;
            TabData = tabData;
            DisplayData = displayData;
            Duration = duration;

            RequestHttpMethod = requestMetadata.RequestHttpMethod;
            RequestIsAjax = requestMetadata.RequestIsAjax;
            RequestUri = requestMetadata.RequestUri;
            ResponseStatusCode = requestMetadata.ResponseStatusCode;
            ResponseContentType = requestMetadata.ResponseContentType;
            ClientId = requestMetadata.GetCookie(Constants.ClientIdCookieName) ?? requestMetadata.ClientId;
            UserAgent = requestMetadata.GetHttpHeader(Constants.UserAgentHeaderName);

            Guid parentRequestId;

#if NET35
            if (RequestIsAjax && Glimpse.Core.Backport.Net35Backport.TryParseGuid(requestMetadata.GetHttpHeader(Constants.HttpRequestHeader), out parentRequestId))
            {
                ParentRequestId = parentRequestId;
            }
#else
            if (RequestIsAjax && Guid.TryParse(requestMetadata.GetHttpHeader(Constants.HttpRequestHeader), out parentRequestId))
            {
                ParentRequestId = parentRequestId;
            }
#endif
        }

        /// <summary>
        /// Gets or sets the client id used for session tracking.
        /// </summary>
        /// <value>
        /// The client id.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the date time of the request.
        /// </summary>
        /// <value>
        /// The date time of the request.
        /// </value>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the request.
        /// </summary>
        /// <value>
        /// The duration of the request.
        /// </value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the parent request id.
        /// </summary>
        /// <value>
        /// The parent request id.
        /// </value>
        public Guid? ParentRequestId { get; set; }

        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        /// <value>
        /// The request id.
        /// </value>
        public Guid RequestId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the request is an Ajax request.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the request is an Ajax request; otherwise, <c>false</c>.
        /// </value>
        public bool RequestIsAjax { get; set; }

        /// <summary>
        /// Gets or sets the request's HTTP method.
        /// </summary>
        /// <value>
        /// The request's HTTP method.
        /// </value>
        public string RequestHttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the request's Uri.
        /// </summary>
        /// <value>
        /// The request's Uri.
        /// </value>
        public string RequestUri { get; set; }

        /// <summary>
        /// Gets or sets the type of the response's content type.
        /// </summary>
        /// <value>
        /// The content type of the response.
        /// </value>
        public string ResponseContentType { get; set; }

        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        /// <value>
        /// The response status code.
        /// </value>
        public int ResponseStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the tab data for the Http request.
        /// </summary>
        /// <value>
        /// The tab data.
        /// </value>
        public IDictionary<string, TabResult> TabData { get; set; }

        public IDictionary<string, TabResult> DisplayData { get; set; }

        /// <summary>
        /// Gets or sets the user agent for the request.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        public string UserAgent { get; set; }
    }
}