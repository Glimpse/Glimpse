using System;
using System.Collections.Generic;
using Tavis.UriTemplates;
using Xunit;
using Glimpse.Core2.Extensions;

namespace Glimpse.Test.Core2.Extensions
{
    public class UriTemplateExtensionsShould
    {
        [Fact]
        public void ReplaceAllMatchingTokens()
        {
            var uriTemplate = new UriTemplate("A={A}{&B}");

            var a = Guid.NewGuid().ToString();
            var b = Guid.NewGuid().ToString();

            var dictionary = new Dictionary<string, string>{{"A", a}, {"B", b}};

            uriTemplate.SetParameters(dictionary);

            var result = uriTemplate.Resolve();

            Assert.Contains(a, result);
            Assert.Contains(b, result);
        }

        [Fact]
        public void SkipNonMatchingTokens()
        {
            var uriTemplate = new UriTemplate("A={A}");

            var a = Guid.NewGuid().ToString();
            var b = Guid.NewGuid().ToString();

            var dictionary = new Dictionary<string, string> { { "A", a }, { "B", b } };

            uriTemplate.SetParameters(dictionary);

            var result = uriTemplate.Resolve();

            Assert.Contains(a, result);
            Assert.DoesNotContain(b, result);
        }

        [Fact]
        public void SupportNullCollection()
        {
            var uriTemplate = new UriTemplate("A={A}{&B}");

            Assert.DoesNotThrow(() => uriTemplate.SetParameters(null));
        }

        [Fact]
        public void SupportEmptyCollection()
        {
            var uriTemplate = new UriTemplate("A={A}{&B}");

            Assert.DoesNotThrow(() => uriTemplate.SetParameters(new Dictionary<string,string>()));
        }
    }
}