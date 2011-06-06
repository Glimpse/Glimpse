using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Plumbing
{
    public class InProcStackMetadataStore : IGlimpseMetadataStore
    {
        public const string JsonQueue = "Glimpse.JsonQueue";
        private GlimpseConfiguration Configuration { get; set; }
        private HttpApplicationStateBase ApplicationState { get; set; }
        private Queue<GlimpseRequestMetadata> Queue
        {
            get
            {
                var queue = ApplicationState[JsonQueue] as Queue<GlimpseRequestMetadata>;

                if (queue == null)
                    ApplicationState[JsonQueue] = queue = new Queue<GlimpseRequestMetadata>(Configuration.RequestLimit);

                return queue;
            }
        }

        public InProcStackMetadataStore(GlimpseConfiguration configuration, HttpApplicationStateBase applicationState)
        {
            Configuration = configuration;
            ApplicationState = applicationState;
        }

        public void Persist(GlimpseRequestMetadata metadata)
        {
            if (Configuration.RequestLimit == 0) return;

            if (Queue.Count == Configuration.RequestLimit && Configuration.RequestLimit != -1) Queue.Dequeue();
            
            Queue.Enqueue(metadata);
        }

        public IEnumerable<GlimpseRequestMetadata> Requests
        {
            get { return Queue; }
        }
    }
}