using Glimpse.WebForms.Model;
using Glimpse.WebForms.Tab;
using System.Collections.Generic;
using System.Web;

namespace Glimpse.WebForms.Support
{
	public class DataBindFormatter
	{
		public void Process(ControlTreeItemTrackModel root)
		{
			ProcessRecord(root, (Dictionary<string, List<List<DataBindParameterModel>>>) HttpContext.Current.Items["_GlimpseWebFormDataBindingInfo"]);
		}

		private void ProcessRecord(ControlTreeItemTrackModel item, Dictionary<string, List<List<DataBindParameterModel>>> dataBindInfo)
		{
			if (dataBindInfo.ContainsKey(item.ControlId))
			{
				item.Record.DataBindParameters = new List<DataBindParameterModel>();
				foreach(var parameters in dataBindInfo[item.ControlId])
				{
					foreach (var parameter in parameters)
					{
						item.Record.DataBindParameters.Add(parameter);
					}
				}
			}
			foreach (var child in item.Children)
			{
				ProcessRecord(child, dataBindInfo);
			}
		}
	}
}
