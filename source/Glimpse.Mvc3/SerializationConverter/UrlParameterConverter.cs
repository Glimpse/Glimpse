using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.SerializationConverter
{
    /// <summary>
    /// Intercepts the serialization of UrlParameters.
    /// </summary>
    /// <remarks>
    /// In particular, we want to intercept UrlParameters being serialized for display
    /// in the Route data in the Routes tab, and display the special MVC "Optional"
    /// parameter value differently.
    /// </remarks>
    class UrlParameterConverter : SerializationConverter<UrlParameter>
    {
        public override object Convert(UrlParameter obj)
        {
            if (obj == UrlParameter.Optional)
                return "_Optional_";
            return obj;
        }
    }
}
