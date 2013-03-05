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
        public void SetModelType(ViewDataDictionary viewData, TempDataDictionary tempData)
        { 
            var sut = new ViewModelSummary(viewData, tempData, typeof(ViewModelSummary), true);
             
            Assert.Equal(true, sut.IsValid);
            Assert.Equal(typeof(ViewModelSummary), sut.ModelType);
        }
         
        [Theory, AutoMock]
        public void ReturnViewDataKeys(TempDataDictionary tempData)
        {
            var viewData = new ViewDataDictionary { { "A", 1 }, { "B", 2 }, { "C", 3 } };

            var sut = new ViewModelSummary(viewData, tempData, typeof(ViewModelSummary), true);

            Assert.Contains("A", sut.ViewDataKeys);
            Assert.Contains("B", sut.ViewDataKeys);
            Assert.Contains("C", sut.ViewDataKeys);
        }

        [Theory, AutoMock]
        public void ReturnTempDataKeys(ViewDataDictionary viewData)
        {
            var tempData = new TempDataDictionary { { "A", 1 }, { "B", 2 }, { "C", 3 } };

            var sut = new ViewModelSummary(viewData, tempData, typeof(ViewModelSummary), true);

            Assert.Contains("A", sut.TempDataKeys);
            Assert.Contains("B", sut.TempDataKeys);
            Assert.Contains("C", sut.TempDataKeys);
        }
    }
}