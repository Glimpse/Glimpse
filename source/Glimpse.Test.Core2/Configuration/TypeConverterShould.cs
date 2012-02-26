using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using Moq;
using Xunit;
using TypeConverter = Glimpse.Core2.Configuration.TypeConverter;

namespace Glimpse.Test.Core2.Configuration
{
    public class TypeConverterShould
    {
        [Fact]
        public void ConvertToType()
        {
            var converter = new TypeConverter();
            var name = GetType().AssemblyQualifiedName;
            //Case insensitive type matching
            var result = converter.ConvertFrom(name.ToLower());

            var type = result as Type;
            Assert.NotNull(type);
            Assert.Equal(typeof(TypeConverterShould), type);
        }

        [Fact]
        public void ThrowTypeLoadExceptionWithBadTypeString()
        {
            var converter = new TypeConverter();

            Assert.Throws<TypeLoadException>(() => converter.ConvertFrom("bad string"));
        }

        [Fact]
        public void ThrowArgumentExceptionWithMissingTypeString()
        {
            var converter = new TypeConverter();

            Assert.Throws<ArgumentException>(() => converter.ConvertFrom(""));
            Assert.Throws<ArgumentException>(() => converter.ConvertFrom(null));
        }

        [Fact]
        public void ConvertTo()
        {
            var contextMock = new Mock<ITypeDescriptorContext>();

            var converter = new TypeConverter();

            var result = converter.ConvertTo(contextMock.Object, Thread.CurrentThread.CurrentCulture, "any object", typeof (string));
        }
    }
}