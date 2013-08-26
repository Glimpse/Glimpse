using Glimpse.Owin.Extensions;
using Glimpse.Owin.Middleware;
using Owin;

namespace Glimpse.Owin.Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // app = app.WithGlimpse();

            app.UseType<TailMiddleware>(); // This doesn't seems like it should be here but rather later in the pipeline?

            app.Use(typeof(TimestampMiddleware));

            app.UseWelcomePage();
        }
    }
}