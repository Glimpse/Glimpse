using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Used to describe how a given request is handled by Glimpse
    /// </summary>
    public enum RequestHandlingMode
    {
        /// <summary>
        /// Glimpse is not handling this request. This can be because Glimpse was disabled to start with or a <see cref="IRuntimePolicy"/> decided 
        /// during <see cref="IGlimpseRuntime.BeginRequest"/> that Glimpse should not handle this request.
        /// </summary>
        Unhandled,

        /// <summary>
        /// Glimpse hooked itself onto the request and started collecting information. This does not mean that information will be stored in the end,
        /// as it is still possible for a <see cref="IRuntimePolicy"/> to decide otherwise by the end of the request.
        /// </summary>
        RegularRequest,

        /// <summary>
        /// Glimpse handles this request completely, as the request is made for a specific Glimpse <see cref="IResource"/>
        /// </summary>
        ResourceRequest
    }
}