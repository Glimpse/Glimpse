using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.SerializationConverter
{
    public class CSharpTypeConverter : SerializationConverter<Type>
    {
        public override object Convert(Type type)
        {
            return GetName(type);
        }

        private string GetName(Type type)
        {
            var typename = new StringBuilder();
            GetName(type, typename);
            return typename.ToString();
        }

        private void GetName(Type type, StringBuilder output)
        {
            if (type.IsArray) //Array
            {
                GetName(type.GetElementType(), output);
                output.Append("[]");
            }
            else if (!type.IsGenericType) //Non-Generics
            {
                output.Append(primitiveTypes.ContainsKey(type) ? primitiveTypes[type] : type.Name);
            }
            else if (type.GetGenericTypeDefinition() == typeof(Nullable<>)) //Nullables
            {
                GetName(type.GetGenericArguments().First(), output);
                output.Append("?");
            }
            else //Generics
            {
                var genericBaseType = type.GetGenericTypeDefinition();
                var genericName = genericBaseType.Name;
                output.Append(genericName, 0, genericName.LastIndexOf('`'));
                output.Append("<");

                var genericArguments = type.GetGenericArguments();
                for (int i = 0; i < genericArguments.Length; i++)
                {
                    if (i > 0) output.Append(", ");
                    GetName(genericArguments[i], output);
                }

                output.Append(">");
            }
        }

        private readonly static Dictionary<Type, string> primitiveTypes =
            new Dictionary<Type, string>
            {
                {typeof(bool), "bool"},
                {typeof(byte), "byte"},
                {typeof(char), "char"},
                {typeof(decimal), "decimal"},
                {typeof(double), "double"},
                {typeof(float), "float"},
                {typeof(int), "int"},
                {typeof(long), "long"},
                {typeof(object), "object"},
                {typeof(sbyte), "sbyte"},
                {typeof(short), "short"},
                {typeof(string), "string"},
                {typeof(uint), "uint"},
                {typeof(ulong), "ulong"},
                {typeof(ushort), "ushort"},
            };
    }
}