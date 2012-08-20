using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core;
using Glimpse.Test.Core.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Policy
{
    public class AjaxPolicyShould:IDisposable
    {
        private AjaxPolicyTester tester;
        private AjaxPolicyTester Policy
        {
            get { return tester ?? (tester = AjaxPolicyTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Policy = null;
        }

        [Fact]
        public void ReducePermissionsOnAjaxRequest()
        {
            
            Policy.RequestMetadataMock.Setup(r => r.RequestIsAjax).Returns(true);

            Assert.Equal(RuntimePolicy.ModifyResponseHeaders, Policy.Execute(Policy.ContextMock.Object));
        }

        [Fact]
        public void KeepPermissionsWithoutAjaxRequest()
        {
            Assert.Equal(RuntimePolicy.On, Policy.Execute(Policy.ContextMock.Object));
        }

        [Fact]
        public void ReducePermissionsOnException()
        {
            var exception = new Exception("Fake error");
            Policy.RequestMetadataMock.Setup(r => r.RequestIsAjax).Throws(exception);

            Assert.Equal(RuntimePolicy.ModifyResponseHeaders, Policy.Execute(Policy.ContextMock.Object));
            Policy.LoggerMock.Verify(l => l.Warn(It.IsAny<string>(), exception, It.IsAny<object[]>()), Times.Once());
        }

        [Fact]
        public void ThrowArgumentNullExceptionWithMissingPolicyContext()
        {
            Assert.Throws<ArgumentNullException>(()=>Policy.Execute(null));
        }

        [Fact]
        public void ExecuteOnBeginRequest()
        {
            Assert.Equal(RuntimeEvent.BeginRequest, Policy.ExecuteOn);
        }
    }
}