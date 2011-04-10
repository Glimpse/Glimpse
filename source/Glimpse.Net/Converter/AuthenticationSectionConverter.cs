using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Glimpse.Protocol;

namespace Glimpse.Net.Converter
{
    [GlimpseConverter]
    public class AuthenticationSectionConverter:IGlimpseConverter
    {
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var source = obj as AuthenticationSection;
            if (source == null) return null;


            var result = new Dictionary<string, object>
                             {
                                 {"Mode", source.Mode.ToString()},
                             };

            var forms = source.Forms;
            if (source.Forms != null)
            {
                result.Add("Cookieless", forms.Cookieless.ToString());
                result.Add("DefaultUrl", forms.DefaultUrl);
                result.Add("Domain", forms.Domain);
                result.Add("EnableCrossAppRedirects", forms.EnableCrossAppRedirects.ToString());
                result.Add("LoginUrl", forms.LoginUrl);
                result.Add("Name", forms.Name);
                result.Add("Path", forms.Path);
                result.Add("Protection", forms.Protection.ToString());
                result.Add("RequireSSL", forms.RequireSSL.ToString());
                result.Add("SlidingExpiration", forms.SlidingExpiration.ToString());
                result.Add("TicketCompatibilityMode", forms.TicketCompatibilityMode.ToString());
                result.Add("Timeout", forms.Timeout.ToString());
            }

            return result;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(AuthenticationSection);
                yield break;
            }
        }
    }
}
