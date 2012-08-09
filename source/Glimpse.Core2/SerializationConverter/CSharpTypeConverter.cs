using System;
using System.Linq;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.SerializationConverter
{
    public class CSharpTypeConverter : SerializationConverter<Type>
    {
        public override object Convert(Type type)
        {
            var name = type.Name;

            if (type.IsValueType)
            {
                if (type == typeof (bool)) return "bool";
                if (type == typeof (int)) return "int";
                if (type == typeof (float)) return "float";
                if (type == typeof (double)) return "double";
                if (type == typeof (long)) return "long";
                if (type == typeof (byte)) return "byte";
                if (type == typeof (char)) return "char";
                if (type == typeof (decimal)) return "decimal";
                if (type == typeof (sbyte)) return "sbyte";
                if (type == typeof (short)) return "short";
                if (type == typeof (uint)) return "uint";
                if (type == typeof (ulong)) return "ulong";
                if (type == typeof (ushort)) return "ushort";
            }

            if (type == typeof(string)) return "string";
            if (type == typeof(object)) return "object";
            
            if (!type.IsGenericType)
                return name;

            var plainTypeName = name.Substring(0, name.IndexOf('`'));
            //Recurse over the Convert method (this method) to support N levels of generic parameters
            var parameterList = string.Join(", ", type.GetGenericArguments().Select(Convert).Cast<string>().ToArray());
            //.ToArray() required for .NET 3.5 support in string.Join.
            //.NET 4.0+ string.Join(string, string[])
            //.NET 3.5  string.Join(string, IEnumerable<string>)
            return string.Format("{0}<{1}>", plainTypeName, parameterList);
        }
    }
}