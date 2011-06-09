using System.Collections.Generic;
using Glimpse.Core.Plumbing;

namespace Glimpse.Core.Extensibility
{
    public interface IGlimpseMetadataStore
    {
        void Persist(GlimpseRequestMetadata metadata);
        IEnumerable<GlimpseRequestMetadata> Requests { get; }
    }
}
