using System;
using Microsoft.Owin.Hosting;

namespace Glimpse.Owin.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8080/"))
            {
                Console.WriteLine("Started");
                Console.ReadLine();
                Console.WriteLine("Stopping");
            }
        }
    }
}