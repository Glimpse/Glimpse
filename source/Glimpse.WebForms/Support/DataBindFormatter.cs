using Glimpse.WebForms.Inspector;
using Glimpse.WebForms.Model;
using Glimpse.WebForms.Tab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glimpse.WebForms.Support
{
	public class DataBindFormatter
	{
        public void Process(ControlTreeItemTrackModel root, IEnumerable<PageLifeCycleMessage> pageLifeCycleMessages)
		{
            ProcessRecord(root, (Dictionary<string, List<DataBindParameterModel>>)HttpContext.Current.Items["_GlimpseWebFormDataBindingInfo"], pageLifeCycleMessages);
		}

        private void ProcessRecord(ControlTreeItemTrackModel item, Dictionary<string, List<DataBindParameterModel>> dataBindInfo, IEnumerable<PageLifeCycleMessage> pageLifeCycleMessages)
		{
			if (dataBindInfo.ContainsKey(item.ControlId))
			{
                var i = 0;
				item.Record.DataBindParameters = new Dictionary<string, List<DataBindParameter>>();
                var previousEventName = string.Empty;
				foreach(var parameterModel in dataBindInfo[item.ControlId])
				{
                    var dataBindParameters = new List<DataBindParameter>();
                    var time = DateTime.MinValue;
                    foreach (var parameter in parameterModel.DataBindParameters)
					{
                        dataBindParameters.Add(parameter);
					}
                    var lifeCycleEvent = pageLifeCycleMessages.First(p => p.StartTime <= parameterModel.Time && parameterModel.Time <= p.StartTime.Add(p.Duration));
                    i = previousEventName == lifeCycleEvent.EventName ? i + 1 : 0;
                    item.Record.DataBindParameters[i == 0 ? lifeCycleEvent.EventName : lifeCycleEvent.EventName + " " + i ] = dataBindParameters;
                    previousEventName = lifeCycleEvent.EventName;
				}
			}
			foreach (var child in item.Children)
			{
                ProcessRecord(child, dataBindInfo, pageLifeCycleMessages);
			}
		}
	}
}
