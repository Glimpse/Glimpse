using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.Tab;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet.Tab
{
    public class TraceShould
    {
        [Fact]
        public void HaveProperContextObjectType()
        {
            var trace = new Trace();

            Assert.Null(trace.RequestContextType);
        }

        [Fact]
        public void UseDefaultLifeCycleSupport()
        {
            var trace = new Trace();
            Assert.Equal(RuntimeEvent.EndRequest, trace.ExecuteOn);
        }

        [Fact]
        public void BeNamedTrace()
        {
            var trace = new Trace();
            Assert.Equal("Trace", trace.Name);
        }

        [Fact]
        public void HaveADocumentationUri()
        {
            var trace = new Trace();

            Assert.False(string.IsNullOrWhiteSpace(trace.DocumentationUri));
        }
         
        [Fact]
        public void ReturnData()
        { 
            var model = new List<TraceModel>();
            var dataStoreMock = new Mock<IDataStore>();
            dataStoreMock.Setup(c => c.Get<IList<TraceModel>>(Trace.TraceMessageStoreKey)).Returns(model);
            var contextMock = new Mock<ITabContext>();
            contextMock.SetupGet(c => c.TabStore).Returns(dataStoreMock.Object);

            var trace = new Trace();
            var result = trace.GetData(contextMock.Object);

            Assert.NotNull(result);
            Assert.Same(model, result);
        } 
    }
}
