using System.Collections.Generic;
using System.Web.Mvc;

// ReSharper disable CheckNamespace
namespace Glimpse.Mvc3.Backport
{
    // This interface is only to trick the compiler into allowing MVC3 to have IEnumerableValueProviders. There should be no implementations of this interface.
    public interface IEnumerableValueProvider : IValueProvider
    {
        IDictionary<string, string> GetKeysFromPrefix(string prefix);
    }
}
// ReSharper restore CheckNamespace