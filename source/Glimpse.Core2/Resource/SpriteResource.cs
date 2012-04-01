using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.ResourceResult;

namespace Glimpse.Core2.Resource
{
    public class SpriteResource:FileResource
    {
        internal const string InternalName = "sprite.png";

        public SpriteResource()
        {
            ResourceName = "Glimpse.Core2.sprite.png";
            ResourceType = "image/png";
            Name = InternalName;
        }   
    }
}