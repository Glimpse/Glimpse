using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Mvc.Model;
using Glimpse.Mvc.SerializationConverter;
using Glimpse.Test.Common;
using Glimpse.Test.Mvc3.AlternateType;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.SerializationConverter
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