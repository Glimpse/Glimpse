using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Extensions
{
    public static class AlternateImplementationContextExtensions
    {
        public static bool TryProceedWithTimer(this IAlternateImplementationContext context, out TimerResult timerResult)
        {
            if (context.RuntimePolicyStrategy() == RuntimePolicy.Off)
            {
                context.Proceed();
                timerResult = null;
                return false;
            }

            var timer = context.TimerStrategy();

            timerResult = timer.Time(context.Proceed);
            return true;
        }
    }
}