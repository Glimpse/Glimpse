using System;
using System.IO;
using System.Reflection;

namespace Glimpse.WindowsAzure.Infrastructure
{
    public class WindowsAzureCloudServicesEnvironment
        : IWindowsAzureEnvironment
    {
        private static Assembly ServiceRuntimeAssembly { get; set; }
        private static Type RoleEnvironmentType { get; set; }

        static WindowsAzureCloudServicesEnvironment()
        {
            ServiceRuntimeAssembly = TryLoadServiceRuntimeAssembly();
            TryLoadServiceRuntimeTypes();
        }

        public bool IsAvailable
        {
            get
            {
                return ServiceRuntimeAssembly != null && WrappedIsAvailable();
            }
        }

        public string DeploymentId
        {
            get
            {
                VerifyAvailable();
                return (string)RoleEnvironmentType.GetProperty("DeploymentId").GetValue(null, null);
            }
        }
        
        public bool IsEmulated
        {
            get
            {
                VerifyAvailable();
                return (bool)RoleEnvironmentType.GetProperty("IsEmulated").GetValue(null, null);
            }
        }

        public object CurrentRoleInstance
        {
            get
            {
                VerifyAvailable();
                return RoleEnvironmentType.GetProperty("CurrentRoleInstance").GetValue(null, null);
            }
        }

        public object Roles
        {
            get
            {
                VerifyAvailable();
                return RoleEnvironmentType.GetProperty("Roles").GetValue(null, null);
            }
        }

        protected void VerifyAvailable()
        {
            if (!IsAvailable)
            {
                // todo: make this a custom exception type
                throw new Exception("Role environment is not available.");
            }
        }

        private static void TryLoadServiceRuntimeTypes()
        {
            try
            {
                RoleEnvironmentType = ServiceRuntimeAssembly.GetType("Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment");
            }
            catch (ReflectionTypeLoadException)
            {
            }
        }

        static bool WrappedIsAvailable()
        {
            try
            {
                return (bool)RoleEnvironmentType.GetProperty("IsAvailable").GetValue(null, null);
            }  
            catch (TargetInvocationException ex)
            {
                Exception innerException1 = ex;
                for (int i = 0; i < 3; i++)
                {
                    if (innerException1.InnerException != null)
                    {
                        innerException1 = innerException1.InnerException;
                    }
                }
                if (innerException1 != null && innerException1.Message.Contains("C++ module failed to load"))
                {
                    return false;
                }
                throw;
            }
            catch (TypeInitializationException ex)
            {
                var innerException2 = ex.InnerException;
                if (innerException2 != null && innerException2 is FileNotFoundException && innerException2.Message.Contains("msshrtmi"))
                {
                    return false;
                }
                throw;
            }
        }

        private static Assembly TryLoadServiceRuntimeAssembly()
        {
            try
            {
                var serviceRuntimeAssembly = Assembly.LoadWithPartialName("Microsoft.WindowsAzure.ServiceRuntime, Culture=neutral, PublicKeyToken=31bf3856ad364e35, ProcessorArchitecture=MSIL");
                return serviceRuntimeAssembly;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

    }
}
