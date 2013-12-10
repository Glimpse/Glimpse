using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using Glimpse.AspNet.Model;

namespace Glimpse.AspNet.Tab
{
    /// <summary>
    /// Creates instances of <see cref="ConfigurationConnectionStringModel"/> based on a <see cref="ConnectionStringSettings"/> instance
    /// </summary>
    public static class ConfigurationConnectionStringModelFactory
    {
        private const string ObfuscatedValue = "########";

        private static readonly IDictionary<string, string> ObfuscatedConnectionStringValues;

        static ConfigurationConnectionStringModelFactory()
        {
            ObfuscatedConnectionStringValues =
                new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
                {
                    {"password", ObfuscatedValue},
                    {"pwd", ObfuscatedValue},
                    {"accountkey", ObfuscatedValue}
                };

            var additionalKeysToObfuscate = ConfigurationManager.AppSettings["Glimpse:ConfigurationTab:ConnectionStrings;KeysToObfuscate"];
            if (!string.IsNullOrEmpty(additionalKeysToObfuscate))
            {
                var additionalKeys = additionalKeysToObfuscate.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var additionalKey in additionalKeys)
                {
                    if (!ObfuscatedConnectionStringValues.ContainsKey(additionalKey))
                    {
                        ObfuscatedConnectionStringValues.Add(additionalKey, ObfuscatedValue);
                    }
                }
            }
        }

        public static ConfigurationConnectionStringModel Create(ConnectionStringSettings connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            var resultItem = new ConfigurationConnectionStringModel
            {
                Key = connectionString.Name,
                Raw = connectionString.ConnectionString,
                ProviderName = connectionString.ProviderName
            };

            if (!string.IsNullOrEmpty(connectionString.ConnectionString))
            {
                resultItem.Details = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

                try
                {
                    IDictionary<string, object> connectionStringKeyValuePairs =
                        DetermineConnectionStringKeyValuePairs(connectionString, resultItem.ProviderName, resultItem) ??
                        DetermineConnectionStringKeyValuePairs(connectionString);

                    AddConnectionStringDetailsToResult(connectionStringKeyValuePairs, resultItem);
                }
                catch (Exception e)
                {
                    resultItem.Details.Add("GENERAL FAILURE", e.Message);
                }
            }

            return resultItem;
        }

        private static IDictionary<string, object> DetermineConnectionStringKeyValuePairs(
            ConnectionStringSettings connectionString,
            string providerName,
            ConfigurationConnectionStringModel resultItem)
        {
            IDictionary<string, object> connectionStringKeyValuePairs = null;

            try
            {
                if (string.IsNullOrEmpty(providerName))
                {
                    resultItem.Details.Add("WARNING", "ProviderName is empty, therefore assuming ProviderName=System.Data.SqlClient");
                    providerName = "System.Data.SqlClient";
                }

                var connectionFactory = DbProviderFactories.GetFactory(providerName);
                var connectionStringBuilder = connectionFactory.CreateConnectionStringBuilder();
                if (connectionStringBuilder != null)
                {
                    try
                    {
                        connectionStringBuilder.ConnectionString = connectionString.ConnectionString;
                        var keys = connectionStringBuilder.Keys;
                        if (keys != null)
                        {
                            connectionStringKeyValuePairs = new Dictionary<string, object>();
                            foreach (string key in keys)
                            {
                                connectionStringKeyValuePairs.Add(key, connectionStringBuilder[key]);
                            }
                        }
                    }
                    catch (ArgumentException argumentException)
                    {
                        resultItem.Details.Add("ERROR", "Connection string is invalid for ProviderName=" + providerName + " : " + argumentException.Message);
                    }
                }
                else
                {
                    resultItem.Details.Add("ERROR", "Connection string builder could not be created");
                }
            }
            catch (Exception exception)
            {
                resultItem.Details.Add("FATAL", exception.Message);
            }

            return connectionStringKeyValuePairs;
        }

        private static IDictionary<string, object> DetermineConnectionStringKeyValuePairs(ConnectionStringSettings connectionString)
        {
            IDictionary<string, object> connectionStringKeyValues = new Dictionary<string, object>();
            var keyValues = connectionString.ConnectionString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var keyValue in keyValues)
            {
                var keyAndValue = keyValue.Split(new[] { "=" }, StringSplitOptions.None);
                connectionStringKeyValues.Add(keyAndValue[0], keyAndValue[1]);
            }

            return connectionStringKeyValues;
        }

        private static void AddConnectionStringDetailsToResult(
            IEnumerable<KeyValuePair<string, object>> connectionStringKeyValuePairs,
            ConfigurationConnectionStringModel resultItem)
        {
            foreach (var connectionStringKeyValue in connectionStringKeyValuePairs)
            {
                resultItem.Details.Add(connectionStringKeyValue.Key, connectionStringKeyValue.Value);

                if (connectionStringKeyValue.Value == null)
                {
                    continue;
                }

                var currentValue = connectionStringKeyValue.Value.ToString();
                if (string.IsNullOrEmpty(currentValue))
                {
                    // no need to replace an empty value
                    continue;
                }

                string obfuscatedValue;
                if (ObfuscatedConnectionStringValues.TryGetValue(connectionStringKeyValue.Key, out obfuscatedValue))
                {
                    // this key must be obfuscated
                    resultItem.Details[connectionStringKeyValue.Key] = obfuscatedValue;
                    resultItem.Raw = resultItem.Raw.Replace(currentValue, obfuscatedValue);
                }
            }
        }
    }
}
