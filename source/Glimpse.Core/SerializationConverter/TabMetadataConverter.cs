using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.SerializationConverter
{
    /// <summary>
    /// The <see cref="ISerializationConverter"/> implementation responsible converting <see cref="TabMetadata"/> representation's into a format suitable for serialization.
    /// </summary>
    public class TabMetadataConverter : SerializationConverter<TabMetadata>
    {
        /// <summary>
        /// Converts the specified metadata.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An object suitable for serialization.</returns>
        public override object Convert(TabMetadata metadata)
        {
            return new Dictionary<string, object>
                       {
                           { "documentationUri", metadata.DocumentationUri },
                           { "layout", metadata.Layout }
                       };
        }
    }
}