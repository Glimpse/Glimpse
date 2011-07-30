using System.Collections.Generic;
using System.Web;
using Glimpse.Core;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Handlers;
using Glimpse.Core.Validator;
using Moq;
using NUnit.Framework;

namespace Glimpse.Test.Core
{
    [TestFixture]
    public class IpAddressValidatorTest
    {
        [Test]
        public void IpAddressValidator_ValidateWithNoFilters_ReturnsTrue()
        {
            Context.Setup(ctx => ctx.Request.UserHostAddress).Returns("1.1.1.1");
            
            Assert.IsTrue(Validator.IsValid(Context.Object, Configuration, LifecycleEvent.BeginRequest));
        }
        
        [Test]
        public void IpAddressValidator_ValidateIP4Address_ReturnsTrue()
        {
            Context.Setup(ctx => ctx.Request.UserHostAddress).Returns("127.0.0.1");
            
            Configuration.IpAddresses.Add(new IpAddress{Address = "127.0.0.1"});

            Assert.IsTrue(Validator.IsValid(Context.Object, Configuration, LifecycleEvent.BeginRequest));
        }
        
        [Test]
        public void IpAddressValidator_ValidateIP6Address_ReturnsTrue()
        {
            Context.Setup(ctx => ctx.Request.UserHostAddress).Returns("::1");
            
            Configuration.IpAddresses.Add(new IpAddress{Address = "::1"});

            Assert.IsTrue(Validator.IsValid(Context.Object, Configuration, LifecycleEvent.BeginRequest));
        }
        
        [Test]
        public void IpAddressValidator_ValidateIncorrectAddress_ReturnsFalse()
        {
            Context.Setup(ctx => ctx.Request.UserHostAddress).Returns("127.0.0.2");
            
            Configuration.IpAddresses.Add(new IpAddress{Address = "127.0.0.1"});
            
            Assert.IsFalse(Validator.IsValid(Context.Object, Configuration, LifecycleEvent.BeginRequest));
        }
        
        [Test]
        public void IpAddressValidator_ValidateAddressInRange_ReturnsTrue()
        {
            Context.Setup(ctx => ctx.Request.UserHostAddress).Returns("127.0.0.2");
            
            Configuration.IpAddresses.Add(new IpAddress{AddressRange = "127.0.0.1/24"});
            
            Assert.IsTrue(Validator.IsValid(Context.Object, Configuration, LifecycleEvent.BeginRequest));
        }
        
        [Test]
        public void IpAddressValidator_ValidateAddressOutOfRange_ReturnsFalse()
        {
            Context.Setup(ctx => ctx.Request.UserHostAddress).Returns("127.1.1.1");
            
            Configuration.IpAddresses.Add(new IpAddress{AddressRange = "127.0.0.1/24"});
            
            Assert.IsFalse(Validator.IsValid(Context.Object, Configuration, LifecycleEvent.BeginRequest));
        }

        public IpAddressValidator Validator { get; set; }
        public Mock<HttpContextBase> Context { get; set; }
        public GlimpseConfiguration Configuration { get; set; }

        [SetUp]
        public void Setup()
        {
            Validator = new IpAddressValidator();
            Context = new Mock<HttpContextBase>();
            Configuration = new GlimpseConfiguration();
            Configuration.IpAddresses.Clear();
        }
    }
}