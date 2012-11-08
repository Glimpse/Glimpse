using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin.Assist;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.SerializationConverter
{
    public class ListOfExecutionModelConverter : SerializationConverter<List<ExecutionModel>>
    {
        public override object Convert(List<ExecutionModel> models)
        {
            var ordinal = 0;

            var section = new TabSection("Ordinal", "Is Child", "Category", "Type", "Method", "Time Elapsed");
            foreach (var model in models)
            {
                section.AddRow()
                    .Column(ordinal++)
                    .Column(model.IsChildAction)
                    .Column(model.Category.ToString())
                    .Column(model.ExecutedType)
                    .Column(model.ExecutedMethod)
                    .Column(model.MillisecondsDuration)
                    .SelectedIf(!model.Category.HasValue);
            }

            return section.Build();
        }
    }
}