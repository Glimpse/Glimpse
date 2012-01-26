using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public interface IReadOnlyPersistanceStore
    {
        GlimpseMetadata GetByRequestId(Guid requestId);
        string GetByRequestIdAndTabKey(Guid requestId, string tabKey);
        IEnumerable<GlimpseMetadata> GetByRequestParentId(Guid parentRequestId);
        IEnumerable<GlimpseMetadata> GetTop(int count);
    }
}