namespace Glimpse.Mvc3.Warning
{
    internal class NotProxyableWarning:WebForms.Warning.Warning
    {
        public NotProxyableWarning(object obj)
        {
            Message = "Cannot create proxy of " + obj.GetType() +
                      ". Object must have a parameterless constructor, cannot be sealed, and cannot already be a proxy object.";
        }
    }
}
