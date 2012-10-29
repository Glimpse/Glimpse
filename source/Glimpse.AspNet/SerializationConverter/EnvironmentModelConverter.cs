using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin.Assist;

namespace Glimpse.AspNet.SerializationConverter
{
    public class EnvironmentModelConverter : SerializationConverter<EnvironmentModel>
    {
        public override object Convert(EnvironmentModel obj)
        {
            var root = new TabSection("Key", "Value");
            root.AddRow().Column("Machine").Column(BuildMachineDetails(obj.Machine));
            root.AddRow().Column("Web Server").Column(BuildWebServerDetails(obj.WebServer));
            root.AddRow().Column("Framework").Column(BuildFrameworkDetails(obj.Framework));
            root.AddRow().Column("Process").Column(BuildProcessDetails(obj.Process));
            root.AddRow().Column("Timezone").Column(BuildTimeZoneDetails(obj.TimeZone));
            root.AddRow().Column("Application Assemblies").Column(BuildAssemblyDetails(obj.ApplicationAssemblies));
            root.AddRow().Column("System Assemblies").Column(BuildAssemblyDetails(obj.SystemAssemblies));

            return root.Build();
        }

        private static TabSection BuildWebServerDetails(EnvironmentWebServerModel model)
        { 
            var section = new TabSection("Type", "Integrated Pipeline");
            section.AddRow().Column(model.ServerType).Column(model.IntegratedPipeline);
            return section;
        }

        private TabSection BuildFrameworkDetails(EnvironmentFrameworkModel model)
        {
            var section = new TabSection(".NET Framework", "Debugging", "Server Culture", "Current Trust Level");
            section.AddRow().Column(model.DotnetFramework).Column(model.Debugging).Column(model.ServerCulture).Column(model.CurrentTrustLevel);
            return section;
        }

        private TabSection BuildMachineDetails(EnvironmentMachineModel model)
        {
            var section = new TabSection("Name", "Operating System", "Start Time");
            section.AddRow().Column(model.Name).Column(model.OperatingSystem).Column(model.StartTime);
            return section;
        }

        private TabSection BuildTimeZoneDetails(EnvironmentTimeZoneModel model)
        {
            var section = new TabSection("Current", "Is Daylight Saving", "UtcOffset", "UtcOffset w/DLS");
            section.AddRow().Column(model.Name).Column(model.IsDaylightSavingTime).Column(model.UtcOffset).Column(model.UtcOffsetWithDls);
            return section;
        }

        private TabSection BuildProcessDetails(EnvironmentProcessModel model)
        {
            var section = new TabSection("Worker Process", "Process ID", "Start Time");
            section.AddRow().Column(model.WorkerProcess).Column(model.ProcessId).Column(model.StartTime);
            return section;
        }

        private TabSection BuildAssemblyDetails(IEnumerable<EnvironmentAssemblyModel> model)
        { 
            var modelList = new TabSection("Name", "Version", "Culture", "From GAC", "Full Trust");
            foreach (var assemblyModel in model)
            {
                modelList.AddRow().Column(assemblyModel.Name).Column(assemblyModel.Version).Column(assemblyModel.Culture).Column(assemblyModel.FromGac).Column(assemblyModel.FullTrust);
            }

            return modelList;
        }
    }
}
