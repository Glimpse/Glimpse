using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    public interface IParameterValueProvider
    {
        void OverrideParameterValues(IDictionary<string, string> defaults);
    }
}