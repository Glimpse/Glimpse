using System;
using System.Collections.Generic;
using Glimpse.Core.SerializationConverter;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.SerializationConverter
{
    public class CSharpTypeConverterShould
    {
        [Theory]
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(uint), "uint")]
        [InlineData(typeof(bool), "bool")]
        [InlineData(typeof(float), "float")]
        [InlineData(typeof(string), "string")]
        [InlineData(typeof(object), "object")]
        [InlineData(typeof(double), "double")]
        [InlineData(typeof(decimal), "decimal")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(char), "char")]
        [InlineData(typeof(sbyte), "sbyte")]
        [InlineData(typeof(byte), "byte")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(ulong), "ulong")]
        [InlineData(typeof(short), "short")]
        [InlineData(typeof(ushort), "ushort")]
        [InlineData(typeof(DateTime), "DateTime")]
        [InlineData(typeof(CSharpTypeConverterShould), "CSharpTypeConverterShould")]

        // Converter supports N levels of generics
        [InlineData(typeof(IDictionary<string, object>), "IDictionary<string, object>")]
        [InlineData(typeof(IDictionary<string, List<int>>), "IDictionary<string, List<int>>")]
        [InlineData(typeof(IDictionary<string, IDictionary<int, IEnumerable<CSharpTypeConverterShould>>>), "IDictionary<string, IDictionary<int, IEnumerable<CSharpTypeConverterShould>>>")]

        // Converter supports arrays
        [InlineData(typeof(int[]), "int[]")]
        [InlineData(typeof(Test[]), "Test[]")]
        [InlineData(typeof(IEnumerable<int[]>[]), "IEnumerable<int[]>[]")]
        [InlineData(typeof(int[][]), "int[][]")]

        // Converter supports nullable type
        [InlineData(typeof(int?), "int?")]
        [InlineData(typeof(DateTime?), "DateTime?")]
        [InlineData(typeof(Test?), "Test?")]
        [InlineData(typeof(int?[]), "int?[]")]

        // Converter supports nested types
        [InlineData(typeof(Dictionary<string, object>.ValueCollection), "Dictionary<string, object>.ValueCollection")]
        [InlineData(typeof(CSharpTypeConverterShould.DummyClass), "CSharpTypeConverterShould.DummyClass")]
        [InlineData(typeof(CSharpTypeConverterShould.DummyClass.StandardNestedClass), "CSharpTypeConverterShould.DummyClass.StandardNestedClass")]
        [InlineData(typeof(CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, object>), "CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, object>")]
        [InlineData(typeof(CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, object>.InnerClass<int, DateTime>), "CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, object>.InnerClass<int, DateTime>")]
        [InlineData(typeof(CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, object>.InnerClass<int, DateTime>.DeepClass), "CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, object>.InnerClass<int, DateTime>.DeepClass")]
        [InlineData(typeof(CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, object>>.InnerClass), "CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, object>>.InnerClass")]
        [InlineData(typeof(CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, CSharpTypeConverterShould.DummyClass>), "CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, CSharpTypeConverterShould.DummyClass>")]
        [InlineData(typeof(CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, Dictionary<CSharpTypeConverterShould.DummyClass, object>.ValueCollection>.InnerClass), "CSharpTypeConverterShould.DummyClass.GenericNestedClass<string, Dictionary<CSharpTypeConverterShould.DummyClass, object>.ValueCollection>.InnerClass")]

        // Converter - everything together
        [InlineData(typeof(Tuple<IDictionary<int?, string[]>, char, Test>), "Tuple<IDictionary<int?, string[]>, char, Test>")]
        public void ConvertToDisplayString(Type input, string output)
        {
            var converter = new CSharpTypeConverter();
            var result = converter.Convert(input);

            Assert.Equal(output, result);
        }

        private class DummyClass
        {
            public class GenericNestedClass<TA, TB>
            {
                public class InnerClass<TC, TD>
                {
                    public class DeepClass
                    {
                    }
                }

                public class InnerClass
                {
                }
            }

            public class StandardNestedClass
            {
            }
        }
    }

    internal enum Test
    {
        A = 1,
        B = 2,
        C = 3,
    }
}