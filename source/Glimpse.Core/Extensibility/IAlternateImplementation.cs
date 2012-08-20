using System.Reflection;

namespace Glimpse.Core.Extensibility
{
    public interface IAlternateImplementation<T> where T : class
    {
        MethodInfo MethodToImplement { get; }
        void NewImplementation(IAlternateImplementationContext context);
    }
}