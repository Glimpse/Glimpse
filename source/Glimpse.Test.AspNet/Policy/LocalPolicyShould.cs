using System;
using System.Web;
using Glimpse.AspNet.Policy;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet.Policy
{
    public class LocalPolicyShould
    {
        [Fact]
        public void ExecuteAsEarlyAsPossible()
        {
            var policy = new LocalPolicy();
            Assert.Equal(RuntimeEvent.BeginRequest, policy.ExecuteOn);
        }

        [Fact]
        public void ThrowExceptionWithNullPolicyContext()
        {
            var policy = new LocalPolicy();

            Assert.Throws<ArgumentNullException>(() => policy.Execute(null));
        }

        [Theory]
        [InlineData(true, RuntimePolicy.On)]
        [InlineData(false, RuntimePolicy.Off)]
        public void LeaveGlimpseOnWithLocalRequest(bool isLocal, RuntimePolicy expectedPolicy)
        {
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(m => m.Request.IsLocal).Returns(isLocal);

            var policyContextMock = new Mock<IRuntimePolicyContext>();
            policyContextMock.Setup(m => m.GetRequestContext<HttpContextBase>()).Returns(httpContextMock.Object);

            var policy = new LocalPolicy();
            var result = policy.Execute(policyContextMock.Object);

            Assert.Equal(expectedPolicy, result);
        }
    }
}