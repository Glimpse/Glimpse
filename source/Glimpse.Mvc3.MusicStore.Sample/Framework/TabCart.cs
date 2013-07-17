using Glimpse.AspNet.Extensibility; 
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;
using MvcMusicStore.Models;

namespace MvcMusicStore.Framework
{
    public class TabCart : AspNetTab, ITabLayout
    {
        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell("{{albumTitle}} ({{albumId}})").AsKey().WithTitle("Album (Id)");
                    r.Cell("albumPrice").AlignRight().Prefix("$").WidthInPixels(100).WithTitle("Price");
                    r.Cell("genreName").WithTitle("Genre");
                    r.Cell("artistName").WithTitle("Artist");
                    r.Cell("count").Class("mono").WidthInPixels(70).WithTitle("Count");
                    r.Cell("dateCreated").WithTitle("Added");
                    r.Cell("recordId").WithTitle("Record Id");
                    r.Cell("cartId").WithTitle("Cart Id"); 
                }).Build();
         
        public override string Name
        {
            get { return "Cart"; }
        }

        public override object GetData(ITabContext context)
        {
            var cart = ShoppingCart.GetCart(context.GetHttpContext());
            var items = cart.GetCartDetials();

            return items;
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