using System.Reflection;

namespace Glimpse.Core2.Extensibility
{
    public interface IAlternateMethodImplementation<T> where T : class
    {
        MethodInfo MethodToImplement { get; }
        void NewImplementation(IAlternateMethodImplementationContext context);
    }
}