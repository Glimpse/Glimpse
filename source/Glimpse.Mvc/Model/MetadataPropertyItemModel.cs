using System;

namespace Glimpse.Mvc.Model
{
    public class MetadataPropertyItemModel
    {
        public string Name { get; set; }

        public MetadataContentModel Metadata { get; set; }

        public Type Type { get; set; }

        public string DisplayName { get; set; }
    }
}