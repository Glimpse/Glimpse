using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Glimpse.WebForms.Plumbing
{
    internal class BlacklistedSafeDirectoryCatalog : ComposablePartCatalog
    {
        private AggregateCatalog AggregateCatalog { get; set; }
        public IList<Exception> Exceptions { get; private set; }
        private IEnumerable<string> TypesBlacklist { get; set; }

        public BlacklistedSafeDirectoryCatalog(string path, IEnumerable<string> typesBlacklist)
        {
            Exceptions = new List<Exception>();
            TypesBlacklist = typesBlacklist;

            var files = Directory.EnumerateFiles(GetFullPath(path), "*.dll", SearchOption.AllDirectories);

            AggregateCatalog = new AggregateCatalog();

            foreach (var file in files)
            {
                try
                {
                    var assemblyCatalog = new AssemblyCatalog(file);

                    if (assemblyCatalog.Parts.ToList().Count > 0)
                        AggregateCatalog.Catalogs.Add(assemblyCatalog);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    foreach (var exception in ex.LoaderExceptions)
                    {
                        Exceptions.Add(exception);
                    }
                }
                catch (Exception ex)
                {
                    Exceptions.Add(ex);
                }
            }
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get
            {
                return from part in AggregateCatalog.Parts
                       from exportDefinition in part.ExportDefinitions
                       where IsMatch(part)
                       select part;
            }
        }

        //3.5 Issue: Tuple
        public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
        {
            return from export in base.GetExports(definition)
                   where IsMatch(export.Item1)
                   select export;
        }

        private bool IsMatch(ComposablePartDefinition composablePartDefinition)
        {
            var result = !TypesBlacklist.Contains(composablePartDefinition.ToString());

            return result;
        }

        private static string GetFullPath(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            return Path.GetFullPath(path).ToUpperInvariant();
        }
    }
}