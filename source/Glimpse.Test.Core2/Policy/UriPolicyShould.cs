using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Policy;
using Glimpse.Test.Core2.TestDoubles;
using Glimpse.Test.Core2.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Policy
{
    public class UriPolicyShould:IDisposable
    {
        private UriPolicyTester tester;
        public UriPolicyTester Policy
        {
            get { return tester ?? (tester = UriPolicyTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Policy = null;
        }

        [Fact]
        public void ConstructWithRegexList()
        {
            var blacklist = new List<Regex>();

            var policy = new UriPolicy(blacklist);

            Assert.Equal(blacklist, policy.UriBlacklist);
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullParameter()
        {
            Assert.Throws<ArgumentNullException>(()=>new UriPolicy(null));
        }

        [Fact]
        public void RetainRuntimePolicyWithEmptyBlacklist()
        {
            var policy = new UriPolicy();

            Assert.Equal(RuntimePolicy.On, policy.Execute(Policy.ContextMock.Object));
        }

        [Fact]
        public void RetainRuntimePolicyWithValidUrl()
        {
            Assert.Equal(RuntimePolicy.On, Policy.Execute(Policy.ContextMock.Object));
        }

        [Fact]
        public void ReduceRuntimePolicyWithMatchingExpression()
        {
            Policy.RequestMetadataMock.Setup(r => r.RequestUri).Returns("http://localhost/admin");

            Policy.UriBlacklist.Add(new Regex(".+/admin"));

            Assert.Equal(RuntimePolicy.Off, Policy.Execute(Policy.ContextMock.Object));
            
        }

        [Fact]
        public void ReduceRuntimePolicyOnException()
        {
            var exception = new DummyException("Houston, we have a problem");

            Policy.RequestMetadataMock.Setup(r => r.RequestUri).Throws(exception);

            Assert.Equal(RuntimePolicy.Off, Policy.Execute(Policy.ContextMock.Object));
            
            Policy.LoggerMock.Verify(l=>l.Warn(It.IsAny<string>(), exception), Times.Once());
        }

        [Fact]
        public void ExecuteOnBeginRequest()
        {
            Assert.Equal(RuntimeEvent.BeginRequest, Policy.ExecuteOn);
            
        }
    }
}