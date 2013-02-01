using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.SerializationConverter
{
    /// <summary>
    /// The <see cref="ISerializationConverter"/> implementation responsible converting <see cref="Type"/> representation's into more recognizable C# syntax.
    /// </summary>
    /// <example>
    /// With <see cref="CSharpTypeConverter"/>, <c>System.Int32</c> is converted to <c>int</c> and <c>System.Collections.Generic.IDictionary`2[System.Double, System.String[]]</c> to <c>IDictionary&lt;double, string[]&gt;</c>.
    /// </example>
    /// <remarks>
    /// Users of other languages could disable <see cref="CSharpTypeConverter"/> and create a <c>SerializationConverter&lt;Type&gt;</c> implementation for their language.
    /// </remarks>
    public class CSharpTypeConverter : SerializationConverter<Type>
    {
        private static readonly Dictionary<Type, string> PrimitiveTypes =
            new Dictionary<Type, string>
                {
                    { typeof(bool), "bool" },
                    { typeof(byte), "byte" },
                    { typeof(char), "char" },
                    { typeof(decimal), "decimal" },
                    { typeof(double), "double" },
                    { typeof(float), "float" },
                    { typeof(int), "int" },
                    { typeof(long), "long" },
                    { typeof(object), "object" },
                    { typeof(sbyte), "sbyte" },
                    { typeof(short), "short" },
                    { typeof(string), "string" },
                    { typeof(uint), "uint" },
                    { typeof(ulong), "ulong" },
                    { typeof(ushort), "ushort" },
                };

        /// <summary>
        /// Converts the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>An string of the C# syntax which would be used to represent <paramref name="type"/>.</returns>
        public override object Convert(Type type)
        {
            return GetName(type);
        }

        private string GetName(Type type)
        {
            var typeName = new StringBuilder();
            GetName(type, typeName);
            return typeName.ToString();
        }

        private void GetName(Type type, StringBuilder output)
        {
            if (type.IsArray) 
            {
                // Array
                GetName(type.GetElementType(), output);
                output.Append("[]");
            }
            else if (!type.IsGenericType)
            {
                // Non-Generics
                output.Append(PrimitiveTypes.ContainsKey(type) ? PrimitiveTypes[type] : type.Name);
            }
            else if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // Null types
                GetName(type.GetGenericArguments().First(), output);
                output.Append("?");
            }
            else 
            {
                // Generics
                var genericBaseType = type.GetGenericTypeDefinition();
                var genericName = genericBaseType.Name;
                output.Append(genericName, 0, genericName.LastIndexOf('`'));
                output.Append("<");

                var genericArguments = type.GetGenericArguments();
                for (int i = 0; i < genericArguments.Length; i++)
                {
                    if (i > 0)
                    {
                        output.Append(", ");
                    }

                    GetName(genericArguments[i], output);
                }

                output.Append(">");
            }
        }
    }
}