using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Core.Tab;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Tab
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
            var model = new ITraceMessage[0];
            var dataStoreMock = new Mock<IDataStore>();
            dataStoreMock.Setup(c => c.Get(typeof(IList<ITraceMessage>).AssemblyQualifiedName)).Returns(model);
            var contextMock = new Mock<ITabContext>();
            contextMock.SetupGet(c => c.TabStore).Returns(dataStoreMock.Object);

            var trace = new Trace();
            var result = trace.GetData(contextMock.Object);

            Assert.NotNull(result);
            Assert.Equal(model, result);
        } 
    }
}
