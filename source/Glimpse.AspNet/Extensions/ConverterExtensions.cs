using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Model;

namespace Glimpse.AspNet.Extensions
{
    public static class ConverterExtensions
    {
        public static object ToTable(this IEnumerable<RequestModel.Cookie> cookies)
        {
            if (cookies.Count() == 0)
            {
                return null;
            }

            var result = new List<string[]> { new[] { "Name", "Value", "Path", "Secure" } };
            result.AddRange(cookies.Select(cookie => new[] { cookie.Name, cookie.Value, cookie.Path, cookie.IsSecure.ToString() }));
            return result;
        }

        public static object ToTable(this IEnumerable<RequestModel.QueryStringParameter> parameters)
        {
            if (parameters.Count() == 0)
            {
                return null;
            }

            var result = new List<string[]> { new[] { "Name", "Value" } };
            result.AddRange(parameters.Select(parameter => new[] { parameter.Key, parameter.Value }));
            return result;
        }

        public static object OrNull(this Uri uri)
        {
            if (uri == null)
            {
                return null;
            }

            return uri.ToString();
        }
    }
}
