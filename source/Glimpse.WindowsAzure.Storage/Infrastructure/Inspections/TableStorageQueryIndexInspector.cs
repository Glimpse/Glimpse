using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query;

namespace Glimpse.WindowsAzure.Storage.Infrastructure.Inspections
{
    public class TableStorageQueryIndexInspector
        : WindowsAzureStorageInspectorBase
    {
        private static readonly EdmModel EdmModel;
        private static readonly EdmEntityType TableServiceEntity;

        static TableStorageQueryIndexInspector()
        {
            EdmModel = new EdmModel();
            TableServiceEntity = new EdmEntityType("Glimpse.WindowsAzure.Storage", "TableServiceEntity");
            TableServiceEntity.AddStructuralProperty("PartitionKey", EdmPrimitiveTypeKind.String);
            TableServiceEntity.AddStructuralProperty("RowKey", EdmPrimitiveTypeKind.String);
            TableServiceEntity.AddStructuralProperty("Timestamp", EdmPrimitiveTypeKind.DateTime);
            EdmModel.AddElement(TableServiceEntity);

            var container = new EdmEntityContainer("TestModel", "DefaultContainer");
            container.AddEntitySet("Entities", TableServiceEntity);
            EdmModel.AddElement(container);
        }

        public override IEnumerable<string> Inspect(WindowsAzureStorageTimelineMessage message)
        {
            // todo: refactor, this has been written on a tight schedule
            if (message.ServiceName == "Table" && message.Url.Contains("$filter="))
            {
                string filterString = HttpUtility.ParseQueryString(new Uri(message.Url).Query)["$filter"];

                try
                {
                    ODataUriParser.ParseFilter(filterString, EdmModel, TableServiceEntity);
                }
                catch (ODataException)
                {
                    // this means we're using other properties...
                    return new[] { "This query may not perform as intended. Consider optimizing the entity structure so that a PartitionKey/RowKey query can be executed instead." };
                } 
            }

            return new string[0];
        }
    }
}