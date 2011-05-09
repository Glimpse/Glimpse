using System;
using System.Configuration;
using System.Net;

namespace Glimpse.Net.Configuration
{
    public class IpAddressFormatValidator : ConfigurationValidatorBase
    {
        public override void Validate(object value)
        {
            if (string.IsNullOrWhiteSpace(value.ToString())) return;
     
            if (!value.ToString().IsValidIpAddress())
                throw new ArgumentException("Invalid IP address found in glimpse\\ipFilter: " + value);
        }

        public override bool CanValidate(Type type)
        {
            return type == typeof (string);
        }
    }

    public class IpAddressFormatAttribute : ConfigurationValidatorAttribute
    {
        public override ConfigurationValidatorBase ValidatorInstance { get { return new IpAddressFormatValidator(); } }
    }
}