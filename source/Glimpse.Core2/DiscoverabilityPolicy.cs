using System;
using System.Collections.Generic;

namespace Glimpse.Core2
{
    public abstract class DiscoverabilityPolicy
    {
        //TODO Remove MEF dependency to support .NET35 and eliminate the need to reference System.ComponentModel.Composition when extending Glimpse
        protected DiscoverabilityPolicy()
        {
            IgnoredTypes = new List<string>();
            AutoDiscover = true;
        }

        public void IgnoreType(Type type)
        {
            IgnoreType(type.AssemblyQualifiedName);
        }

        public void IgnoreType(string name)
        {
            IgnoredTypes.Add(name);
        }

        private string path;
        public string Path
        {
            get { return path ?? (path = AppDomain.CurrentDomain.BaseDirectory); }
            set { path = System.IO.Path.IsPathRooted(value) ? value : System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value); }
        }

        protected IList<string> IgnoredTypes { get; set; }
        public bool AutoDiscover { get; set; }
        public abstract void Discover();
    }
}