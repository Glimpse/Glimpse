using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpseBinderMetadata
    {
        public List<GlimpseModelBoundProperties> Properties { get; set; }

        public GlimpseBinderMetadata()
        {
            Properties = new List<GlimpseModelBoundProperties>();
        }

        public string MemberOf { get; set; }

        private GlimpseModelBoundProperties currentProperty;
        public GlimpseModelBoundProperties CurrentProperty
        {
            get { return currentProperty ?? (currentProperty = new GlimpseModelBoundProperties{MemberOf = MemberOf, Name=""}); }
            set
            {
                value.MemberOf = MemberOf;
                currentProperty = value;
                Properties.Add(value);
            }
        }
    }

    internal class GlimpseModelBoundProperties
    {
        public GlimpseModelBoundProperties()
        {
            NotFoundIn = new HashSet<IValueProvider>();
        }

        public Type ModelBinderType { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public HashSet<IValueProvider> NotFoundIn { get; set; }
        public IValueProvider FoundIn { get; set; }
        public string AttemptedValue { get; set; }
        public object RawValue { get; set; }
        public string MemberOf { get; set; }
        public CultureInfo Culture { get; set; }
    }
}