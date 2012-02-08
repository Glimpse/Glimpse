using System;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Test.Core2.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Policy
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
            Policy.LoggerMock.Verify(l=>l.Warn(It.IsAny<string>(), exception), Times.Once());
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