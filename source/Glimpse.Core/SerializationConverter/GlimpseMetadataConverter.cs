using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.SerializationConverter
{
    /// <summary>
    /// The <see cref="ISerializationConverter"/> implementation responsible converting <see cref="GlimpseMetadata"/> representation's into a format suitable for serialization.
    /// </summary>
    public class GlimpseMetadataConverter : SerializationConverter<GlimpseMetadata>
    {
        /// <summary>
        /// Converts the specified metadata.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An object suitable for serialization.</returns>
        public override object Convert(GlimpseMetadata metadata)
        {
            return new Dictionary<string, object>
                       {
                           { "version", metadata.Version },
                           { "plugins", metadata.Tabs },
                           { "resources", metadata.Resources },
                       };
        }
    }
}