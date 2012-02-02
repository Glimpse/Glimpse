using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Glimpse.NH.Plumbing.Injectors
{
    public class NHibernateInfoProvider : INHibernateInfoProvider
    {
        public Assembly GetNhibernateAssembly()
        {
            var sessionFactoryObjectFactoryType = Type.GetType("NHibernate.Impl.SessionFactoryObjectFactory, NHibernate", false, true);
            if (sessionFactoryObjectFactoryType == null)
                return null;

            return sessionFactoryObjectFactoryType.Assembly;
        }

        public IEnumerable<INHibernateDriverInfo> GetNHibernateDriverInfos()
        {
            var sessionFactoryObjectFactoryType = Type.GetType("NHibernate.Impl.SessionFactoryObjectFactory, NHibernate", false, true);
            if (sessionFactoryObjectFactoryType == null)
                return null;

            var intancesField = sessionFactoryObjectFactoryType.GetField("Instances", BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static);
            if (intancesField == null)
                return null;

            var instances = (IDictionary)intancesField.GetValue(null);
            if (instances == null)
                return null;

            var sessionFactoryImplType = Type.GetType("NHibernate.Impl.SessionFactoryImpl, NHibernate", false, true);
            if (sessionFactoryImplType == null)
                return null;

            var settingsField = sessionFactoryImplType.GetField("Settings", BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance);
            if (settingsField == null)
                return null;

            var settingsType = Type.GetType("NHibernate.Cfg.Settings, NHibernate", false, true);
            if (settingsType == null)
                return null;

            var connectionProviderField = settingsType.GetProperty("ConnectionProvider", BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            if (connectionProviderField == null)
                return null;

            var connectionProviderType = Type.GetType("NHibernate.Connection.ConnectionProvider, NHibernate", false, true);
            if (connectionProviderType == null)
                return null;

            var driverField = connectionProviderType.GetField("Driver", BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance);
            if (driverField == null)
                return null;

            var driverInfos = new List<INHibernateDriverInfo>();
            foreach (DictionaryEntry instance in instances)
            {
                // Get the driver to wrap
                var settings = settingsField.GetValue(instance.Value);
                var connectionProvider = connectionProviderField.GetValue(settings, BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance, null, null, null);
                var driverInfo = new NHibernateDriverInfo(connectionProvider, driverField);
                driverInfos.Add(driverInfo);
            }

            return driverInfos;
        }
    }
}