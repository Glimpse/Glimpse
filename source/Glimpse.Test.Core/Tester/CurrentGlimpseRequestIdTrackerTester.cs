using System;
using Glimpse.Core.Framework;

namespace Glimpse.Test.Core.Tester
{
    internal class CurrentGlimpseRequestIdTrackerTester : ICurrentGlimpseRequestIdTracker
    {
        private Guid? TrackedGlimpseRequestId { get; set; }

        public void StartTracking(Guid glimpseRequestId)
        {
            TrackedGlimpseRequestId = glimpseRequestId;
        }

        public bool TryGet(out Guid glimpseRequestId)
        {
            glimpseRequestId = TrackedGlimpseRequestId.HasValue ? TrackedGlimpseRequestId.Value : new Guid();
            return TrackedGlimpseRequestId.HasValue;
        }

        public void StopTracking()
        {
            TrackedGlimpseRequestId = null;
        }
    }
}