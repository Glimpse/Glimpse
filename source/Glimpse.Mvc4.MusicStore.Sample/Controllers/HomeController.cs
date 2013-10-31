using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        private MusicStoreEntities storeDB = new MusicStoreEntities();
        //
        // GET: /Home/

        public async Task<ActionResult> Index()
        {
            // Get most popular albums
            var albums = await GetTopSellingAlbums(6);
            //var albums = GetTopSellingAlbums(6);

            // Trigger some good old ADO code 
            var albumCount = GetTotalAlbumns(); 
            Trace.Write(string.Format("Total number of Albums = {0} and Albums with 'The' = {1}", albumCount.Item1, albumCount.Item2));

            return View(albums);
        }


        private Task<List<Album>> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count

            return storeDB.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToListAsync();
        }

        private Tuple<int, int> GetTotalAlbumns()
        {
            var result1 = 0;
            var result2 = 0;

            var connectionString = ConfigurationManager.ConnectionStrings["MusicStoreEntities"];
            var factory = DbProviderFactories.GetFactory(connectionString.ProviderName);
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString.ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM Albums";
                    command.CommandType = CommandType.Text;
                    result1 = (int)command.ExecuteScalar();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM Albums WHERE Title LIKE 'The%'";
                    command.CommandType = CommandType.Text;
                    result2 = (int)command.ExecuteScalar();
                }
            }

            return new Tuple<int, int>(result1, result2);
        }
    }
}