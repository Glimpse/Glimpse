using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Policy;
using Moq;

namespace Glimpse.Test.Core.Tester
{
    public class UriPolicyTester : UriPolicy
    {
        public Mock<IRequestMetadata> RequestMetadataMock { get; set; }
        public Mock<IRuntimePolicyContext> ContextMock { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }

        private UriPolicyTester()
        {
            Configurator.AddUriPatternToIgnore("blocked");

            RequestMetadataMock = new Mock<IRequestMetadata>();
            RequestMetadataMock.Setup(r => r.RequestUri).Returns(new Uri("http://should.not.matter"));

            LoggerMock = new Mock<ILogger>();

            ContextMock = new Mock<IRuntimePolicyContext>();
            ContextMock.Setup(c => c.RequestMetadata).Returns(RequestMetadataMock.Object);
            ContextMock.Setup(c => c.Logger).Returns(LoggerMock.Object);
        }

        public static UriPolicyTester Create()
        {
            return new UriPolicyTester();
        }
    }
}