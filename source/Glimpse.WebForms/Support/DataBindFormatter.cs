using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.WebForms.Inspector;
using Glimpse.WebForms.Model;
using Glimpse.WebForms.Tab;

namespace Glimpse.WebForms.Support
{
    public class DataBindFormatter
    {
        public void Process(ControlTreeItemTrackModel root, IEnumerable<PageLifeCycleMessage> pageLifeCycleMessages)
        {
            ProcessRecord(root, (Dictionary<string, List<DataBindParameterModel>>)HttpContext.Current.Items["_GlimpseWebFormDataBinding"], pageLifeCycleMessages.OrderByDescending(p => p.Offset));
        }

        private void ProcessRecord(ControlTreeItemTrackModel item, Dictionary<string, List<DataBindParameterModel>> dataBindInfo, IEnumerable<PageLifeCycleMessage> pageLifeCycleMessages)
        {
            if (dataBindInfo.ContainsKey(item.ControlId))
            {
                item.Record.DataBindParameters = new List<DataBindModel>();
                var dataBindModel = new DataBindModel(null, null);
                foreach (var parameterModel in dataBindInfo[item.ControlId])
                {
                    var dataBindParameters = new List<ModelBindParameter>();
                    var time = DateTime.MinValue;
                    foreach (var parameter in parameterModel.DataBindParameters)
                    {
                        dataBindParameters.Add(parameter);
                    }

                    var lifeCycleEvent = pageLifeCycleMessages.First(p => p.Offset <= parameterModel.Offset);
                    if (dataBindModel.Event == lifeCycleEvent.EventName)
                    {
                        var multipleEventParameters = dataBindModel.Parameters as List<EventParameterModel>;
                        if (multipleEventParameters == null)
                        {
                            var existingEventParameters = (List<ModelBindParameter>)dataBindModel.Parameters;
                            multipleEventParameters = new List<EventParameterModel>();
                            dataBindModel.Parameters = multipleEventParameters;
                            multipleEventParameters.Add(new EventParameterModel(0, existingEventParameters));
                        }

                        multipleEventParameters.Add(new EventParameterModel(multipleEventParameters.Count, dataBindParameters));
                    }
                    else
                    {
                        dataBindModel = new DataBindModel(lifeCycleEvent.EventName, dataBindParameters);
                        item.Record.DataBindParameters.Add(dataBindModel);
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