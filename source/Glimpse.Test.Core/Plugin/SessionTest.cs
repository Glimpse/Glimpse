using System.Collections.Generic;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin;
using Moq;
using NUnit.Framework;

namespace Glimpse.Test.Core.Plugin
{
    [TestFixture]
    public class SessionTest
    {
        [Test]
        public void Session_Name_IsSession()
        {
            var name = Plugin.Name;

            Assert.AreEqual("Session", name);
        }

        [Test]
        public void Session_HelpUrl_IsRight()
        {
            var helper = Plugin as IProvideGlimpseHelp;

            Assert.AreEqual("http://getGlimpse.com/Help/Plugin/Session", helper.HelpUrl);
        }

        [Test]
        public void Session_SetupInit_DoesNothing()
        {
            Plugin.SetupInit();
        }

        [Test]
        public void Session_GetData_WithNullSession_ReturnsNull()
        {
            HttpSessionStateBase session = null;
            Context.Setup(ctx => ctx.Session).Returns(session);
            var data = Plugin.GetData(Context.Object);

            Assert.IsNull(data);
            Context.VerifyAll();
        }

        [Test]
        public void Session_GetData_WithEmptySession_ReturnsNull()
        {
            //TODO: Mock out session state
            var session = new Mock<HttpSessionStateBase>();
            Context.Setup(ctx => ctx.Session).Returns(session.Object);
            var data = Plugin.GetData(Context.Object);

            Assert.IsNull(data);
            Context.VerifyAll();
        }

        public IGlimpsePlugin Plugin { get; set; }
        public Mock<HttpContextBase> Context { get; set; }
        [SetUp]
        public void Setup()
        {
            Plugin = new Session();
            Context = new Mock<HttpContextBase>();
        }
    }
}
