using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Dapper;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        MusicStoreEntities storeDB = new MusicStoreEntities();

        public ActionResult CSPTest()
        {
            return this.View();
        }

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

            GetAllAlbums();

            return View(albums);
        }

        [NoCache]
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

        private int GetAllAlbums()
        {  
            var rowcount = 0; 
            var ds = new DataSet();

            var connectionString = ConfigurationManager.ConnectionStrings["MusicStoreEntities"];
            var factory = DbProviderFactories.GetFactory(connectionString.ProviderName);  //factory: {Glimpse.Ado.AlternateType.GlimpseDbProviderFactory<System.Data.SqlClient.SqlClientFactory>}

            using (DbCommand cmd = factory.CreateCommand()) //cmd: {Glimpse.Ado.AlternateType.GlimpseDbCommand} 
            { 
                //cmd.CommandType = CommandType.StoredProcedure; 
                cmd.CommandText = "SELECT * FROM Albums";

                using (DbConnection con = factory.CreateConnection()) //con: {Glimpse.Ado.AlternateType.GlimpseDbConnection} 
                {
                    con.ConnectionString = connectionString.ConnectionString; 
                    cmd.Connection = con;

                    IDbDataAdapter dbAdapter = factory.CreateDataAdapter(); //dbAdapter: {System.Data.SqlClient.SqlDataAdapter} not GlimpseDbDataAdapter 
                    dbAdapter.SelectCommand = cmd;

                    dbAdapter.Fill(ds); 
                } 
            }

            rowcount = ds.Tables[0].Rows.Count;

            return rowcount; 
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

                var albums = connection.Query<Album>("SELECT * FROM Albums WHERE Title LIKE 'K%'");
            }

            var test = storeDB.Database.ExecuteSqlCommand("SELECT count(*) FROM Albums WHERE Title LIKE 'The%'");


            return new Tuple<int, int>(result1, result2);
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public sealed class NoCacheAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();
                 
                base.OnResultExecuting(filterContext);
            }
        }
    }
}