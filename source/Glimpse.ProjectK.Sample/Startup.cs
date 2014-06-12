using Glimpse.ProjectK;
using Microsoft.AspNet.Abstractions;
using Microsoft.AspNet;
using Microsoft.AspNet.Diagnostics;

namespace KWebStartup
{
    public class Startup
    {
        public void Configuration(IBuilder app)
        {
            app = app.WithGlimpse();

            app.UseErrorPage(ErrorPageOptions.ShowAll);
            app.UseStaticFiles();
            app.UseWelcomePage();
        }
    }
}