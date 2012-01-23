using System.Collections.Generic;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Resource
{
    [Resource]
    public class Data:IResource
    {
        internal const string InternalName = "data.js";

        public string Name
        {
            get { return InternalName; }
        }

        public IEnumerable<string> ParameterKeys
        {
            get { return new[] {ResourceParameterKey.RequestId, ResourceParameterKey.VersionNumber}; }
        }

        public ResourceResult Execute(IDictionary<string, string> parameters)
        {
            //TODO: Return data for one glimpse request
            throw new System.NotImplementedException();
        }
    }
}