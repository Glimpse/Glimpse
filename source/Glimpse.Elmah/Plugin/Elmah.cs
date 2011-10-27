using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Elmah;
using Glimpse.Core.Extensibility;
using Glimpse.Elmah.Plumbing;

namespace Glimpse.Elmah.Plugin
{
    [GlimpsePlugin]
    public class ElmahPlugin : IGlimpsePlugin, IProvideGlimpseHelp, IProvideGlimpsePaging
    {
        private readonly IConfigurationReaderFactory _configurationReaderFactory;

        [ImportingConstructor]
        public ElmahPlugin()
            : this(new ConfigurationReaderFactory())
        {
        }

        public ElmahPlugin(IConfigurationReaderFactory configurationReaderFactory)
        {
            if (configurationReaderFactory == null)
                throw new ArgumentNullException("configurationReaderFactory");

            _configurationReaderFactory = configurationReaderFactory;
        }

        public string Name
        {
            get { return "Elmah"; }
        }

        public string HelpUrl
        {
            get { return "http://elmah4glimpse.codeplex.com/"; }
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
            var errors = GetErrors(context, pageIndex, PageSize);
            return errors;
        }

        private object GetErrors(HttpContextBase context, int pageIndex, int pageSize)
        {
            // Validate parameters
            if (context == null)
                return null;

            // Get the errors from Elmah
            var errorList = new List<ErrorLogEntry>();
            var totalCount = ErrorLog.GetDefault(context.ApplicationInstance.Context).GetErrors(pageIndex, pageSize, errorList);
            if (totalCount == 0)
                return null;

            if (errorList.Count() == 0)
                return null;

            // Create the header row
            var data = new List<object>
			           	{
			           		new object[]
			           			{
			           				"Host",
			           				"Code",
			           				"Type",
			           				"Error",
			           				"User",
			           				"Date",
			           				"Time",
			           				"Details"
			           			}
			           	};

            // Create the data rows
            var path = VirtualPathUtility.ToAbsolute("~/", context.Request.ApplicationPath);
            var errorPageUrl = GetErrorPageUrl();

            data.AddRange(errorList
                .Select(e => new object[]
                    {
                        e.Error.HostName,
                        e.Error.StatusCode,
                        e.Error.Type,
                        e.Error.Message,
                        e.Error.User,
                        e.Error.Time.ToShortDateString(),
                        e.Error.Time.ToShortTimeString(),
                        string.Format("!<a href=\"{0}{1}/detail?id={2}&\" target=\"_blank\">View</a>!", path, errorPageUrl, e.Id)
                    })
                .ToList());

            // Set the paging properties
            PageIndex = pageIndex;
            TotalNumberOfRecords = totalCount;

            // Return the error page
            return data;
        }

        private string GetErrorPageUrl()
        {
            var configurationReader = _configurationReaderFactory.Create();
            if (configurationReader == null)
                return null;

            var errorPageUrl = configurationReader.GetPathFor<ErrorLogPageFactory>();
            return errorPageUrl;
        }

        public void SetupInit()
        {
        }
    }
}