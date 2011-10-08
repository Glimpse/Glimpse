using System.Collections;
using System.Collections.Generic;
using Glimpse.AspNet;
using Xunit;

namespace Glimpse.Test.AspNet
{
    public class DictionaryDataStoreAdapterShould
    {
        public IDictionary Dictionary { get; set; }

        public DictionaryDataStoreAdapterShould()
        {
            Dictionary = new Dictionary<object, object>
                             {
                                 {typeof(string),"TestString"},
                                 {typeof(int), 4},
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
    }
}
