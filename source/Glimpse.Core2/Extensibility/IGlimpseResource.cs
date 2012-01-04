using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Glimpse.Core2.Extensibility
{
    [ContractClass(typeof(GlimpseResourceContract))]
    public interface IGlimpseResource
    {
        string Name { get; }
        ResourceResult Execute(IDictionary<string, string> parameters);
    }

    [ContractClassFor(typeof(IGlimpseResource))]
    public abstract class GlimpseResourceContract : IGlimpseResource
    {
        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return default(string);
            }
        }

        public ResourceResult Execute(IDictionary<string, string> parameters)
        {
            Contract.Requires<ArgumentNullException>(parameters != null, "parameters");
            return default(ResourceResult);
        }
    }
}
