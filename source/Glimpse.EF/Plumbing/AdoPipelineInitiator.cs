using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.EF.Plumbing.Injectors;
using Microsoft.CSharp;

namespace Glimpse.EF.Plumbing
{
    internal class AdoPipelineInitiator
    {
        private IGlimpseLogger Logger { get; set; }
        private IList<IWrapperInjectorProvider> Providers { get; set; }

        internal AdoPipelineInitiator(IGlimpseLogger logger)
        {
            Logger = logger; 

            //Note order of execution is important.
            Providers = new List<IWrapperInjectorProvider>
                            {
                                new WrapDbProviderFactories(Logger),
                                new WrapDbConnectionFactories(Logger),
                                new WrapCachedMetadata(Logger)
                            };

        }

        internal void Initiate()
        {
            Logger.Info("AdoPipelineInitiator: Starting");

            foreach (var provider in Providers) 
                provider.Inject(); 

            Logger.Info("AdoPipelineInitiator: Finished");
        } 
    }
}