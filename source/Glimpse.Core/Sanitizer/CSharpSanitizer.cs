using System;
using System.Text.RegularExpressions;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Sanitizer
{
    public class CSharpSanitizer:IGlimpseSanitizer
    {
        public string Sanitize(string json)
        {
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
