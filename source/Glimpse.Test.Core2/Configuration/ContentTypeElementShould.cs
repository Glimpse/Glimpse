using Glimpse.Core2.Configuration;
using Xunit;

namespace Glimpse.Test.Core2.Configuration
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