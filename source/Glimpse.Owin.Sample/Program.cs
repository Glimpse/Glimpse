using System;
using Microsoft.Owin.Hosting;

namespace Glimpse.Owin.Sample
{
    public class Program
    {
        // Think about switching to the OwinHost NuGet package and deleting this file when we no longer care about supporting VS 2012
        // http://blogs.msdn.com/b/webdev/archive/2013/09/11/visual-studio-2013-custom-web-servers-and-owinhost-exe.aspx

        public static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8080/"))
            {
                Console.WriteLine("Started at http://localhost:8080/");
                Console.ReadLine();
                Console.WriteLine("Stopping");
            }
        }
    }
}