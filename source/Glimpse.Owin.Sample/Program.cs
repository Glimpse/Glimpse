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
            int port;
            do
            {
                Console.Write("Please enter a port for the web application : ");
                var enteredPort = Console.ReadLine();
                if (!int.TryParse(enteredPort, out port))
                {
                    Console.WriteLine("Invalid port");
                }
            } while (port == 0);

            using (WebApp.Start<Startup>("http://localhost:" + port))
            {
                Console.WriteLine();
                Console.WriteLine("Started at http://localhost:" + port);
                Console.WriteLine("Press <ENTER> to stop the application");
                Console.ReadLine();
                Console.WriteLine("Stopping");
            }
        }
    }
}