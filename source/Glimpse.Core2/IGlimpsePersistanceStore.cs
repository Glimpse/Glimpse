using System;

namespace Glimpse.Core2
{
    public interface IGlimpsePersistanceStore
    {
        int Count();
        void Save(GlimpseMetadata data);
        GlimpseMetadata GetById(Guid requestId);
    }
}