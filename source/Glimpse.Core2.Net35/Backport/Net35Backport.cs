using System;

namespace Glimpse.Core2.Backport
{
    public static class Net35Backport
    {
        public static bool HasFlag<T>(this Enum type, T flag)
        {
            try
            {
                return (((int) (object) type & (int) (object) flag) == (int) (object) flag);
            }
            catch
            {
                return false;
            }
        }

        public static bool TryParseGuid(string input, out Guid output)
        {
            try
            {
                output = new Guid(input);
                return true;
            }
            catch
            {
                output = default(Guid);
                return false;
            }
        }

        public static bool TryParseEnum<T>(string input, bool ignoreCase, out T result)
        {
            try
            {
                result = (T) Enum.Parse(typeof (T), input, ignoreCase);
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }
    }
}