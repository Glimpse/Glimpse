using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.AspNet.SerializationConverter
{
    public class EnvironmentModelConverter : SerializationConverter<EnvironmentModel>
    {
        public override object Convert(EnvironmentModel obj)
        {
            var root = new TabObject();
            root.AddRow().Key("Machine").Value(BuildMachineDetails(obj.Machine));
            root.AddRow().Key("Web Server").Value(BuildWebServerDetails(obj.WebServer));
            root.AddRow().Key("Framework").Value(BuildFrameworkDetails(obj.Framework));
            root.AddRow().Key("Process").Value(BuildProcessDetails(obj.Process));
            root.AddRow().Key("Timezone").Value(BuildTimeZoneDetails(obj.TimeZone));
            root.AddRow().Key("Application Assemblies").Value(BuildAssemblyDetails(obj.ApplicationAssemblies));
            root.AddRow().Key("System Assemblies").Value(BuildAssemblyDetails(obj.SystemAssemblies));

            return root.Build();
        }

        private static TabObject BuildWebServerDetails(EnvironmentWebServerModel model)
        {
            var section = new TabObject();
            section.AddRow().Key("Type").Value(model.ServerType);
            section.AddRow().Key("Integrated Pipeline").Value(model.IntegratedPipeline);
            return section;
        }

        private TabObject BuildFrameworkDetails(EnvironmentFrameworkModel model)
        {
            var section = new TabObject();
            section.AddRow().Key(".NET Framework").Value(model.DotnetFramework);
            section.AddRow().Key("Debugging").Value(model.Debugging);
            section.AddRow().Key("Server Culture").Value(model.ServerCulture);
            section.AddRow().Key("Current Trust Level").Value(model.CurrentTrustLevel); 
            return section;
        }

        private TabObject BuildMachineDetails(EnvironmentMachineModel model)
        {
            var section = new TabObject();
            section.AddRow().Key("Name").Value(model.Name);
            section.AddRow().Key("Operating System").Value(model.OperatingSystem);
            section.AddRow().Key("Start Time").Value(model.StartTime); 
            return section;
        }

        private TabObject BuildTimeZoneDetails(EnvironmentTimeZoneModel model)
        {
            var section = new TabObject();
            section.AddRow().Key("Current").Value(model.Name);
            section.AddRow().Key("Is Daylight Saving").Value(model.IsDaylightSavingTime);
            section.AddRow().Key("UtcOffset").Value(model.UtcOffset);
            section.AddRow().Key("UtcOffset w/DLS").Value(model.UtcOffsetWithDls); 
            return section;
        }

        private TabObject BuildProcessDetails(EnvironmentProcessModel model)
        {
            var section = new TabObject();
            section.AddRow().Key("Worker Process").Value(model.WorkerProcess);
            section.AddRow().Key("Process ID").Value(model.ProcessId);
            section.AddRow().Key("Start Time").Value(model.StartTime); 
            return section;
        }

        private TabSection BuildAssemblyDetails(IEnumerable<EnvironmentAssemblyModel> model)
        { 
            var modelList = new TabSection("Name", "Version", "Version Info", "Culture", "From GAC", "Full Trust");
            foreach (var assemblyModel in model)
            {
                modelList.AddRow().Column(assemblyModel.Name).Column(assemblyModel.Version).Column(assemblyModel.VersionInfo).Column(assemblyModel.Culture).Column(assemblyModel.FromGac).Column(assemblyModel.FullTrust);
            }

            return modelList;
        }
    }
}
