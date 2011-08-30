using System.Web;

namespace Glimpse.Core.Extensibility
{
    public static class GlimpseTimer
    {
        private static HttpContextBase context;
        internal static HttpContextBase Context
        {
            get { return context ?? new HttpContextWrapper(HttpContext.Current); }
            set { context = value; }
        }

        private static TimerMetadata TimerMetadata
        {
            get
            {
                var metadata = Context.Items["TimerMetadata"] as TimerMetadata;

                if (metadata == null)
                {
                    metadata = new TimerMetadata();
                    Context.Items["TimerMetadata"] = metadata;
                }

                return metadata;
            }
        }

        public static void Moment(string message, string category = "ASP.NET", string description = null)
        {
            TimerEvent timerEvent = TimerMetadata.AddEvent(message, category, description);
            timerEvent.Stop();
            timerEvent.Duration = 0;
        }

        public static TimerEvent Start(string message, string category = "ASP.NET", string description = null)
        {
            return TimerMetadata.AddEvent(message, category, description);
        }

        public static void Stop(string message)
        {
            var metadata = TimerMetadata;

            var timerEvent = metadata.GetEvent(message);

            timerEvent.Stop();
        }
    }
}