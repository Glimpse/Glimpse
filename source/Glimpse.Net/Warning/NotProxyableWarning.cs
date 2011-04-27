namespace Glimpse.Net.Warning
{
    internal class NotProxyableWarning:Warning
    {
        public NotProxyableWarning(object obj)
        {
            Message = "Cannot create proxy of " + obj.GetType() +
                      ". Object must have a parameterless constructor cannot be sealed.";
        }
    }
}
