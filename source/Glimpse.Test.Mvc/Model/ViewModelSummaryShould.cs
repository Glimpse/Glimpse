using System;
using System.Web.Mvc;
using Glimpse.Mvc.Model;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.Model
{
    public class ViewModelSummaryShould
    {
        [Theory, AutoMock]
        public void SetModelType(ViewDataDictionary viewData, TempDataDictionary tempData, string displayMode, Type displayModeType)
        {
            var sut = new ViewModelSummary(viewData, tempData, typeof(ViewModelSummary), true, displayMode, displayModeType);
             
            Assert.Equal(true, sut.IsValid);
            Assert.Equal(typeof(ViewModelSummary), sut.ModelType);
        }
         
        [Theory, AutoMock]
        public void ReturnViewDataKeys(TempDataDictionary tempData, string displayMode, Type displayModeType)
        {
            var viewData = new ViewDataDictionary { { "A", 1 }, { "B", 2 }, { "C", 3 } };

            var sut = new ViewModelSummary(viewData, tempData, typeof(ViewModelSummary), true, displayMode, displayModeType);

            Assert.Contains("A", sut.ViewDataKeys);
            Assert.Contains("B", sut.ViewDataKeys);
            Assert.Contains("C", sut.ViewDataKeys);
        }

        [Theory, AutoMock]
        public void ReturnTempDataKeys(ViewDataDictionary viewData, string displayMode, Type displayModeType)
        {
            var tempData = new TempDataDictionary { { "A", 1 }, { "B", 2 }, { "C", 3 } };

            var sut = new ViewModelSummary(viewData, tempData, typeof(ViewModelSummary), true, displayMode, displayModeType);

            Assert.Contains("A", sut.TempDataKeys);
            Assert.Contains("B", sut.TempDataKeys);
            Assert.Contains("C", sut.TempDataKeys);
        }

        [Theory, AutoMock]
        public void SetDisplayMode(ViewDataDictionary viewData, TempDataDictionary tempData, string displayMode, Type displayModeType)
        {
            var sut = new ViewModelSummary(viewData, tempData, typeof(ViewModelSummary), true, displayMode, displayModeType);

            Assert.Equal(displayMode, sut.DisplayModeId);
        }

        [Theory, AutoMock]
        public void SetDisplayModeType(ViewDataDictionary viewData, TempDataDictionary tempData, string displayMode, Type displayModeType)
        {
            var sut = new ViewModelSummary(viewData, tempData, typeof(ViewModelSummary), true, displayMode, displayModeType);

            Assert.Equal(displayModeType, sut.DisplayModeType);
            Assert.True(sut.HasDisplayMode);
        }
    }
}