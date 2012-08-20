using System.Collections.Generic;
using Tavis.UriTemplates;

namespace Glimpse.Core.Extensions
{
    public static class UriTemplateExtensions
    {
        public static UriTemplate SetParameters(this UriTemplate template, IEnumerable<KeyValuePair<string, string>> nameValues)
         {
             if (nameValues == null)
                 return template;

             foreach (var pair in nameValues)
             {
                template.SetParameter(pair.Key, pair.Value);
             }

            return template;
         }
    }
}