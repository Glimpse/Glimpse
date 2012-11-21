using System.Web.Mvc;

// ReSharper disable CheckNamespace
namespace Glimpse.Mvc2.Backport
{
    // This interface is only to trick the compiler into allowing MVC2 to have IUnvalidatedValueProviders. There should be no implementations of this interface.
    public interface IUnvalidatedValueProvider : IValueProvider
    {
        ValueProviderResult GetValue(string key, bool skipValidation);
    }
}
// ReSharper restore CheckNamespace