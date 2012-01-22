using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Glimpse.Core2.Extensibility
{
    [ContractClass(typeof(ResourceContract))]
    public interface IResource
    {
        string Name { get; }
        string[] Parameters { get; }
        ResourceResult Execute(IDictionary<string, string> parameters);
    }

    [ContractClassFor(typeof(IResource))]
    public abstract class ResourceContract : IResource
    {
        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return default(string);
            }
        }

        public string[] Parameters
        {
            get { return default(string[]); }
        }

        public ResourceResult Execute(IDictionary<string, string> parameters)
        {
            Contract.Requires<ArgumentNullException>(parameters != null, "parameters");
            return default(ResourceResult);
        }
    }
}
