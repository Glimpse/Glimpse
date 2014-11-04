using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Policy;
using Glimpse.Test.Core.TestDoubles;
using Glimpse.Test.Core.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Policy
{
    public class StatusCodePolicyShould : IDisposable
    {
        private StatusCodePolicyTester tester;
        public StatusCodePolicyTester Policy
        {
            get { return tester ?? (tester = StatusCodePolicyTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Policy = null;
        }

        [Fact]
        public void ReducePolicyWithInvalidStatusCode()
        {
            Assert.Equal(RuntimePolicy.Off, Policy.Execute(Policy.ContextMock.Object));
        }

        [Fact]
        public void RetainPolicyWithValidStatusCode()
        {
            Policy.RequestMetadataMock.Setup(rm => rm.ResponseStatusCode).Returns(200);

            Assert.Equal(RuntimePolicy.On, Policy.Execute(Policy.ContextMock.Object));
        }

        [Fact]
        public void RespectConfigredStatusCodeList()
        {
            var codes = new List<int> { 5, 6, 7 };

            var policy = new StatusCodePolicy();
            policy.Configurator.AddSupportedStatusCodes(codes);

            foreach (var code in codes)
            {
                Policy.RequestMetadataMock.Setup(rm => rm.ResponseStatusCode).Returns(code);

                Assert.Equal(RuntimePolicy.On, policy.Execute(Policy.ContextMock.Object));
            }
        }

        [Fact]
        public void ReducePolicyOnError()
        {
            var exception = new DummyException("I am a problem");

            Policy.RequestMetadataMock.Setup(r => r.ResponseStatusCode).Throws(exception);

            Assert.Equal(RuntimePolicy.Off, Policy.Execute(Policy.ContextMock.Object));

            Policy.LoggerMock.Verify(l => l.Warn(It.IsAny<string>(), exception, It.IsAny<object[]>()), Times.Once());
        }

        [Fact]
        public void ExecuteOnEndRequest()
        {
            Assert.Equal(RuntimeEvent.EndRequest, Policy.ExecuteOn);
        }
    }
}