using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{ 
    public class InventoryManager
    {
        private readonly MusicStoreEntities StoreDb = new MusicStoreEntities();
        private readonly Random Random = new Random();

        private static IDictionary<int, Inventory> InventoryStore = new Dictionary<int, Inventory>();
        private static IList<string> Warehouses = new List<string> { "New York", "Portland", "Seattle", "Washington", "Dallas", "Los Angeles" };

        public static Inventory GetInventory(int albumId)
        {
            Inventory result;
            if (!InventoryStore.TryGetValue(albumId, out result))
            {
                var manager = new InventoryManager();
                result = manager.GenerateInventory(albumId);
                InventoryStore.Add(albumId, result);
            }

            return result;
        }


        public Inventory GenerateInventory(int albumId)
        {
            var inventory = new Inventory();

            var album = StoreDb.Albums.FirstOrDefault(x => x.AlbumId == albumId);
            if (album != null)
            {
                //Detials
                inventory.Details.Id = albumId;
                inventory.Details.Name = album.Title;

                //Warehouse
                for (var i = 0; i <= Random.Next(Warehouses.Count - 1); i++)
                {
                    inventory.Warehouse.Add(new InventoryLocations { Name = Warehouses[i], Available = Random.Next(50), Backorder = Random.Next(30), Sold = Random.Next(40) });
                }
            }

            return inventory;
        }
    }
}