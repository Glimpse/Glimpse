using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin;
using Moq;
using NUnit.Framework;

namespace Glimpse.Test.Core.Plugin
{
    [TestFixture]
    public class ServerTest
    {
        [Test]
        public void Server_Name_IsServer()
        {
            var name = Plugin.Name;

            Assert.AreEqual("Server", name);
        }

        [Test]
        public void Server_HelpUrl_IsRight()
        {
            var helper = Plugin as IProvideGlimpseHelp;

            Assert.AreEqual("http://getGlimpse.com/Help/Plugin/Server", helper.HelpUrl);
        }

        [Test]
        public void Server_SetupInit_ShouldDoNothing()
        {
            Plugin.SetupInit();
        }

        [Test]
        public void Server_GetData_WithNull_ShouldReturnNull()
        {
            NameValueCollection serverVars = null;
            Context.Setup(ctx => ctx.Request.ServerVariables).Returns(serverVars);

            var data = Plugin.GetData(Context.Object);

            Assert.IsNull(data);
        }

        [Test]
        public void Server_GetData_WithEmptyCollection_ShouldReturnNull()
        {
            NameValueCollection serverVars = new NameValueCollection();
            Context.Setup(ctx => ctx.Request.ServerVariables).Returns(serverVars);

            var data = Plugin.GetData(Context.Object);

            Assert.IsNull(data);
        }

        [Test]
        public void Server_GetData_WithCollection_ShouldReturnIDictionary()
        {
            NameValueCollection serverVars = new NameValueCollection();
            serverVars.Add("key1","value1");
            Context.Setup(ctx => ctx.Request.ServerVariables).Returns(serverVars);

            var data = Plugin.GetData(Context.Object);

            Assert.IsInstanceOf(typeof(IDictionary<string,string>), data);
        }

        private IGlimpsePlugin Plugin { get; set; }
        private Mock<HttpContextBase> Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Plugin = new Server();
            Context = new Mock<HttpContextBase>();
        }
    }
}