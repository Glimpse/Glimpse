using System.Reflection;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for an alternate method which allows classes to be proxied.
    /// </summary>
    public interface IAlternateMethod
    {
        /// <summary>
        /// Gets the method to implement.
        /// </summary>
        /// <value>The method to implement.</value>
        /// <remarks>
        /// The info of the method that this alternate is for. 
        /// </remarks>
        MethodInfo MethodToImplement { get; }

        /// <summary>
        /// New implementation that is called in-place of the the original method.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>
        /// It is up to this method to call the underlying target method.
        /// </remarks>
        void NewImplementation(IAlternateMethodContext context);
    }
}