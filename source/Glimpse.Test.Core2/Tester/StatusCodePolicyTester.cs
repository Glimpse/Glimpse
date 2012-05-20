using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.Policy;
using Moq;

namespace Glimpse.Test.Core2.Tester
{
    public class StatusCodePolicyTester : StatusCodePolicy
    {
        public Mock<IRequestMetadata> RequestMetadataMock { get; set; }
        public Mock<IRuntimePolicyContext> ContextMock { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }

        private StatusCodePolicyTester(IList<int> statusCodes):base(statusCodes)
        {
            RequestMetadataMock = new Mock<IRequestMetadata>();
            RequestMetadataMock.Setup(r => r.ResponseStatusCode).Returns(500);

            LoggerMock = new Mock<ILogger>();

            ContextMock = new Mock<IRuntimePolicyContext>();
            ContextMock.Setup(pc => pc.RequestMetadata).Returns(RequestMetadataMock.Object);
            ContextMock.Setup(pc => pc.Logger).Returns(LoggerMock.Object);
        }

        public static StatusCodePolicyTester Create()
        {
            return new StatusCodePolicyTester(new List<int>{200,301,302});
        }
    }
}