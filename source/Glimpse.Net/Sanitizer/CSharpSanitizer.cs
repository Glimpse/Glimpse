using System;
using System.Text.RegularExpressions;
using Glimpse.WebForms.Extensibility;

namespace Glimpse.Net.Sanitizer
{
    public class CSharpSanitizer:IGlimpseSanitizer
    {
        public string Sanitize(string json)
        {
            json = Regex.Replace(json, @"(?<=`[0-9]+\[.+)\](?=""| )", @">"); //Replace '>' for generics
            json = Regex.Replace(json, @"`[0-9]\[", @"<"); //Replace '<' for generics
            json = Regex.Replace(json, @"(?<=System\.Nullable<.+)>(?= )", @"?"); //Add '?' for nullable types
            json = Regex.Replace(json, @"System.Nullable<", @""); //Add '?' for nullable types
            json = json.Replace("System.Boolean", "bool"); //Convert CLR type names to c# keywords
            json = json.Replace("System.Byte", "byte");
            json = json.Replace("System.SByte", "sbyte");
            json = json.Replace("System.Char", "char");
            json = json.Replace("System.Decimal", "decimal");
            json = json.Replace("System.Double", "double");
            json = json.Replace("System.Single", "float");
            json = json.Replace("System.Int32", "int");
            json = json.Replace("System.UInt32", "uint");
            json = json.Replace("System.Int64", "long");
            json = json.Replace("System.UInt64", "ulong");
            json = json.Replace("System.Object", "object");
            json = json.Replace("System.Int16", "short");
            json = json.Replace("System.UInt16", "ushort");
            json = json.Replace("System.String", "string");
            json = json.Replace("-2147483648", "\"int.MinValue\""); //Convert min ints
            json = json.Replace("2147483647", "\"int.MaxValue\""); //Convert max ints

            //Convert /Date(15434532)/ format to readable date
            var matches = Regex.Matches(json, @"\\/Date\((?<ticks>(\d+))\)\\/");

            long ticks;
            var epoch = new DateTime(1970, 1, 1);

            foreach (Match match in matches)
            {
                if (long.TryParse(match.Groups["ticks"].Value, out ticks))
                {
                    var dateTime = epoch.AddMilliseconds(ticks).ToLocalTime();
                    json = json.Replace(match.Value, dateTime.ToString());
                }
            }

            return json;
        }
    }
}
