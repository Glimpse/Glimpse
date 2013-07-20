//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Web;
//using Glimpse.Core.Extensibility;
//using Glimpse.Core.Extensions;
//using Glimpse.Core.Framework;
//using Glimpse.Core.ResourceResult;
//using MvcMusicStore.Models;

//namespace MvcMusicStore.Framework
//{
//    public class InventoryResource: IResource
//    {
//        private const string AlbumIdKey = "albumId";

//        public string Name
//        {
//            get { return "music_query"; }
//        }

//        public IEnumerable<ResourceParameterMetadata> Parameters
//        {
//            get { return new[] { new ResourceParameterMetadata(AlbumIdKey) }; }
//        }

//        public IResourceResult Execute(IResourceContext context)
//        {
//            var queryValue = int.Parse(context.Parameters.GetValueOrDefault(AlbumIdKey));

//            var data = InventoryManager.GetInventory(queryValue);

//            return new CacheControlDecorator(0, CacheSetting.NoCache, new JsonResourceResult(data, null));;
//        }

//        public class InventoryManager
//        {
//            private readonly MusicStoreEntities StoreDb = new MusicStoreEntities();
//            private readonly Random Random = new Random();

//            private static IDictionary<int, Inventory> InventoryStore = new Dictionary<int, Inventory>();
//            private static IList<string> Warehouses = new List<string> { "New York", "Portland", "Seattle", "Washington", "Dallas", "Los Angeles" };

//            public static Inventory GetInventory(int albumId)
//            {
//                Inventory result;
//                if (!InventoryStore.TryGetValue(albumId, out result))
//                {
//                    var manager = new InventoryManager();
//                    result = manager.GenerateInventory(albumId);
//                    InventoryStore.Add(albumId, result);
//                }

//                return result;
//            }


//            public Inventory GenerateInventory(int albumId)
//            {
//                var inventory = new Inventory();

//                var album = StoreDb.Albums.FirstOrDefault(x => x.AlbumId == albumId);
//                if (album != null)
//                {
//                    //Detials
//                    inventory.Details.Id = albumId;
//                    inventory.Details.Name = album.Title;

//                    //Warehouse
//                    for (var i = 0; i <= Random.Next(Warehouses.Count - 1); i++)
//                    {
//                        inventory.Warehouse.Add(new InventoryLocations { Name = Warehouses[i], Available = Random.Next(50), Backorder = Random.Next(30), Sold = Random.Next(40) });
//                    }
//                }

//                return inventory;
//            }

//            public class Inventory
//            {
//                public Inventory()
//                {
//                    Details = new InventoryDetail(this);
//                    Warehouse = new List<InventoryLocations>();
//                }

//                public InventoryDetail Details { get; private set; }

//                public IList<InventoryLocations> Warehouse { get; private set; }
//            }

//            public class InventoryDetail
//            {
//                public InventoryDetail(Inventory root)
//                {
//                    Root = root;
//                }

//                private Inventory Root { get; set; }

//                public int Id { get; set; }

//                public string Name { get; set; }

//                public int Available
//                {
//                    get { return Root.Warehouse.Sum(x => x.Available); }
//                }

//                public int Backorder
//                {
//                    get { return Root.Warehouse.Sum(x => x.Backorder); }
//                }

//                public int Sold
//                {
//                    get { return Root.Warehouse.Sum(x => x.Sold); }
//                }
//            }

//            public class InventoryLocations
//            {
//                public string Name { get; set; }

//                public int Available { get; set; }

//                public int Backorder { get; set; }

//                public int Sold { get; set; }
//            }
//        }
//    }
//}