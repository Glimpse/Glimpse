using Glimpse.AspNet.Extensibility; 
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;
using MvcMusicStore.Models;

namespace MvcMusicStore.Framework
{ 
    public class TabCart : AspNetTab
    {
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
    }
}