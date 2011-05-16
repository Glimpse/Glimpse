using System.IO;
using System.Web.Mvc;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpseView : IView
    {
        public IView View { get; set; }
        public ViewContext ViewContext { get; set; }

        public GlimpseView(IView view)
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