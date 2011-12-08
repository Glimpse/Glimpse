using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Glimpse.Test.NH.DataContext
{
    public interface INHibernateContext : IDisposable
    {
    }

    public class NHibernateContext : INHibernateContext
    {
        private readonly NHibernateVersion _version;
        private AppDomain _appDomain;

        public NHibernateContext(NHibernateVersion version)
        {
            _version = version;

            LoadAssembliesForVersion();
            BuildSessionFactoryForVersion();
        }

        public void Dispose()
        {
            UnloadAssembliesForVersion();
        }

        private void LoadAssembliesForVersion()
        {
            _appDomain = AppDomain.CreateDomain("NHibernateContext");

            var nhibernatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataContext\{0}", _version));
            var assembliesToLoad = Directory.GetFiles(nhibernatePath, "*.dll");
            foreach (var assemblyFile in assembliesToLoad)
            {
                var assemblyFileName = new FileInfo(assemblyFile).Name;
                var assemblyFileToCreate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyFileName);
                File.Copy(assemblyFile, assemblyFileToCreate, true);
                _appDomain.Load(AssemblyName.GetAssemblyName(assemblyFileToCreate));
            }
        }

        private void BuildSessionFactoryForVersion()
        {
            var nhibernateAssembly = _appDomain.GetAssemblies().Single(a => a.GetName().Name == "NHibernate");
            var configurationType = nhibernateAssembly.GetType("NHibernate.Cfg.Configuration", true, true);
            var configuration = configurationType.GetConstructor(new Type[] { }).Invoke(null);
            var configureMethodInfo = configurationType.GetMethod("Configure", new[] { typeof(string) });
            var buildSessionFactoryMethodInfo = configurationType.GetMethod("BuildSessionFactory", new Type[] { });
            configuration = configureMethodInfo.Invoke(configuration, new[] { Path.Combine(AppDomain.CurrentDomain.BaseDirectory + string.Format(@"\DataContext\{0}\", _version), "hibernate.cfg.xml") });

            var sessionFactory = buildSessionFactoryMethodInfo.Invoke(configuration, null);
            Trace.WriteLine("Loaded: " + sessionFactory);
        }

        private void UnloadAssembliesForVersion()
        {
            AppDomain.Unload(_appDomain);

            var nhibernatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataContext\{0}", _version));
            var assembliesToLoad = Directory.GetFiles(nhibernatePath, "*.dll");
            foreach (var assemblyFile in assembliesToLoad)
            {
                var assemblyFileName = new FileInfo(assemblyFile).Name;
                var assemblyFileToDelete = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyFileName);
                File.Delete(assemblyFileToDelete);
            }
        }
    }
}