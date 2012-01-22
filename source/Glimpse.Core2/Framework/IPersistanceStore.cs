using System;

namespace Glimpse.Core2.Framework
{
    public interface IPersistanceStore
    {
        void Save(GlimpseMetadata data);
        GlimpseMetadata GetByRequestId(Guid requestId);
        string GetByRequestIdAndTabKey(Guid requestId, string tabKey);
        GlimpseMetadata[] GetByRequestParentId(Guid parentRequestId);
        GlimpseMetadata[] GetTop(int count);
    }
}