namespace Glimpse.Core.Framework
{
    public interface IPersistenceStore:IReadOnlyPersistenceStore
    {
        void Save(GlimpseRequest request);
        void Save(GlimpseMetadata metadata);
    }
}