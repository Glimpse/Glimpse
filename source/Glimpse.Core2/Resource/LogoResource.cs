using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.ResourceResult;

namespace Glimpse.Core2.Resource
{
    public class Logo:FileResource
    {
        internal const string InternalName = "logo.png";

        public Logo()
        {
            ResourceName = "Glimpse.Core2.logo.png";
            ResourceType = "image/png";
            Name = InternalName;
        }  
    }
}