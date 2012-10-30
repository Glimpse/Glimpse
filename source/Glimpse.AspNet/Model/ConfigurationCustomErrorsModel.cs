using System.Collections.Generic;

namespace Glimpse.AspNet.Model
{
    public class ConfigurationCustomErrorsModel
    {
        public string DefaultRedirect { get; set; }

        public IEnumerable<ConfigurationCustomErrorsErrorModel> Errors { get; set; }
    }
}