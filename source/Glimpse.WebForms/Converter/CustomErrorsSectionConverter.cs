using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using Glimpse.WebForms.Extensibility;

namespace Glimpse.WebForms.Converter
{
    [GlimpseConverter]
    internal class CustomErrorsSectionConverter:IGlimpseConverter
    {
        public IDictionary<string, object> Serialize(object obj)
        {
            var source = obj as CustomErrorsSection;
            if (source == null) return null;

            var errors = source.Errors.Cast<CustomError>().ToDictionary(error => error.StatusCode.ToString(), error => error.Redirect);

            var result = new Dictionary<string, object>
                             {
                                 {"Mode", source.Mode.ToString()},
                                 {"RedirectMode", source.RedirectMode.ToString()},
                                 {"DefaultRedirect", source.DefaultRedirect},
                                 {"Errors", errors.Count > 0 ? errors : null },
                             };

            return result;
        }

        public IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof (CustomErrorsSection);
                yield break;
            }
        }
    }
}
