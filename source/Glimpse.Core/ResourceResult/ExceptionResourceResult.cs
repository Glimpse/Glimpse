using System;

namespace Glimpse.Core.ResourceResult
{
    /// <summary>
    /// The <see cref="StatusCodeResourceResult"/> implementation responsible returning .Net exceptions to a client in a way compatible with Http.
    /// </summary>
    public class ExceptionResourceResult : StatusCodeResourceResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionResourceResult" /> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ExceptionResourceResult(Exception exception) : base(500, exception.ToString())
        {
        }
    }
}