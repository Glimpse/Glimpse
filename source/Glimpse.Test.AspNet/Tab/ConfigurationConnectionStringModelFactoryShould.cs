using System.Configuration;
using Glimpse.AspNet.Tab;
using Xunit;

namespace Glimpse.Test.AspNet.Tab
{
    public class ConfigurationConnectionStringModelFactoryShould
    {
        [Fact]
        public void HaveParsedTypicalConnectionString()
        {
            CheckHaveParsedTypicalConnectionString("System.Data.SqlClient");
        }

        [Fact]
        public void HaveParsedTypicalConnectionStringWithoutExplicitProviderName()
        {
            CheckHaveParsedTypicalConnectionString(null);
        }

        private static void CheckHaveParsedTypicalConnectionString(string providerName)
        {
            const string connectionString = "Data Source=MyServer;Initial Catalog=MyDatabase;User Id=MyUsername;Password=MyPassword";
            const string connectionStringName = "MyConnectionString";

            var result = ConfigurationConnectionStringModelFactory.Create(new ConnectionStringSettings(connectionStringName, connectionString, providerName));

            Assert.Equal(providerName, result.ProviderName);
            Assert.Equal(connectionStringName, result.Key);
            Assert.Equal("Data Source=MyServer;Initial Catalog=MyDatabase;User Id=MyUsername;Password=########", result.Raw);
            Assert.True(result.Details.Count > 4);
            Assert.Equal("MyServer", result.Details["Data Source"]);
            Assert.Equal("MyDatabase", result.Details["Initial Catalog"]);
            Assert.Equal("MyUsername", result.Details["User Id"]);
            Assert.Equal("########", result.Details["Password"]);
        }

        [Fact]
        public void HaveHandledInvalidConnectionStringForKnownProvider()
        {
            const string connectionString = "Data Source=MyServer;Initial Catalog=MyDatabase;User Id=MyUsername;Password=MyPassword;SomeUnknownKey=BadValue";
            const string connectionStringName = "MyConnectionString";
            const string providerName = "System.Data.SqlClient";

            var result = ConfigurationConnectionStringModelFactory.Create(new ConnectionStringSettings(connectionStringName, connectionString, providerName));

            Assert.Equal(providerName, result.ProviderName);
            Assert.Equal(connectionStringName, result.Key);
            Assert.Equal("Data Source=MyServer;Initial Catalog=MyDatabase;User Id=MyUsername;Password=########;SomeUnknownKey=BadValue", result.Raw);
            Assert.Equal(6, result.Details.Count);
            Assert.True(result.Details["ERROR"].ToString().StartsWith("Connection string is invalid for ProviderName=System.Data.SqlClient : "));
            Assert.Equal("MyServer", result.Details["Data Source"]);
            Assert.Equal("MyDatabase", result.Details["Initial Catalog"]);
            Assert.Equal("MyUsername", result.Details["User Id"]);
            Assert.Equal("########", result.Details["Password"]);
            Assert.Equal("BadValue", result.Details["SomeUnknownKey"]);
        }

        [Fact]
        public void HaveHandledUnknownProvider()
        {
            const string connectionString = "Data Source=MyServer;Initial Catalog=MyDatabase;User Id=MyUsername;Password=MyPassword";
            const string connectionStringName = "MyConnectionString";
            const string providerName = "This.Is.Unknown";

            var result = ConfigurationConnectionStringModelFactory.Create(new ConnectionStringSettings(connectionStringName, connectionString, providerName));

            Assert.Equal(providerName, result.ProviderName);
            Assert.Equal(connectionStringName, result.Key);
            Assert.Equal("Data Source=MyServer;Initial Catalog=MyDatabase;User Id=MyUsername;Password=########", result.Raw);
            Assert.Equal(5, result.Details.Count);
            Assert.True(!string.IsNullOrEmpty(result.Details["FATAL"] as string));
            Assert.Equal("MyServer", result.Details["Data Source"]);
            Assert.Equal("MyDatabase", result.Details["Initial Catalog"]);
            Assert.Equal("MyUsername", result.Details["User Id"]);
            Assert.Equal("########", result.Details["Password"]);
        }

        [Fact]
        public void HaveParsedNonTypicalConnectionString()
        {
            const string connectionString = "DefaultEndpointsProtocol=https;AccountName=SomeAccount;AccountKey=SomeAccountKey";
            const string connectionStringName = "MyConnectionString";
            const string providerName = null;

            var result = ConfigurationConnectionStringModelFactory.Create(new ConnectionStringSettings(connectionStringName, connectionString, providerName));

            Assert.Equal(providerName, result.ProviderName);
            Assert.Equal(connectionStringName, result.Key);
            Assert.Equal("DefaultEndpointsProtocol=https;AccountName=SomeAccount;AccountKey=########", result.Raw);
            Assert.Equal(5, result.Details.Count);
            Assert.Equal("ProviderName is empty, therefore assuming ProviderName=System.Data.SqlClient", result.Details["WARNING"]);
            Assert.True(result.Details["ERROR"].ToString().StartsWith("Connection string is invalid for ProviderName=System.Data.SqlClient : "));
            Assert.Equal("https", result.Details["DefaultEndpointsProtocol"]);
            Assert.Equal("SomeAccount", result.Details["AccountName"]);
            Assert.Equal("########", result.Details["AccountKey"]);
        }

        [Fact]
        public void HaveObfuscatedMultipleKeys()
        {
            const string connectionString = "DefaultEndpointsProtocol=https;AccountName=SomeAccount;AccountKey=SomeAccountKey;Password=SomePassword";
            const string connectionStringName = "MyConnectionString";
            const string providerName = null;

            var result = ConfigurationConnectionStringModelFactory.Create(new ConnectionStringSettings(connectionStringName, connectionString, providerName));

            Assert.Equal(providerName, result.ProviderName);
            Assert.Equal(connectionStringName, result.Key);
            Assert.Equal("DefaultEndpointsProtocol=https;AccountName=SomeAccount;AccountKey=########;Password=########", result.Raw);
            Assert.Equal(6, result.Details.Count);
            Assert.Equal("ProviderName is empty, therefore assuming ProviderName=System.Data.SqlClient", result.Details["WARNING"]);
            Assert.True(result.Details["ERROR"].ToString().StartsWith("Connection string is invalid for ProviderName=System.Data.SqlClient : "));
            Assert.Equal("https", result.Details["DefaultEndpointsProtocol"]);
            Assert.Equal("SomeAccount", result.Details["AccountName"]);
            Assert.Equal("########", result.Details["AccountKey"]);
            Assert.Equal("########", result.Details["Password"]);
        }
    }
}