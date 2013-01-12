using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Web.Configuration;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.AspNet.Tab
{
    public class Configuration : AspNetTab, IDocumentation, IKey
    {
        private const string PasswordHash = "########";
        private readonly IEnumerable<string> keysToAnnomalizePassword = new[] { "Password", "Pwd" };

        public override string Name
        {
            get { return "Configuration"; }
        }

        public string Key 
        {
            get { return "glimpse_configuration"; }
        }

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Plugin/Config"; }
        }

        public override object GetData(ITabContext context)
        { 
            var result = new ConfigurationModel
                {
                    AppSettings = ConfigurationManager.AppSettings.ToDictionary(),
                    Authentication = ProcessAuthenticationSection(ConfigurationManager.GetSection("system.web/authentication") as AuthenticationSection),
                    ConnectionStrings = ProcessConnectionString(ConfigurationManager.ConnectionStrings),
                    CustomErrors = ProcessCustomErrors(ConfigurationManager.GetSection("system.web/customErrors") as CustomErrorsSection),
                    HttpModules = ProcessHttpModules(ConfigurationManager.GetSection("system.web/httpModules") as HttpModulesSection),
                    HttpHandlers = ProcessHttpHandler(ConfigurationManager.GetSection("system.web/httpHandlers") as HttpHandlersSection),
                    RoleManager = ProcessRoleManager(ConfigurationManager.GetSection("system.web/roleManager") as RoleManagerSection)
                };

            return result;
        }

        private ConfigurationAuthenticationModel ProcessAuthenticationSection(AuthenticationSection authenticationSection)
        {
            if (authenticationSection == null)
            {
                return null;
            }

            var formsSection = authenticationSection.Forms;

            var result = new ConfigurationAuthenticationModel { Mode = authenticationSection.Mode.ToString() };

            if (result.Forms != null)
            {
                result.Forms = new ConfigurationAuthenticationFormsModel
                    {
                        Cookieless = formsSection.Cookieless.ToString(),
                        DefaultUrl = formsSection.DefaultUrl,
                        Domain = formsSection.Domain,
                        EnableCrossAppRedirects = formsSection.EnableCrossAppRedirects,
                        Name = formsSection.Name,
                        Path = formsSection.Path,
                        Protection = formsSection.Protection.ToString(),
                        RequireSSL = formsSection.RequireSSL,
                        SlidingExpiration = formsSection.SlidingExpiration,
                        TicketCompatibilityMode = TicketCompatibilityMode(formsSection),
                        Timeout = formsSection.Timeout
                    };

                var credentialsSection = formsSection.Credentials;
                if (credentialsSection != null)
                {
                    result.Forms.Credentials = new ConfigurationAuthenticationFormsCredentialsModel
                        {
                            PasswordFormat = credentialsSection.PasswordFormat.ToString()
                        };
                }
            }

            return result;
        }

        private IEnumerable<ConfigurationConnectionStringModel> ProcessConnectionString(ConnectionStringSettingsCollection connectionStrings)
        {
            if (connectionStrings == null)
            {
                return null;
            }

            var result = new List<ConfigurationConnectionStringModel>();

            foreach (ConnectionStringSettings connectionString in connectionStrings)
            {
                var resultItem = new ConfigurationConnectionStringModel
                    {
                        Key = connectionString.Name,
                        Raw = connectionString.ConnectionString,
                        ProviderName = connectionString.ProviderName
                    };

                try
                {
                    var connectionFactory = DbProviderFactories.GetFactory(connectionString.ProviderName);
                    var connectionStringBuilder = connectionFactory.CreateConnectionStringBuilder();
                    if (connectionStringBuilder != null)
                    {
                        connectionStringBuilder.ConnectionString = connectionString.ConnectionString;

                        var connectionDetails = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
                        var keys = connectionStringBuilder.Keys;
                        if (keys != null)
                        {
                            foreach (string key in keys)
                            {
                                connectionDetails.Add(key, connectionStringBuilder[key]);
                            }

                            resultItem.Details = connectionDetails;

                            AnnomalizeConnectionStringPassword(connectionDetails, resultItem);
                        }
                    }
                }
                catch (Exception e)
                {
                    resultItem.Details = new Dictionary<string, object> { { "Error", e.Message } };
                }

                result.Add(resultItem);
            }

            return result.Count > 0 ? result : null;
        }

        private void AnnomalizeConnectionStringPassword(IDictionary<string, object> connectionDetails, ConfigurationConnectionStringModel model)
        {
            foreach (var key in keysToAnnomalizePassword)
            {
                if (connectionDetails.ContainsKey(key))
                {
                    var password = connectionDetails[key].ToString();
                    if (!string.IsNullOrEmpty(password))
                    {
                        connectionDetails[key] = PasswordHash; 
                        model.Raw = model.Raw.Replace(password, PasswordHash);
                    }

                    return;
                }
            }
        }

        private ConfigurationCustomErrorsModel ProcessCustomErrors(CustomErrorsSection customErrorsSection)
        {
            if (customErrorsSection == null)
            {
                return null;
            }

            var result = new ConfigurationCustomErrorsModel
                {
                    DefaultRedirect = customErrorsSection.DefaultRedirect,
                    RedirectMode = customErrorsSection.RedirectMode.ToString(),
                    Mode = customErrorsSection.Mode.ToString()
                };

            var errorsSection = customErrorsSection.Errors;
            if (errorsSection != null)
            {
                var resultErrors = new List<ConfigurationCustomErrorsErrorModel>();
                foreach (CustomError error in errorsSection)
                {
                    var resultError = new ConfigurationCustomErrorsErrorModel
                        {
                            Redirect = error.Redirect,
                            StatusCode = error.StatusCode
                        };

                    resultErrors.Add(resultError);
                }

                result.Errors = resultErrors;
            }

            return result;
        }

        private IEnumerable<ConfigurationHttpModulesModel> ProcessHttpModules(HttpModulesSection httpModulesSection)
        {
            if (httpModulesSection == null)
            {
                return null;
            }

            var result = new List<ConfigurationHttpModulesModel>();
            foreach (HttpModuleAction httpModule in httpModulesSection.Modules)
            {
                var resultItem = new ConfigurationHttpModulesModel { Name = httpModule.Name, Type = httpModule.Type };

                result.Add(resultItem);
            }

            return result;
        }

        private IEnumerable<ConfigurationHttpHandlersModel> ProcessHttpHandler(HttpHandlersSection httpHandlersSection)
        {
            if (httpHandlersSection == null)
            {
                return null;
            }

            var result = new List<ConfigurationHttpHandlersModel>();
            foreach (HttpHandlerAction httpModule in httpHandlersSection.Handlers)
            {
                var resultItem = new ConfigurationHttpHandlersModel
                    {
                        Path = httpModule.Path,
                        Verb = httpModule.Verb,
                        Validate = httpModule.Validate,
                        Type = httpModule.Type
                    };

                result.Add(resultItem);
            }

            return result;
        }

        private ConfigurationRoleManagerModel ProcessRoleManager(RoleManagerSection roleManagerSection)
        {
            if (roleManagerSection == null)
            {
                return null;
            }

            var result = new ConfigurationRoleManagerModel
                {
                    CacheRolesInCookie = roleManagerSection.CacheRolesInCookie,
                    CookieName = roleManagerSection.CookieName,
                    CookiePath = roleManagerSection.CookiePath,
                    CookieProtection = roleManagerSection.CookieProtection.ToString(),
                    CookieRequireSSL = roleManagerSection.CookieRequireSSL,
                    CookieSlidingExpiration = roleManagerSection.CookieSlidingExpiration,
                    CookieTimeout = roleManagerSection.CookieTimeout,
                    CreatePersistentCookie = roleManagerSection.CreatePersistentCookie,
                    DefaultProvider = roleManagerSection.DefaultProvider,
                    Domain = roleManagerSection.Domain,
                    Enabled = roleManagerSection.Enabled,
                    MaxCachedResults = roleManagerSection.MaxCachedResults
                };

            var providerSection = roleManagerSection.Providers;
            if (providerSection != null)
            {
                var resultProviders = new List<ConfigurationRoleManagerProviderSettingsModel>();
                foreach (ProviderSettings provider in providerSection)
                {
                    var resultProvider = new ConfigurationRoleManagerProviderSettingsModel
                        {
                            Name = provider.Name,
                            Type = provider.Type,
                            Parameters = provider.Parameters.ToDictionary()
                        };

                    resultProviders.Add(resultProvider);
                }

                result.Providers = resultProviders;
            }

            return result;
        }

        private string TicketCompatibilityMode(FormsAuthenticationConfiguration formsSection)
        {
#if NET35
            return "n/a";      
#else
            return formsSection.TicketCompatibilityMode.ToString();
#endif
        }
    }
}
