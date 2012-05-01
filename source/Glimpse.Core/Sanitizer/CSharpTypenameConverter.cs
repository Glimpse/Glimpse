using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Glimpse.Core.Sanitizer
{
    public class CSharpTypenameConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (typeof(Type).IsAssignableFrom(objectType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(GetCSharpname((Type)value));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private string GetCSharpname(Type type)
        {
            var typename = new StringBuilder();
            GetCSharpname(type, typename);
            return typename.ToString();
        }

        private void GetCSharpname(Type type, StringBuilder output)
        {
            bool useFullName = false;
            bool aliasForPrimitives = true;

            if (type.IsArray)
            {
                GetCSharpname(type.GetElementType(), output);
                output.Append("[]");
            }
            else if (!type.IsGenericType)
            {
                if (aliasForPrimitives && primitiveTypes.ContainsKey(type))
                {
                    output.Append(primitiveTypes[type]);
                }
                else if (useFullName)
                {
                    output.Append(type.FullName);
                }
                else
                {
                    output.Append(type.Name);
                }
            }
            else if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                GetCSharpname(type.GetGenericArguments().First(), output);
                output.Append("?");
            }
            else
            {
                var genericBaseType = type.GetGenericTypeDefinition();
                var genericName = useFullName ? genericBaseType.FullName : genericBaseType.Name;
                output.Append(genericName, 0, genericName.LastIndexOf('`'));
                output.Append("<");

                var genericArguments = type.GetGenericArguments();
                for (int i = 0; i < genericArguments.Length; i++)
                {
                    if (i > 0) output.Append(", ");
                    GetCSharpname(genericArguments[i], output);
                }

                output.Append(">");
                return;
            }
        }

        private readonly static Dictionary<Type, string> primitiveTypes =
            new Dictionary<Type, string>
            {
                {typeof(object), "object"},
                {typeof(string), "string"},
                {typeof(bool), "bool"},
                {typeof(byte), "byte"},
                {typeof(short), "short"},
                {typeof(int), "int"},
                {typeof(long), "long"},
                {typeof(sbyte), "sbyte"},
                {typeof(ushort), "ushort"},
                {typeof(uint), "uint"},
                {typeof(ulong), "ulong"},
                {typeof(decimal), "decimal"},
                {typeof(float), "float"},
                {typeof(double), "double"}
            };
    }
}
