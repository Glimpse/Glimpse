using System.Diagnostics;
using System.IO;
using System.Web.Mvc;

namespace Glimpse.Test.Mvc3.TestDoubles
{
    public class DummyView : IView
    {
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            Debug.WriteLine("In Render");
        }
    }
}