using System.Web.Configuration;
using Glimpse.Core;
using Glimpse.Core.Extensions;
using System.Web;
using Moq;
using NUnit.Framework;

namespace Glimpse.Test.Core.Extensions
{
    [TestFixture]
    public class HttpContextBaseExtensionsTest
    {
        [Test]
        public void ResourceRoot_NoHandlers_DefaultsPath()
        {
            var rootPath= HttpContextBaseExtensions.GetResourceRootFromHandler(Handlers);

            Assert.AreEqual("~/", rootPath);
        }

        [Test]
        public void ResourceRoot_BasePath()
        {
            Handlers.Handlers.Add(new HttpHandlerAction("Glimpse.axd", typeof(Handler).FullName, "GET,POST"));
            var rootPath= HttpContextBaseExtensions.GetResourceRootFromHandler(Handlers);

            Assert.AreEqual("~/", rootPath);
        }

        [Test]
        public void ResourceRoot_WithSubDirectory()
        {
            Handlers.Handlers.Add(new HttpHandlerAction("MyDirectory/Glimpse.axd", typeof(Handler).FullName, "GET,POST"));
            var rootPath= HttpContextBaseExtensions.GetResourceRootFromHandler(Handlers);

            Assert.AreEqual("~/MyDirectory/", rootPath);
        }

        [Test]
        public void ResourceRoot_WithPrecedingSlash()
        {
            Handlers.Handlers.Add(new HttpHandlerAction("/Glimpse.axd", typeof(Handler).FullName, "GET,POST"));
            var rootPath= HttpContextBaseExtensions.GetResourceRootFromHandler(Handlers);

            Assert.AreEqual("~/", rootPath);
        }

        public HttpHandlersSection Handlers { get; set; }
        public Mock<HttpContextBase> Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Handlers = new HttpHandlersSection();
            Context = new Mock<HttpContextBase>();
        }
    }
}
