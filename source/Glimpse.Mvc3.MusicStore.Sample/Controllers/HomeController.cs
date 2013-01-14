using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        MusicStoreEntities storeDB = new MusicStoreEntities();

        public ActionResult Index()
        {
            // Get most popular albums
            var albums = GetTopSellingAlbums(5);


            Trace.Write("Got top 5 albums");
            Trace.TraceWarning("Test TraceWarning;");
            Trace.IndentLevel++;
            Trace.TraceError("Test TraceError;");
            Trace.Write("Another trace line");
            Trace.IndentLevel++;
            Trace.Write("Yet another trace line");
            Trace.IndentLevel = 0;
            Trace.TraceInformation("Test TraceInformation;");


            TraceSource ts = new TraceSource("Test source");

            ts.TraceEvent(TraceEventType.Warning, 0, string.Format("{0}: {1}", "trace", "source"));

            return View(albums);
        }

        public virtual ActionResult News()
        {
            var views = new[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight" };

            var randomIndex = new Random().Next(0, views.Count());

            Trace.Write("Randomly selected story number " + randomIndex);

            return PartialView(views[randomIndex]);
        }

        private List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count

            return storeDB.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }
    }
}