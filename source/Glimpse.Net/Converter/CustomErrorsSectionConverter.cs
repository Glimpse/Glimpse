using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Glimpse.Protocol;

namespace Glimpse.Net.Converter
{
    [GlimpseConverter]
    internal class CustomErrorsSectionConverter:IGlimpseConverter
    {
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
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

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof (CustomErrorsSection);
                yield break;
            }
        }
    }
}
