using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glimpse.Core.Plumbing;
using System.IO;
using Moq;
using System.Web;
using Glimpse.Core;

namespace Glimpse.Test.Core.Plumbing
{
    [TestFixture]
    public class GlimpseResponseFilterTest
    {
        HttpContextBase httpContext;
        [Test]
        public void OutputDoesNotDuplicateHtmlIfWriteCalledTwice()
        {
            var encoding = UTF8Encoding.UTF8;
            string htmlFirst = "<html><head><title>THETITLE</title></head><body>";
            string htmlLast = "</body></html>";

            MemoryStream stream = new MemoryStream();
            var filter = new GlimpseResponseFilter(stream, httpContext);
            var firstBytes = encoding.GetBytes(htmlFirst);
            var secondBytes = encoding.GetBytes(htmlLast);
            filter.Write(firstBytes, 0, firstBytes.Count());
            filter.Write(secondBytes, 0, secondBytes.Count());
            var html = encoding.GetString(stream.ToArray());
            Assert.That(CountSubstrings(html, "THETITLE") == 1, "The Title html element was duplicated");
        }

        [SetUp]
        public void Setup()
        {
            var httpContextBaseMock = new Moq.Mock<HttpContextBase>();

            var httpRequestBase = new Moq.Mock<HttpRequestBase>();
            httpRequestBase.SetupGet(r => r.ApplicationPath).Returns(@"\");
            httpContextBaseMock.SetupGet(c => c.Items).Returns(new Dictionary<string, object>());
            httpRequestBase.SetupGet(r => r.Cookies).Returns(new HttpCookieCollection());
            httpRequestBase.Object.Cookies.Add(new HttpCookie(GlimpseConstants.CookieModeKey, "On"));

            httpContextBaseMock.SetupGet(c => c.Request).Returns(httpRequestBase.Object);
            httpContext = httpContextBaseMock.Object;
        }

        public int CountSubstrings(string @string, string substringToCount)
        {
            return @string.Select((c, i) => @string.Substring(i)).Count(sub => sub.StartsWith(substringToCount));
        }
        
    }
}
