namespace Glimpse.AspNet.Model
{
    public class EnvironmentAssemblyModel
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Culture { get; set; }

        public bool FromGac { get; set; }

        public bool? FullTrust { get; set; }
    }
}