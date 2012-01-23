using System.Collections.Generic;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Resource
{
    [Resource]
    public class Client:IResource
    {
        internal const string InternalName = "glimpse.js";

        public string Name
        {
            get { return InternalName; }
        }

        public IEnumerable<string> ParameterKeys
        {
            get { return new[]{ResourceParameterKey.RequestId, ResourceParameterKey.VersionNumber }; }
        }

        public ResourceResult Execute(IDictionary<string, string> parameters)
        {
            //TODO: Return the embedded glimpseClient.js file
            throw new System.NotImplementedException();
        }
    }
}