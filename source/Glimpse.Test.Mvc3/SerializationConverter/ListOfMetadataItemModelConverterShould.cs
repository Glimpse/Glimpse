using System.Collections.Generic;
using System.Linq;
using Glimpse.Mvc.Model;
using Glimpse.Mvc.SerializationConverter;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.SerializationConverter
{
    public class ListOfMetadataItemModelConverterShould
    {
        [Theory, AutoMock]
        public void ConvertToList(ListOfMetadataItemModelConverter sut)
        {
            var model = new List<MetadataItemModel> { new MetadataItemModel { Action = "Action", Controller = "Controller", DisplayName = "Name", Name = "Display", Type = typeof(int) } };

            var result = sut.Convert(model) as IEnumerable<object>;

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
