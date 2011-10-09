using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Glimpse.Core2
{
    internal class FilteredSafeDirectoryCatalog:ComposablePartCatalog
    {
        private AggregateCatalog AggregateCatalog { get; set; }
        private IEnumerable<string> IgnoredTypes { get; set; }

        public FilteredSafeDirectoryCatalog(string directoryPath, IEnumerable<string> ignoredTypes = null)
        {
            AggregateCatalog = new AggregateCatalog();
            IgnoredTypes = ignoredTypes ?? Enumerable.Empty<string>();

            var files = Directory.EnumerateFiles(directoryPath, "*.dll", SearchOption.AllDirectories);


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
                        //TODO: Log exception
                    }
                }
                catch (Exception ex)
                {
                    //TODO: Log exception
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

        public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
        {
            return from export in base.GetExports(definition)
                   where IsMatch(export.Item1)
                   select export;
        }

        private bool IsMatch(ComposablePartDefinition composablePartDefinition)
        {
            return !(from type in IgnoredTypes
                          where type.Equals(composablePartDefinition.ToString(), StringComparison.InvariantCultureIgnoreCase)
                          select type).Any();
        }
    }
}
