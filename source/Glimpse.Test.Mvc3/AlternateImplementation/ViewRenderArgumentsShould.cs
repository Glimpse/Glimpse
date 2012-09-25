using System.IO;
using System.Web.Mvc;
using Glimpse.Mvc.AlternateImplementation;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewRenderArgumentsShould
    {
        [Fact]
        public void SetProperties()
        {
            var viewContext = new ViewContext();
            var textWriter = new StringWriter();
            var arguments = new View.Render.Arguments(new object[] {viewContext, textWriter});

            Assert.Equal(viewContext, arguments.ViewContext);
            Assert.Equal(textWriter, arguments.Writer);
        } 
    }
}