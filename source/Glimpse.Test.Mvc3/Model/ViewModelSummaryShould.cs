using System;
using System.Web.Mvc;
using Glimpse.Mvc.Model;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.Model
{
    public class ViewModelSummaryShould
    {
        [Theory, AutoMock]
        public void SetModelType(TempDataDictionary tempData)
        {
            var viewData = new ViewDataDictionary(DateTime.Now);

            var sut = new ViewModelSummary(viewData, tempData);

            Assert.Equal(typeof(DateTime), sut.ModelType);
        }

        [Theory, AutoMock]
        public void IgnoreModelTypeWithWeaklyTypedView(TempDataDictionary tempData)
        {
            var viewData = new ViewDataDictionary();

            var sut = new ViewModelSummary(viewData, tempData);

            Assert.Null(sut.ModelType);
        }

        [Theory, AutoMock]
        public void ReturnViewDataKeys(TempDataDictionary tempData)
        {
            var viewData = new ViewDataDictionary { { "A", 1 }, { "B", 2 }, { "C", 3 } };

            var sut = new ViewModelSummary(viewData, tempData);

            Assert.Contains("A", sut.ViewDataKeys);
            Assert.Contains("B", sut.ViewDataKeys);
            Assert.Contains("C", sut.ViewDataKeys);
        }

        [Theory, AutoMock]
        public void ReturnTempDataKeys(ViewDataDictionary viewData)
        {
            var tempData = new TempDataDictionary { { "A", 1 }, { "B", 2 }, { "C", 3 } };

            var sut = new ViewModelSummary(viewData, tempData);

            Assert.Contains("A", sut.TempDataKeys);
            Assert.Contains("B", sut.TempDataKeys);
            Assert.Contains("C", sut.TempDataKeys);
        }

        [Theory, AutoMock]
        public void SetInvalidModelState(ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            viewData.ModelState.AddModelError("key", @"there was an error");

            var sut = new ViewModelSummary(viewData, tempData);

            Assert.False(sut.IsValid);
        }

        [Theory, AutoMock]
        public void SetValidModelState(ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            var sut = new ViewModelSummary(viewData, tempData);

            Assert.True(sut.IsValid);
        }
    }
}