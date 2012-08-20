using System;
using System.Web.Mvc;
using Glimpse.Mvc3.Model;
using Xunit;

namespace Glimpse.Test.Mvc3.Model
{
    public class ViewModelSummaryShould
    {
        [Fact]
        public void SetModelType()
        {
            var vm = DateTime.Now;
            var viewData = new ViewDataDictionary(vm);
            var tempData = new TempDataDictionary();

            var model = new ViewModelSummary(viewData, tempData);

            Assert.Equal(typeof(DateTime), model.ModelType);
        }

        [Fact]
        public void IgnoreModelTypeWithWeaklyTypedView()
        {
            var viewData = new ViewDataDictionary();
            var tempData = new TempDataDictionary();

            var model = new ViewModelSummary(viewData, tempData);

            Assert.Null(model.ModelType);
        }

        [Fact]
        public void ReturnViewDataKeys()
        {
            var viewData = new ViewDataDictionary {{"A", 1}, {"B", 2}, {"C", 3}};
            var tempData = new TempDataDictionary();

            var model = new ViewModelSummary(viewData, tempData);

            Assert.Contains("A", model.ViewDataKeys);
            Assert.Contains("B", model.ViewDataKeys);
            Assert.Contains("C", model.ViewDataKeys);
        }

        [Fact]
        public void ReturnTempDataKeys()
        {
            var viewData = new ViewDataDictionary();
            var tempData = new TempDataDictionary { { "A", 1 }, { "B", 2 }, { "C", 3 } };

            var model = new ViewModelSummary(viewData, tempData);

            Assert.Contains("A", model.TempDataKeys);
            Assert.Contains("B", model.TempDataKeys);
            Assert.Contains("C", model.TempDataKeys);
        }

        [Fact]
        public void SetInvalidModelState()
        {
            var viewData = new ViewDataDictionary();
            viewData.ModelState.AddModelError("key", "there was an error");

            var tempData = new TempDataDictionary();

            var model = new ViewModelSummary(viewData, tempData);

            Assert.False(model.IsValid);
        }

        [Fact]
        public void SetValidModelState()
        {
            var viewData = new ViewDataDictionary();

            var tempData = new TempDataDictionary();

            var model = new ViewModelSummary(viewData, tempData);

            Assert.True(model.IsValid);
        }
    }
}