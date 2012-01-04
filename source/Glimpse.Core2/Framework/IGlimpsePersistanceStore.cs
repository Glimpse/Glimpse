using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public interface IGlimpsePersistanceStore
    {
        int Count();
        void Save(GlimpseMetadata data);
        GlimpseMetadata GetById(Guid requestId);
        GlimpseMetadata[] GetByClient(string clientName);
        IDictionary<string, int> GetClients();
    }
}