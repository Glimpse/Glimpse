using System.Collections.Generic;
using Glimpse.Mvc.Model;
using Glimpse.Mvc.SerializationConverter;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.SerializationConverter
{
    public class ListOfViewsModelConverterShould
    {
        [Theory, AutoMock]
        public void ConvertEmptyCollection(ListOfViewsModelConverter sut)
        {
            var result = sut.Convert(new List<ViewsModel>()) as IEnumerable<object>;

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        } 
    }
}