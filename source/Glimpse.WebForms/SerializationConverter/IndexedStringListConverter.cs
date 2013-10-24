using System;
using System.Collections;
using System.Collections.Generic; 
using System.Web.UI; 
using Glimpse.Core.Extensibility;

namespace Glimpse.WebForms.SerializationConverter
{
    public class IndexedStringListConverter : ISerializationConverter
    {
        public IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type> { typeof(Pair), typeof(Triplet), typeof(IndexedStringListConverterTarget) }; }
        }

        public object Convert(object target)
        {
            var pair = target as Pair;
            if (pair != null)
            {
                var result = new Dictionary<string, object>();
                result.Add("first", ProcessValue(pair.First));
                result.Add("second", ProcessValue(pair.Second));

                return result;
            }
            
            var triplet = target as Triplet;
            if (triplet != null)
            {
                var result = new Dictionary<string, object>();
                result.Add("first", ProcessValue(triplet.First));
                result.Add("second", ProcessValue(triplet.Second));
                result.Add("third", ProcessValue(triplet.Third));

                return result;
            }

            var store = target as IndexedStringListConverterTarget;
            if (store != null)
            {
                return ProcessValue(store.Data);
            }

            return target;
        }

        private object ProcessValue(object data)
        {
            if (data != null)
            {
                var list = data as IList;
                if (list != null && list.Count > 0)
                {
                    if (list.Count%2 == 0 && list[0] is IndexedString)
                    {
                        var result = new Dictionary<object, object>();
                        for (int i = 0; i < list.Count; i = i + 2)
                        {
                            var key = list[i];
                            if (key is IndexedString)
                            {
                                key = ((IndexedString)key).Value;
                            }

                            result.Add(key, list[i + 1]);
                        }

                        data = result;
                    }
                    else
                    {
                        var temp = ProcessValue(list[0]);
                        if (temp != list[0])
                        {
                            var result = new Dictionary<int, object> { {0, temp} }; 
                            for (int i = 1; i < list.Count; i++)
                            {
                                result.Add(i, ProcessValue(list[i]));
                            }

                            data = result; 
                        }
                    }
                }
            }

            return data;
        } 
    }
}
