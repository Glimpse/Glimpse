using System;

namespace Glimpse.Core.Framework
{
    public interface IScriptTagsGenerator
    {
        /// <summary>
        /// Generates Glimpse script tags for the given Glimpse request id
        /// </summary>
        /// <param name="glimpseRequestId">The Glimpse request Id of the request for which script tags must be generated</param>
        /// <returns>The generated script tags</returns>
        string Generate(Guid glimpseRequestId);
    }
}