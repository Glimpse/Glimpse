using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.EF.Plumbing.Profiler;

namespace Glimpse.EF.Plumbing.Injectors
{
    public class WrapCachedMetadata : IWrapperInjectorProvider
    {
        private IGlimpseLogger Logger { get; set; }

        public WrapCachedMetadata(IGlimpseLogger logger)
        {
            Logger = logger;
        }

        public void Inject()
        { 
            var type = Type.GetType("System.Data.Entity.Database, EntityFramework", false);

            if (type != null)
            {
                var lazyInternalContextType = type.Assembly.GetType("System.Data.Entity.Internal.LazyInternalContext", false); 
                if (lazyInternalContextType != null)
                {
                    // if a query is made before we have injected our provider factory, 
                    // ef code first will cache the metadata workspace with original provider factory
                    // resulting in trying to use the original connection type. so change it
                    // to our provider factory.
                    InjectCachedMetadataWorkspaceProviderFactory(lazyInternalContextType);

                    // if a query is made before we have injected our provider factory, 
                    // ef code first will cache the model and connection key when query
                    // triggers database creation resulting in recreation of
                    // database. so change the connection key to our connection.
                    InjectCachedInitializedDatabasesKeys(lazyInternalContextType);
                }
            }

            Logger.Info("AdoPipelineInitiator: Finished trying to injecting WrapCachedMetadata");
        }


        /// <summary>
        /// Change the metadata workspace provider factory to ours for all cached models.
        /// </summary>
        /// <param name="lazyInternalContextType"></param>
        private void InjectCachedMetadataWorkspaceProviderFactory(Type lazyInternalContextType)
        {
            var locksAcquired = 0;
            // ConcurrentDictionary<Type, RetryLazy<LazyInternalContext, DbCompiledModel>>
            var dictionary = GetDictionaryAndLock(lazyInternalContextType, "CachedModels", ref locksAcquired);

            if (dictionary != null)
            {
                var idictionary = (IDictionary)dictionary; 
                foreach (DictionaryEntry i in idictionary)
                {
                    ChangeProviderFactory(i);
                }

                ReleaseDictionaryLock(dictionary, locksAcquired);
            }
        }

        private void ChangeProviderFactory(DictionaryEntry i)
        {
            var valueField = i.Value.GetType().GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance); 
            if (valueField != null)
            {
                var value = valueField.GetValue(i.Value); 
                var workspaceField = value.GetType().GetField("_workspace", BindingFlags.NonPublic | BindingFlags.Instance);
                if (workspaceField != null)
                {
                    var workspace = workspaceField.GetValue(value);
                    var metadataWorkspaceField = workspace.GetType().GetField("_metadataWorkspace", BindingFlags.NonPublic | BindingFlags.Instance); 
                    if (metadataWorkspaceField != null)
                    {
                        var metadataWorkspace = metadataWorkspaceField.GetValue(workspace);
                        var itemsSSpaceField = metadataWorkspace.GetType().GetField("_itemsSSpace", BindingFlags.NonPublic | BindingFlags.Instance);

                        if (itemsSSpaceField != null)
                        {
                            var itemsSSpace = itemsSSpaceField.GetValue(metadataWorkspace);
                            var providerFactoryField = itemsSSpace.GetType().GetField("_providerFactory", BindingFlags.NonPublic | BindingFlags.Instance);

                            if (providerFactoryField != null)
                            {
                                var providerFactory = providerFactoryField.GetValue(itemsSSpace);

                                providerFactoryField.SetValue(itemsSSpace, Activator.CreateInstance(GetProxyTypeForProvider(providerFactory.GetType())));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Change the connection key to ours for all initialized databases.
        /// </summary>
        /// <param name="lazyInternalContextType"></param>
        private void InjectCachedInitializedDatabasesKeys(Type lazyInternalContextType)
        {
            var locksAcquired = 0; 

            var dictionary = GetDictionaryAndLock(lazyInternalContextType, "InitializedDatabases", ref locksAcquired); 
            if (dictionary != null)
            {
                var dbCompiledModelType = lazyInternalContextType.Assembly.GetType("System.Data.Entity.Infrastructure.DbCompiledModel"); 
                if (dbCompiledModelType != null)
                {
                    var tupleType = typeof(Tuple<,>).MakeGenericType(dbCompiledModelType, typeof(string));  
                    var idictionary = (IDictionary)dictionary;

                    var keys = new List<object>(); 
                    foreach (DictionaryEntry i in idictionary) 
                        keys.Add(i.Key); 

                    string connectionTypeName = typeof(GlimpseProfileDbConnection).FullName;

                    foreach (var i in keys)
                    {
                        var connectionKey = (string)tupleType.GetProperty("Item2").GetValue(i, null);

                        var index = connectionKey.IndexOf(';');
                        if (index != -1)
                        {
                            connectionKey = connectionTypeName + connectionKey.Remove(0, index);

                            var v = idictionary[i];
                            var dbCompiledModel = tupleType.GetProperty("Item1").GetValue(i, null);

                            idictionary.Remove(i);
                            idictionary.Add(Activator.CreateInstance(tupleType, dbCompiledModel, connectionKey), v);
                        }
                    }
                }

                ReleaseDictionaryLock(dictionary, locksAcquired);
            }
        }


        private object GetDictionaryAndLock(Type type, string fieldName, ref int locksAcquired)
        {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static); 
            if (field != null)
            {
                var dictionary = field.GetValue(null); 
                if (dictionary != null)
                {
                    var acquireAllLocksMethod = dictionary.GetType().GetMethod("AcquireAllLocks", BindingFlags.NonPublic | BindingFlags.Instance); 
                    if (acquireAllLocksMethod != null)
                    {
                        var parameters = new object[1];
                        acquireAllLocksMethod.Invoke(dictionary, parameters);

                        locksAcquired = (int)parameters[0];

                        return dictionary;
                    }
                    return null;
                }
            }

            return null;
        }

        private void ReleaseDictionaryLock(object dictionary, int locksAcquired)
        {
            var releaseLocksMethod = dictionary.GetType().GetMethod("ReleaseLocks", BindingFlags.NonPublic | BindingFlags.Instance); 
            if (releaseLocksMethod != null) 
                releaseLocksMethod.Invoke(dictionary, new object[] { 0, locksAcquired }); 
        }

        private static Type GetProxyTypeForProvider(Type factoryType)
        {
            return typeof(GlimpseProfileDbProviderFactory<>).MakeGenericType(factoryType);
        }
    }
}
