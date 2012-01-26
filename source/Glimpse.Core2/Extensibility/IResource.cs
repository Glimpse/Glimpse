using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Glimpse.Core2.Extensibility
{
    [ContractClass(typeof(ResourceContract))]
    public interface IResource
    {
        string Name { get; }
        IEnumerable<string> ParameterKeys { get; }
        IResourceResult Execute(IDictionary<string, string> parameters);
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

        public IEnumerable<string> ParameterKeys
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
                return default(string[]);
            }
        }

        public IResourceResult Execute(IDictionary<string, string> parameters)
        {
            Contract.Requires<ArgumentNullException>(parameters != null, "parameters");
            return default(IResourceResult);
        }
    }
}
