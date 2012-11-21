using System;
using System.Collections.Generic;

namespace Glimpse.Mvc.Model
{
    public class MetadataItemModel
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public MetadataContentModel ModelMetadata { get; set; }

        public IList<MetadataPropertyItemModel> PropertyMetadata { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Type Type { get; set; }
    }
}