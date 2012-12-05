namespace Glimpse.Core.Extensibility
{
    public interface IAlternateType<T>
    {
        bool TryCreate(T originalObj, out T newObj);
    }
}