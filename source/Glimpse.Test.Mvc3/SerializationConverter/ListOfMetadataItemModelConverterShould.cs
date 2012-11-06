using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.SerializationConverter;
using Glimpse.Core.Plugin.Assist;
using Glimpse.Mvc.Model;
using Glimpse.Mvc.SerializationConverter;
using Xunit;

namespace Glimpse.Test.Mvc3.SerializationConverter
{
    public class ListOfMetadataItemModelConverterShould
    {
        [Fact]
        public void ConvertToList()
        {
            var model = new List<MetadataItemModel> { new MetadataItemModel { Action = "Action", Controller = "Controller", DisplayName = "Name", Name = "Display", Type = typeof(int) } };

            var converter = new ListOfMetadataItemModelConverter();
            var result = converter.Convert(model) as IList<object[]>;

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
    }
}
