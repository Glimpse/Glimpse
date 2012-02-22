using System.Reflection;

namespace Glimpse.Core2.Extensibility
{
    public interface IAlternateImplementation<T> where T : class
    {
        MethodInfo MethodToImplement { get; }
        void NewImplementation(IAlternateImplementationContext context);
    }
}