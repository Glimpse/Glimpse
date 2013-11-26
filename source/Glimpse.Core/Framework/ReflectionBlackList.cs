using System.Collections.Generic;
using System.Reflection;

namespace Glimpse.Core.Framework
{
    public static class ReflectionBlackList
    {
        private static List<string> blackList = new List<string>();

        static ReflectionBlackList()
        {
            blackList.Add("mscorlib");
            blackList.Add("System.Web");
            blackList.Add("System");
            blackList.Add("System.Core");
            blackList.Add("System.Web.ApplicationServices");
            blackList.Add("System.Configuration");
            blackList.Add("System.Xml");
            blackList.Add("System.Runtime.Caching");
            blackList.Add("Microsoft.Build.Utilities.v4.0");
            blackList.Add("Microsoft.JScript");
            blackList.Add("Microsoft.CSharp");
            blackList.Add("System.Data");
            blackList.Add("System.Web.Services");
            blackList.Add("System.Drawing");
            blackList.Add("System.EnterpriseServices");
            blackList.Add("System.IdentityModel");
            blackList.Add("System.Runtime.Serialization");
            blackList.Add("System.ServiceModel");
            blackList.Add("System.ServiceModel.Activation");
            blackList.Add("System.ServiceModel.Web");
            blackList.Add("System.Activities");
            blackList.Add("System.ServiceModel.Activities");
            blackList.Add("System.WorkflowServices");
            blackList.Add("System.Web.Extensions");
            blackList.Add("System.Data.DataSetExtensions");
            blackList.Add("System.Xml.Linq");
            blackList.Add("System.ComponentModel.DataAnnotations");
            blackList.Add("System.Web.DynamicData");
            blackList.Add("AntiXssLibrary");
            blackList.Add("Antlr3.Runtime");
            blackList.Add("Antlr4.StringTemplate");
            blackList.Add("Castle.Core");
            blackList.Add("DotNetOpenAuth.AspNet");
            blackList.Add("DotNetOpenAuth.Core");
            blackList.Add("DotNetOpenAuth.OAuth.Consumer");
            blackList.Add("DotNetOpenAuth.OAuth");
            blackList.Add("DotNetOpenAuth.OpenId");
            blackList.Add("DotNetOpenAuth.OpenId.RelyingParty");
            blackList.Add("EntityFramework");
            blackList.Add("EntityFramework.SqlServer");
            blackList.Add("Microsoft.Web.Infrastructure");
            blackList.Add("Microsoft.Web.WebPages.OAuth");
            blackList.Add("Mono.Math");
            blackList.Add("Newtonsoft.Json");
            blackList.Add("NLog");
            blackList.Add("Org.Mentalis.Security.Cryptography");
            blackList.Add("System.Net.Http");
            blackList.Add("System.Net.Http.Formatting");
            blackList.Add("System.Net.Http.WebRequest");
            blackList.Add("System.Web.Helpers");
            blackList.Add("System.Web.Http");
            blackList.Add("System.Web.Http.WebHost");
            blackList.Add("System.Web.Mvc");
            blackList.Add("System.Web.Optimization");
            blackList.Add("System.Web.Razor");
            blackList.Add("System.Web.WebPages.Deployment");
            blackList.Add("System.Web.WebPages");
            blackList.Add("System.Web.WebPages.Razor");
            blackList.Add("Tavis.UriTemplates");
            blackList.Add("WebGrease");
            blackList.Add("WebMatrix.Data");
            blackList.Add("WebMatrix.WebData");
            blackList.Add("Microsoft.VisualStudio.Web.PageInspector.Loader");
            blackList.Add("Microsoft.VisualStudio.Web.PageInspector.Runtime");
            blackList.Add("Microsoft.VisualStudio.Web.PageInspector.Tracing");
            blackList.Add("System.Xaml");
            blackList.Add("System.Numerics");
            blackList.Add("System.Data.OracleClient");
            blackList.Add("System.Data.SqlServerCe");
            blackList.Add("System.Web.RegularExpressions");
            blackList.Add("System.Data.Services.Design");
            blackList.Add("System.Windows.Forms");
            blackList.Add("System.ServiceModel.Internals");
            blackList.Add("System.Workflow.ComponentModel");
            blackList.Add("System.Workflow.Activities");
            blackList.Add("System.Workflow.Runtime");
            blackList.Add("System.Data.Linq");
            blackList.Add("System.Transactions");
            blackList.Add("System.Data.SqlXml");
            blackList.Add("Microsoft.Build.Framework");
            blackList.Add("System.Xaml.Hosting");
            blackList.Add("Microsoft.VisualBasic.Activities.Compiler");
            blackList.Add("System.Runtime.DurableInstancing");
            blackList.Add("System.Security");
            blackList.Add("System.Dynamic");
        }

        public static bool IsBlackListed(Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;
            if (blackList.Contains(assemblyName))
            {
                return true;
            }
            return false;
        }
    }
}