using Glimpse.WebForms.Inspector;
using Glimpse.WebForms.Model;
using Glimpse.WebForms.Tab;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glimpse.WebForms.Support
{
	public class DataBindFormatter
	{
        public void Process(ControlTreeItemTrackModel root, IEnumerable<PageLifeCycleMessage> pageLifeCycleMessages)
		{
            ProcessRecord(root, (Dictionary<string, List<DataBindParameterModel>>)HttpContext.Current.Items["_GlimpseWebFormDataBindingInfo"], pageLifeCycleMessages.OrderByDescending(p => p.StartTime));
		}

        private void ProcessRecord(ControlTreeItemTrackModel item, Dictionary<string, List<DataBindParameterModel>> dataBindInfo, IEnumerable<PageLifeCycleMessage> pageLifeCycleMessages)
		{
			if (dataBindInfo.ContainsKey(item.ControlId))
			{
				item.Record.DataBindParameters = new Dictionary<string, object>();
                foreach (var parameterModel in dataBindInfo[item.ControlId])
				{
                    var dataBindParameters = new List<DataBindParameter>();
                    var time = DateTime.MinValue;
                    foreach (var parameter in parameterModel.DataBindParameters)
					{
                        dataBindParameters.Add(parameter);
					}
                    var lifeCycleEvent = pageLifeCycleMessages.First(p => p.StartTime <= parameterModel.Time);
                    if (item.Record.DataBindParameters.ContainsKey(lifeCycleEvent.EventName))
                    {
                        var multipleEventParameters = item.Record.DataBindParameters[lifeCycleEvent.EventName] as Dictionary<int, object>;
                        if (multipleEventParameters == null)
                        {
                            var existingEventParameters = (List<DataBindParameter>)item.Record.DataBindParameters[lifeCycleEvent.EventName];
                            multipleEventParameters = new Dictionary<int, object>();
                            item.Record.DataBindParameters[lifeCycleEvent.EventName] = multipleEventParameters;
                            multipleEventParameters[0] = existingEventParameters;
                        }
                        multipleEventParameters[multipleEventParameters.Count] = dataBindParameters;
                    }
                    else
                    {
                        item.Record.DataBindParameters[lifeCycleEvent.EventName] = dataBindParameters;
                    }
				}
			}
			foreach (var child in item.Children)
			{
                ProcessRecord(child, dataBindInfo, pageLifeCycleMessages);
			}
		}
	}
}
