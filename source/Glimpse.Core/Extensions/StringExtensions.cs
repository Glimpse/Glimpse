using System;
using System.Globalization; 

namespace Glimpse.Core.Extensions
{
    public static class StringExtensions
    {
        public static string TakeFirstChar(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                input = input[0].ToString(CultureInfo.InvariantCulture);
            }

            return input;
        }
    }
}