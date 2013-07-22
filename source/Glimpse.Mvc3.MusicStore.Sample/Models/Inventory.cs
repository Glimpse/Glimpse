using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{ 
    public class Inventory
    {
        public Inventory()
        {
            Details = new InventoryDetail(this);
            Warehouse = new List<InventoryLocations>();
        }

        public InventoryDetail Details { get; private set; }

        public IList<InventoryLocations> Warehouse { get; private set; }
    }

    public class InventoryDetail
    {
        public InventoryDetail(Inventory root)
        {
            Root = root;
        }

        private Inventory Root { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Available
        {
            get { return Root.Warehouse.Sum(x => x.Available); }
        }

        public int Backorder
        {
            get { return Root.Warehouse.Sum(x => x.Backorder); }
        }

        public int Sold
        {
            get { return Root.Warehouse.Sum(x => x.Sold); }
        }
    }

    public class InventoryLocations
    {
        public string Name { get; set; }

        public int Available { get; set; }

        public int Backorder { get; set; }

        public int Sold { get; set; }
    }
}