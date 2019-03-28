using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Glimpse.AspNet;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet
{
    public class RequestMetadataShould
    {
        private const string XMLHttpRequest = "XMLHttpRequest";
        private Mock<HttpContextBase> _httpContextMock;
        private NameValueCollection _headers;
        private NameValueCollection _queryString;

        public RequestMetadataShould()
        {
            _httpContextMock = new Mock<HttpContextBase>();
            _headers = new NameValueCollection();
            _queryString = new NameValueCollection();
            _httpContextMock.SetupGet(r => r.Request[It.IsAny<string>()]).Throws(new InvalidOperationException("Accessing the indexer will make underlaying request network stream not readable to some native modules"));
            _httpContextMock.SetupGet(r => r.Request.Form).Throws(new InvalidOperationException("Accessing the request form will make underlaying request network stream not readable to some native modules"));
            
            _httpContextMock.SetupGet(x => x.Request.Headers).Returns(_headers);
            _httpContextMock.SetupGet(x => x.Request.QueryString).Returns(_queryString);
        }

        [Theory]
        [InlineData(XMLHttpRequest)]
        [InlineData("NotAXMLHttpRequest")]
        public void IndicateAjaxRequestIfRequestedWithHeaderIsXmlHttpRequest(string requestedWith)
        {
            _headers.Add("X-Requested-With", requestedWith);
            
            var metadata = NewRequestMetadata();

            Assert.Equal(metadata.RequestIsAjax, requestedWith == XMLHttpRequest);
        }
        
        [Theory]
        [InlineData(XMLHttpRequest)]
        [InlineData("NotAXMLHttpRequest")]
        public void IndicateAjaxRequestIfQueryStringRequestedWithValueIsXmlHttpRequest(string requestedWith)
        {
            _queryString.Add("X-Requested-With", requestedWith);

            var metadata = NewRequestMetadata();

            Assert.Equal(metadata.RequestIsAjax, requestedWith == XMLHttpRequest);
        }

        private RequestMetadata NewRequestMetadata()
        {
            return new RequestMetadata(_httpContextMock.Object);
        }
    }
}
