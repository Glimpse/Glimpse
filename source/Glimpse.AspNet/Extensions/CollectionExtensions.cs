using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Model;

namespace Glimpse.AspNet.Extensions
{
    public static class CollectionExtensions
    {
        public static object ToTable(this IEnumerable<RequestModel.Cookie> cookies)
        {
            var result = new List<string[]> { new[] { "Name", "Value", "Path", "Secure" } };
            result.AddRange(cookies.Select(cookie => new[] { cookie.Name, cookie.Value, cookie.Path, cookie.IsSecure.ToString() }));
            return result;
        }

        public static object ToTable(this IEnumerable<RequestModel.QueryStringParameter> parameters)
        {
            var result = new List<string[]> { new[] { "Name", "Value" } };
            result.AddRange(parameters.Select(parameter => new[]{ parameter.Key, parameter.Value }));
            return result;
        }
    }
}
