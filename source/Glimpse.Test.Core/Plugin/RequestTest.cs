using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin;
using NUnit.Framework;

namespace Glimpse.Test.Core.Plugin
{
    [TestFixture]
    public class RequestTest
    {
        [Test]
        public void Request_Name_IsRequest()
        {
            var name = Plugin.Name;

            Assert.AreEqual("Request", name);
        }

        private IGlimpsePlugin Plugin { get; set; }

        [SetUp]
        public void Setup()
        {
            Plugin = new Request();
        }
    }
}
