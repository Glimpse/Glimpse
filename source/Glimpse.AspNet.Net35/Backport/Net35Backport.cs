namespace Glimpse.AspNet.Net35.Backport
{
    public static class Net35Backport
    {
        public static bool IsNullOrWhiteSpace(string value)
        {
            if (value == null) return true;

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i])) return false;
            }

            return true;
        }
    }
}