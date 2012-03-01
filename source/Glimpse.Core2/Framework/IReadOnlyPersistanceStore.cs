using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public interface IReadOnlyPersistanceStore
    {
        GlimpseRequest GetByRequestId(Guid requestId);
        TabResult GetByRequestIdAndTabKey(Guid requestId, string tabKey);
        IEnumerable<GlimpseRequest> GetByRequestParentId(Guid parentRequestId);
        IEnumerable<GlimpseRequest> GetTop(int count);
        GlimpseMetadata GetMetadata();
    }
}