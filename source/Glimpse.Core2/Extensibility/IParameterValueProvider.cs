using System.Collections.Generic;

namespace Glimpse.Core2.Extensibility
{
    public interface IParameterValueProvider
    {
        void OverrideParameterValues(IDictionary<string,string> defaults);
    }
}