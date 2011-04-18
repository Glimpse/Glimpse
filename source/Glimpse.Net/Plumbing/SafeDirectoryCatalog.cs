using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Glimpse.Net.Plumbing
{
    internal class SafeDirectoryCatalog : ComposablePartCatalog
    {
        private readonly AggregateCatalog aggregateCatalog;
        private readonly IList<Exception> exceptions;

        public SafeDirectoryCatalog(string path)
        {
            exceptions = new List<Exception>();
            var files = Directory.EnumerateFiles(GetFullPath(path), "*.dll", SearchOption.AllDirectories);

            aggregateCatalog = new AggregateCatalog();

            foreach (var file in files)
            {
                try
                {
                    var assemblyCatalog = new AssemblyCatalog(file);

                    if (assemblyCatalog.Parts.ToList().Count > 0)
                        aggregateCatalog.Catalogs.Add(assemblyCatalog);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    foreach (var exception in ex.LoaderExceptions)
                    {
                        exceptions.Add(exception);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
        }

        public IList<Exception> Exceptions
        {
            get { return exceptions; }
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return aggregateCatalog.Parts; }
        }

        private static string GetFullPath(string path)
        {
            if (!Path.IsPathRooted(path) && AppDomain.CurrentDomain.BaseDirectory != null)
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            return Path.GetFullPath(path).ToUpperInvariant();
        }
    }
}
