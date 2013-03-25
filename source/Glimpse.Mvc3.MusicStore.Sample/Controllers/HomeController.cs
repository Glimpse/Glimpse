using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
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
            var albumCount = GetTotalAlbumns();

            Trace.Write(string.Format("Total number of Albums = {0} and Albums with 'The' = {1}", albumCount.Item1, albumCount.Item2));
            Trace.Write("Got top 5 albums");
            Trace.TraceWarning("Test TraceWarning;");
            Trace.IndentLevel++;
            Trace.TraceError("Test TraceError;");
            Trace.Write("Another trace line");
            Trace.IndentLevel++;
            Trace.Write("Yet another trace line");
            Trace.IndentLevel = 0;
            Trace.TraceInformation("Test TraceInformation;");

            HttpContext.Session["TestObject"] = new Artist { ArtistId = 123, Name = "Test Artist" };

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

        private Tuple<int, int> GetTotalAlbumns()
        {
            var result1 = 0;
            var result2 = 0;
            var result3 = 0;

            var connectionString = ConfigurationManager.ConnectionStrings["MusicStoreEntities"];
            var factory = DbProviderFactories.GetFactory(connectionString.ProviderName); 
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString.ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                { 
                    command.CommandText = "SELECT COUNT(*) FROM Albums WHERE Title LIKE 'A%'";
                    command.CommandType = CommandType.Text;
                    result3 = (int)command.ExecuteScalar();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM Albums WHERE Title LIKE 'B%'";
                    command.CommandType = CommandType.Text;
                    result3 = (int)command.ExecuteScalar();
                }

                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = "SELECT COUNT(*) FROM Albums";
                        command.CommandType = CommandType.Text;
                        result1 = (int)command.ExecuteScalar();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = "SELECT COUNT(*) FROM Albums WHERE Title LIKE 'C%'";
                        command.CommandType = CommandType.Text;
                        result2 = (int)command.ExecuteScalar();
                    }

                    transaction.Commit();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM Albums WHERE Title LIKE 'D%'";
                    command.CommandType = CommandType.Text;
                    result3 = (int)command.ExecuteScalar();
                }
                    
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = "SELECT COUNT(*) FROM Albums WHERE Title LIKE 'E%'";
                        command.CommandType = CommandType.Text;
                        result3 = (int)command.ExecuteScalar();
                    } 
                    //transaction.Commit();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM Albums WHERE Title LIKE 'F%'";
                    command.CommandType = CommandType.Text;
                    result3 = (int)command.ExecuteScalar();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM Albums WHERE Title LIKE 'G%'";
                    command.CommandType = CommandType.Text;
                    result3 = (int)command.ExecuteScalar();
                }
            }

            using (var connection = factory.CreateConnection())
            {  
                connection.ConnectionString = connectionString.ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Albums WHERE Title LIKE 'I%'";
                    command.CommandType = CommandType.Text;
                    var result = command.ExecuteReader();
                }

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Albums WHERE Title LIKE 'J%'";
                        command.CommandType = CommandType.Text;
                        var result = command.ExecuteReader();
                    }

                    scope.Complete();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM Albums WHERE Title LIKE 'K%'";
                    command.CommandType = CommandType.Text;
                    result3 = (int)command.ExecuteScalar();
                } 
            }

            var test = storeDB.Database.ExecuteSqlCommand("SELECT count(*) FROM Albums WHERE Title LIKE 'The%'");


            return new Tuple<int, int>(result1, result2);
        }
    }
}