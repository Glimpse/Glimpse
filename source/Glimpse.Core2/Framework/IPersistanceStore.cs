namespace Glimpse.Core2.Framework
{
    public interface IPersistanceStore:IReadOnlyPersistanceStore
    {
        void Save(GlimpseMetadata data);
    }
}