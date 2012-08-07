using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Glimpse.AspNet.Model;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet.Model
{
    public class RequestModelShould
    {
        [Fact]
        public void Construct()
        {
            var serverUtilMock = new Mock<HttpServerUtilityBase>();
            serverUtilMock.Setup(s => s.UrlDecode(It.IsAny<string>())).Returns("decoded");

            var requestMock = new Mock<HttpRequestBase>();
            var applicationPath = "/";
            requestMock.Setup(r => r.ApplicationPath).Returns(applicationPath);
            var appRelativeCurrentExeFilePath = "/";
            requestMock.Setup(r => r.AppRelativeCurrentExecutionFilePath).Returns(appRelativeCurrentExeFilePath);
            var currentExeFilePath = "/";
            requestMock.Setup(r => r.CurrentExecutionFilePath).Returns(currentExeFilePath);
            var filePath = "/";
            requestMock.Setup(r => r.FilePath).Returns(filePath);
            var path = "/";
            requestMock.Setup(r => r.Path).Returns(path);
            var pathInfo = "/";
            requestMock.Setup(r => r.PathInfo).Returns(pathInfo);
            var physicalApplicationPath = "/";
            requestMock.Setup(r => r.PhysicalApplicationPath).Returns(physicalApplicationPath);
            var physicalPath = "/";
            requestMock.Setup(r => r.PhysicalPath).Returns(physicalPath);
            var rawUrl = "/";
            requestMock.Setup(r => r.RawUrl).Returns(rawUrl);
            var url = new Uri("http://localhost/");
            requestMock.Setup(r => r.Url).Returns(url);
            var urlReferrer = new Uri("http://localhost/");
            requestMock.Setup(r => r.UrlReferrer).Returns(urlReferrer);
            var userAgent = "agent";
            requestMock.Setup(r => r.UserAgent).Returns(userAgent);
            var userHostAddress = "127.0.0.1";
            requestMock.Setup(r => r.UserHostAddress).Returns(userHostAddress);
            var userHostName = "host name";
            requestMock.Setup(r => r.UserHostName).Returns(userHostName);

            var cookies = new HttpCookieCollection {new HttpCookie("TestKey", "TestValue")};
            requestMock.Setup(r => r.Cookies).Returns(cookies);
            
            var queryString = new NameValueCollection {{"TestKey", "TestValue"}};
            requestMock.Setup(r => r.QueryString).Returns(queryString);

            var contextMock = new Mock<HttpContextBase>();
            contextMock.Setup(c => c.Request).Returns(requestMock.Object);
            contextMock.Setup(c => c.Server).Returns(serverUtilMock.Object);


            var model = new RequestModel(contextMock.Object);

            Assert.Equal(applicationPath, model.ApplicationPath);
            Assert.Equal(appRelativeCurrentExeFilePath, model.AppRelativeCurrentExecutionFilePath);
            Assert.Equal(currentExeFilePath, model.CurrentExecutionFilePath);
            Assert.Equal(filePath, model.FilePath);
            Assert.Equal(path, model.Path);
            Assert.Equal(pathInfo, model.PathInfo);
            Assert.Equal(physicalApplicationPath, model.PhysicalApplicationPath);
            Assert.Equal(physicalPath, model.PhysicalPath);
            Assert.Equal(rawUrl, model.RawUrl);
            Assert.Equal(url, model.Url);
            Assert.Equal(urlReferrer, model.UrlReferrer);
            Assert.Equal(userAgent, model.UserAgent);
            Assert.Equal(userHostAddress, model.UserHostAddress);
            Assert.Equal(userHostName, model.UserHostName);

            Assert.Equal(1, model.Cookies.Count());
            Assert.Equal(1, model.QueryString.Count());

            Assert.Equal("TestKey", model.Cookies.First().Name);
            Assert.Equal("TestKey", model.QueryString.First().Key);
        }
    }
}