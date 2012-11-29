using System.Reflection;

namespace Glimpse.Core.Extensibility
{
    public interface IAlternateImplementation
    {
        MethodInfo MethodToImplement { get; }
        
        void NewImplementation(IAlternateImplementationContext context);
    }
}