using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Extensions
{
    public static class AlternateImplementationContextExtensions
    {
        public static TimerResult ProceedWithTimerIfAllowed(this IAlternateImplementationContext context)
        {
            if (context.RuntimePolicyStrategy() == RuntimePolicy.Off)
            {
                context.Proceed();
                return null;
            }

            var timer = context.TimerStrategy();

            return timer.Time(context.Proceed);
        }
    }
}