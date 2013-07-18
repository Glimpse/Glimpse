using System.Linq;
using Glimpse.AspNet.Extensibility; 
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;
using MvcMusicStore.Models;

namespace MvcMusicStore.Framework
{
    public class TabCart : AspNetTab, ITabLayout, ILayoutControl
    {
        private static readonly object Layout = TabLayout.Create()
                .Cell("items", TabLayout.Create().Row(r =>
                    {
                        r.Cell("{{albumTitle}} ({{albumId}})").AsKey().WithTitle("Album (Id)");
                        r.Cell("albumPrice").AlignRight().Prefix("$").WidthInPixels(100).WithTitle("Price");
                        r.Cell("genreName").WithTitle("Genre");
                        r.Cell("artistName").WithTitle("Artist");
                        r.Cell("count").Class("mono").WidthInPixels(70).WithTitle("Count");
                        r.Cell("dateCreated").WithTitle("Added");
                        r.Cell("recordId").WithTitle("Record Id");
                    })).Build();
         
        public override string Name
        {
            get { return "Cart"; }
        }

        public bool KeysHeadings
        {
            get { return true; }
        }

        public override object GetData(ITabContext context)
        {
            var httpContext = context.GetHttpContext();

            var cart = ShoppingCart.GetCart(httpContext);
            var items = cart.GetCartDetials();

            var root = new
            {
                Details = new {
                        CartId = ShoppingCart.GetCartId(httpContext), 
                        Total = items.Any() ? items.Sum(x => x.AlbumPrice).ToString() : "--"
                    },
                Items = items
            };

            return root;
        }

        public override RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndSessionAccess; }
        }

        public object GetLayout()
        {
            return Layout;
        }
    }
}