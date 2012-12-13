using System.Reflection;

namespace Glimpse.Core.Extensibility
{
    public interface IAlternateMethod
    {
        MethodInfo MethodToImplement { get; }
        
        void NewImplementation(IAlternateImplementationContext context);
    }
}