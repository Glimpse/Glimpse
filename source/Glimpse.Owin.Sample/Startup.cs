using Glimpse.Owin.Katana;
using Owin;

namespace Glimpse.Owin.Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app = app.WithGlimpse();

            app.UseWelcomePage();

            app.UseErrorPage();
        }
    }
}