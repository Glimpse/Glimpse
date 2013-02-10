using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An implementation of Castle DynamicProxy's <see cref="IInterceptorSelector"/> for Glimpse.
    /// </summary>
    public class AlternateTypeSelector : IInterceptorSelector
    {
        /// <summary>
        /// Selects the interceptors that should intercept calls to the given <paramref name="method" />.
        /// </summary>
        /// <param name="type">The type declaring the method to intercept.</param>
        /// <param name="method">The method that will be intercepted.</param>
        /// <param name="interceptors">All interceptors registered with the proxy.</param>
        /// <returns>
        /// An array of interceptors to invoke upon calling the <paramref name="method" />.
        /// </returns>
        /// <remarks>
        /// This method is called only once per proxy instance, upon the first call to the
        /// <paramref name="method" />. Either an empty array or null are valid return values to indicate
        /// that no interceptor should intercept calls to the method. Although it is not advised, it is
        /// legal to return other <see cref="T:Castle.DynamicProxy.IInterceptor" /> implementations than these provided in
        /// <paramref name="interceptors" />.
        /// </remarks>
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            return interceptors.Where(i =>
                                          {
                                              var ai = i as AlternateTypeToCastleInterceptorAdapter;
                                              return ai != null && ai.MethodToImplement == method;
                                          }).ToArray();
        }
    }
}