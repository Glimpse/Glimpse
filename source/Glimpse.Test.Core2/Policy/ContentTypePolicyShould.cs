using System;
using System.Collections.Generic;
using Glimpse.Core2;
using Glimpse.Core2.Policy;
using Glimpse.Test.Core2.TestDoubles;
using Glimpse.Test.Core2.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Policy
{
    public class ContentTypePolicyShould:IDisposable
    {
        private ContentTypePolicyTester tester;
        public ContentTypePolicyTester Policy
        {
            get { return tester ?? (tester = ContentTypePolicyTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Policy = null;
        }

        [Fact]
        public void RetainPolicyOnValidContentTypes()
        {
            Assert.Equal(RuntimePolicy.On, Policy.Execute(Policy.ContextMock.Object));
        }

        [Fact]
        public void ReducePolicyOnInvalidContentTypes()
        {
            Policy.RequestMetadataMock.Setup(r => r.ResponseContentType).Returns("Unsupported/Content+Type");

            Assert.Equal(RuntimePolicy.Off, Policy.Execute(Policy.ContextMock.Object));
        }

        [Fact]
        public void ReducePolicyOnError()
        {
            var ex = new DummyException("I am a problem.");
            Policy.RequestMetadataMock.Setup(r => r.ResponseContentType).Throws(ex);

            Assert.Equal(RuntimePolicy.Off, Policy.Execute(Policy.ContextMock.Object));
            Policy.LoggerMock.Verify(l=>l.Warn(It.IsAny<string>(), ex), Times.Once());
        }

        [Fact]
        public void ConstructWithNonNullWhitelist()
        {
            Assert.NotNull(Policy.ContentTypeWhitelist);
        }

        [Fact]
        public void ConstructWithDefaultContentTypes()
        {
            Assert.True(Policy.ContentTypeWhitelist.Count > 0);
        }

        [Fact]
        public void ConstructWithWhitelistArgument()
        {
            var list = new List<string>{"anything"};
            var policy = new ContentTypePolicy(list);

            Assert.Equal(list, policy.ContentTypeWhitelist);
        }
    }
}