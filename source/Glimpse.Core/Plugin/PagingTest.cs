using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Plugin
{
    [GlimpsePlugin(ShouldSetupInInit = false)]
    internal class PagingTest : IGlimpsePlugin, IProvideGlimpsePaging
    {
        public string Name
        {
            get { return "PagingTest"; }
        }

        private readonly Guid _pagerKey = Guid.NewGuid();
        public Guid PagerKey
        {
            get { return _pagerKey; }
        }

        public PagerType PagerType
        {
            get { return PagerType.ContinuousPaging; }
        }

        public int PageSize
        {
            get { return 10; }
        }

        public int PageIndex
        {
            get;
            private set;
        }

        public int TotalNumberOfRecords
        {
            get;
            private set;
        }

        public object GetData(HttpContextBase context)
        {
            var data = GetData(context, 0);
            return data;
        }

        public object GetData(HttpContextBase context, int pageIndex)
        {
            // Simulate a lengthy operation to allow the loading message to be shown to the user
            Thread.Sleep(500);

            var data = new List<object[]> { new object[] { "ID", "Column1", "Column2", "Column3", "Column4", "Column5" } };
            var from = pageIndex * PageSize;
            var until = from + PageSize;
            for (var i = from; i < until; i++)
            {
                data.Add(new object[] { i, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() });
            }

            PageIndex = pageIndex;
            TotalNumberOfRecords = 100;

            return data;
        }

        public void SetupInit()
        {
        }
    }
}