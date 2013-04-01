using System.IO;
using System.Web.Mvc;
using Glimpse.Mvc.AlternateType;
using Xunit;

namespace Glimpse.Test.Mvc.AlternateType
{
    public class ViewRenderArgumentsShould
    {
        [Fact]
        public void SetProperties()
        {
            var viewContext = new ViewContext();
            var textWriter = new StringWriter();
            var sut = new View.Render.Arguments(viewContext, textWriter);

            Assert.Equal(viewContext, sut.ViewContext);
            Assert.Equal(textWriter, sut.Writer);
        } 
    }
}