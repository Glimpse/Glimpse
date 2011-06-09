using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin;
using Glimpse.Core.Plumbing;
using Moq;
using NUnit.Framework;

namespace Glimpse.Test.Core.Plugin
{
    [TestFixture]
    public class TraceTest
    {
        [Test]
        public void Trace_SetupInit_WithoutGlimpseTraceListner_ShouldAddListener()
        {
            System.Diagnostics.Trace.Listeners.Clear();

            Plugin.SetupInit();

            Assert.IsTrue(System.Diagnostics.Trace.Listeners.OfType<GlimpseTraceListener>().Any());
        }

        [Test]
        public void Trace_SetupInit_WithExistingGlimpseTraceListner_ShouldNotAddAnotherListener()
        {
            var glimpseTraceListener = new GlimpseTraceListener();
            System.Diagnostics.Trace.Listeners.Add(glimpseTraceListener);

            Plugin.SetupInit();

            CollectionAssert.Contains(System.Diagnostics.Trace.Listeners, glimpseTraceListener);
            Assert.AreEqual(1, System.Diagnostics.Trace.Listeners.OfType<GlimpseTraceListener>().Count());
        }

        [Test]
        public void Trace_Name_IsTrace()
        {
            var name = Plugin.Name;

            Assert.AreEqual("Trace", name);
        }

        [Test]
        public void Trace_GetData_WithoutAnyLoggedMessaged_IsNull()
        {
            Context.Setup(ctx => ctx.Items).Returns(new Dictionary<string, string>());

            var data = Plugin.GetData(Context.Object);

            Assert.Null(data);
        }

        [Test]
        public void Trace_GetData_IsNotNull()
        {
            var values = new List<IList<string>>{ new List<string> {"1", "2", "3"} };
            var dictionary = new Dictionary<object, object> { { Trace.TraceMessageStoreKey, values } };
            Context.Setup(ctx => ctx.Items).Returns(dictionary);

            var data = Plugin.GetData(Context.Object);

            Assert.AreEqual(values, data);
        }

        [Test]
        public void Trace_HelpUrl()
        {
            var helper = Plugin as IProvideGlimpseHelp;

            Assert.AreEqual("http://getGlimpse.com/Help/Plugin/Trace", helper.HelpUrl);
        }


        private IGlimpsePlugin Plugin { get; set; }
        private Mock<HttpContextBase> Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Plugin = new Trace();
            Context = new Mock<HttpContextBase>();
        }

        [TearDown]
        public void TearDown()
        {
            Plugin = null;
        }
    }
}
