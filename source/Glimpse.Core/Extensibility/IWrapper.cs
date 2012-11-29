namespace Glimpse.Core.Extensibility
{
    public interface IWrapper<T>
    {
        T GetWrappedObject();
    }
}