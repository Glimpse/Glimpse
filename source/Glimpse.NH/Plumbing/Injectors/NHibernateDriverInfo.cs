using System.Reflection;
using Glimpse.NH.Plumbing.Profiler;

namespace Glimpse.NH.Plumbing.Injectors
{
    public class NHibernateDriverInfo : INHibernateDriverInfo
    {
        private readonly object _connectionProvider;
        private readonly FieldInfo _driverField;
        private object _driver;

        public NHibernateDriverInfo(object connectionProvider, FieldInfo driverField)
        {
            _connectionProvider = connectionProvider;
            _driverField = driverField;
        }

        public bool IsWrapped()
        {
            var isWrapped = (typeof(IGlimpseProfileDbDriver).IsAssignableFrom(GetDriver().GetType()));
            return isWrapped;
        }

        public object GetDriver()
        {
            if (_driver == null)
                _driver = _driverField.GetValue(_connectionProvider);

            return _driver;
        }

        public void SetDriver(IGlimpseProfileDbDriver driver)
        {
            _driverField.SetValue(_connectionProvider, driver, BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance, null, null);
            _driver = null;
        }
    }
}