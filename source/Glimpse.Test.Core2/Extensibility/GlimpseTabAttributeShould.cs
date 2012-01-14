using System;
using Glimpse.Core2.Extensibility;
using Glimpse.Test.Core2.TestDoubles;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class GlimpseTabAttributeShould
    {


        [Fact]
        public void ConstructWith0Arguments()
        {
            var attribute = new TabAttribute();
            Assert.Null(attribute.RequestContextType);
            Assert.Equal(LifeCycleSupport.EndRequest, attribute.LifeCycleSupport);
            
        }

        [Fact]
        public void ConstructWith1Arguments()
        {
            var type = typeof (DummyObjectContext);
            var attribute = new TabAttribute(type);
            Assert.Equal(type, attribute.RequestContextType);
            Assert.Equal(LifeCycleSupport.EndRequest, attribute.LifeCycleSupport);
        }

        [Fact]
        public void ConstructWith2Arguments()
        {
            var type = typeof(DummyObjectContext);
            var lifecycleSupport = LifeCycleSupport.SessionAccessEnd;
            var attribute = new TabAttribute(type, lifecycleSupport);
            Assert.Equal(type, attribute.RequestContextType);
            Assert.Equal(lifecycleSupport, attribute.LifeCycleSupport);
        }

        [Fact]
        public void ThrowExceptionWithNullRequestContextType()
        {
            Assert.Throws<ArgumentNullException>(()=>new TabAttribute(null));
            Assert.Throws<ArgumentNullException>(()=>new TabAttribute(null, LifeCycleSupport.EndRequest));
        }
    }
}