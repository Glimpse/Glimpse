using System.Reflection;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.SerializationConverter
{
    public class MethodInfoConverter : SerializationConverter<MethodInfo>
    {
        public override object Convert(MethodInfo methodInfo)
        {
            var nameParts = methodInfo.Name.Split('.');
            return nameParts[nameParts.Length - 1] + "()";
        }
    }
}