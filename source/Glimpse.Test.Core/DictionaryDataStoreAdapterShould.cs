using System;
using System.Collections;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Xunit;

namespace Glimpse.Test.Core
{
    public class DictionaryDataStoreAdapterShould
    {
        public IDictionary Dictionary { get; set; }

        public DictionaryDataStoreAdapterShould()
        {
            Dictionary = new Dictionary<string, object>
                             {
                                 {typeof(string).AssemblyQualifiedName,"TestString"},
                                 {typeof(int).AssemblyQualifiedName, 4},
                                 {"intKey", 5}
                             };
        }

        [Fact]
        public void ConstructWithDictionary()
        {
            var instance = new DictionaryDataStoreAdapter(Dictionary);

            Assert.NotNull(instance);
        }

        [Fact]
        public void GetViaGenerics()
        {
            var instance = new DictionaryDataStoreAdapter(Dictionary);

            var result = instance.Get<string>();
            Assert.Equal("TestString", result);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void GetViaGenericsWithKey()
        {
            var instance = new DictionaryDataStoreAdapter(Dictionary);

            var result = instance.Get<int>("intKey");
            Assert.Equal(5, result);
            Assert.IsType<int>(result);
        }

        [Fact]
        public void GetViaKey()
        {
            var instance = new DictionaryDataStoreAdapter(Dictionary);

            Assert.Equal(5, instance.Get("intKey"));
        }

        [Fact]
        public void SetViaGenerics()
        {
            var instance = new DictionaryDataStoreAdapter(Dictionary);
            instance.Set<bool>(true);
            
            Assert.True(instance.Get<bool>());
        }

        [Fact]
        public void SetViaKey()
        {
            var instance = new DictionaryDataStoreAdapter(Dictionary);
            var random = new {Some = "Test", Data = 5};

            instance.Set("random", random);

            Assert.Equal(random, instance.Get("random"));
        }

        [Fact]
        public void WorkWithStringBasedKeys()
        {
            var instance = new DictionaryDataStoreAdapter(new Dictionary<string,object>());

            instance.Set<int>(5);

            Assert.Equal(5, instance.Get<int>());
        }

        [Fact]
        public void ThrowArgumentExceptionWithNonStringOrObjectKeyedDictionary()
        {
            Assert.Throws<ArgumentException>(()=>
                                                 {
                                                     new DictionaryDataStoreAdapter(new Dictionary<int, object>());
                                                 }
                );
        }

        [Fact]
        public void ConstructWithNonGenericDictionary()
        {
            var dictionary = new Hashtable();
            var instance = new DictionaryDataStoreAdapter(dictionary);

            Assert.NotNull(instance);
        }

        [Fact]
        public void ThrowWithImproperInternalDictionary()
        {
            var dictionary = new Dictionary<string, string>();

            Assert.Throws<ArgumentException>(() => new DictionaryDataStoreAdapter(dictionary));
        }

        [Fact]
        public void ContainItems()
        {
            var instance = new DictionaryDataStoreAdapter(Dictionary);
            Assert.True(instance.Contains("intKey"));
        }

        [Fact]
        public void NotContainItems()
        {
            var instance = new DictionaryDataStoreAdapter(Dictionary);
            Assert.False(instance.Contains("random string"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullDictionary()
        {
            Assert.Throws<ArgumentException>(()=>new DictionaryDataStoreAdapter(null));
        }
    }
}
