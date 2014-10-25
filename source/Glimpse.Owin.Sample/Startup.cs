using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Owin;

namespace Glimpse.Owin.Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/flush", innerMap =>
            {
                innerMap = innerMap.WithGlimpse();
                innerMap.Run(async handler =>
                {
                    handler.Response.ContentType = "text/html";

                    await WriteAndFlushAsync(handler.Response.Body, "<html><body>first part flushed on " + DateTime.Now.ToString("HH:mm:ss") + "<br/>");

                    Thread.Sleep(1500);
                    await WriteAndFlushAsync(handler.Response.Body, "flushed again on " + DateTime.Now.ToString("HH:mm:ss") + "<br/>");

                    Thread.Sleep(1500);
                    await WriteAndFlushAsync(handler.Response.Body, "final flush on " + DateTime.Now.ToString("HH:mm:ss") + "</body></html>");
                });
            });

            app.Map("/issue856", innerMap =>
            {
                innerMap = innerMap.WithGlimpse();
                innerMap.Run(async context =>
                {
                    context.Response.ContentType = "text/html";

                    await context.Response.WriteAsync("<html><body>Hello world</body></html>");
                });
            });

            app = app.WithGlimpse();
            app.UseWelcomePage();

            app.UseErrorPage();
        }

        private static async Task WriteAndFlushAsync(Stream stream, string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            stream.Write(buffer, 0, buffer.Length);
            await stream.FlushAsync();
        }
    }
}