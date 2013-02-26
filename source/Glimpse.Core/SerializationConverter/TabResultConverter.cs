using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.SerializationConverter
{
    /// <summary>
    /// The <see cref="ISerializationConverter"/> implementation responsible converting <see cref="TabResult"/> representation's into a format suitable for serialization.
    /// </summary>
    public class TabResultConverter : SerializationConverter<TabResult>
    {
        /// <summary>
        /// Converts the specified result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>An object suitable for serialization.</returns>
        public override object Convert(TabResult result)
        {
            return new Dictionary<string, object>
                       {
                           { "data", result.Data },
                           { "name", result.Name },
                       };
        }
    }
}