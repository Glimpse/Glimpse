using System.IO;
using System.Web.Mvc;

namespace Glimpse.Mvc3.Plumbing
{
    public class GlimpseView : IView
    {
        public IView View { get; set; }
        internal ViewContext ViewContext { get; set; }

        internal GlimpseView(IView view)
        {
            View = view;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            ViewContext = viewContext;

            View.Render(viewContext, writer);
        }
    }
}