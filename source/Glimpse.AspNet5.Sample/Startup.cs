using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;

namespace Glimpse.AspNet5.Sample
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app = app.WithGlimpse();

            app.UseErrorPage(ErrorPageOptions.ShowAll);
            app.UseStaticFiles();
            app.UseWelcomePage();
        }
    }
}
