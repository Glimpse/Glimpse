using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Resource
{
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

        public IResourceResult Execute(IResourceContext context)
        {
            Contract.Requires<ArgumentNullException>(context != null, "context");

            Guid requestId;

            if (!Guid.TryParse(context.Parameters[ResourceParameterKey.RequestId], out requestId))
                return new StatusCodeResourceResult(404);
            
            var data = context.PersistanceStore.GetByRequestId(requestId);

            if(data == null)
                return new StatusCodeResourceResult(404);

            var cacheDuration = 150*24*60*60; //150 days * hours * minutes * seconds
            return new JsonResourceResult(data, @"application/json", cacheDuration, CacheSetting.Private);
        }
    }
}