using System.Text;
using System.Web.Mvc;

namespace Glimpse.Net.Extentions
{
    internal static class ActionDescriptorExtentions
    {
        //returns a method name like "Index(int id)" instead of "Index"
        internal static string GetMethodNameWithParameters(this ActionDescriptor mvcActionDescriptor)
        {
            var sb = new StringBuilder(mvcActionDescriptor.ActionName + "(");

            var parameters = mvcActionDescriptor.GetParameters();
            foreach (var parameter in parameters)
            {
                sb.Append(parameter.ParameterType + " ");
                sb.Append(parameter.ParameterName + ", ");
            }
            if (parameters.Length > 0) sb.Remove(sb.Length - 2, 2);
            sb.Append(")");
            return sb.ToString();
        }
    }
}
