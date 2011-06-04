using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public partial class StoreController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        //
        // GET: /Store/

        public virtual ActionResult Index()
        {
            var genres = storeDB.Genres.ToList();

            Trace.Write(string.Format("There are {0} genres in the store.", genres.Count));

            return View(genres);
        }

        //
        // GET: /Store/Browse?genre=?Disco

        public virtual ActionResult Browse(string genre)
        {
            // Retrieve Genre and its Associated Albums from database
            var genreModel = storeDB.Genres.Include("Albums")
                .Single(g => g.Name == genre);

            Trace.Write(string.Format("Showing {0} albums for {1} genre.", genreModel.Albums.Count, genre));


            return View(genreModel);
        }

        //
        // GET: /Store/Details/5

        public virtual ActionResult Details(int id)
        {
            Album album = storeDB.Albums.Find(id);

            return View(album);
        }

        //
        // GET: /Store/GenreMenu

        [ChildActionOnly]
        public virtual ActionResult GenreMenu()
        {
            var genres = storeDB.Genres.ToList();

            return PartialView(genres);
        }
    }
}
