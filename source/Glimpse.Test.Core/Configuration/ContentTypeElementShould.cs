using Glimpse.Core.Configuration;
using Xunit;

namespace Glimpse.Test.Core.Configuration
{
    public class ContentTypeElementShould
    {
        [Fact]
        public void GetAndSetContentTypes()
        {
            var contentType = "content/type";

            var element = new ContentTypeElement();
            element.ContentType = contentType;

            Assert.Equal(contentType, element.ContentType);
        }
    }
}