using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Handlers;
using Glimpse.Core.Plumbing;

namespace MvcMusicStore.Glimpse
{
    [GlimpsePlugin]
    public class TestUrlPlugin : IGlimpsePlugin
    {
        public string Name
        {
            get { return "TestURL"; }
        }

        public object GetData(HttpContextBase context)
        {
            return "http://localhost:33333/glimpse.axd?r=TestUrlHandler";
        }

        public void SetupInit()
        { 
        }
    }

    [GlimpseHandler]
    public class TestUrlHandler : JsonHandlerBase
    {
        [ImportingConstructor]
        public TestUrlHandler(GlimpseSerializer serializer) : base(serializer)
        {
        }

        protected override object GetData(HttpContextBase context)
        {
            return new List<object>()
                       {
                           new [] { "Test 1", "Test 2", "Test 3", "Test 4", "Test 5" },
                           new [] { "Test 1", "Test 2", "Test 3", "Test 4", "Test 5" },
                           new [] { "Test 1", "Test 2", "Test 3", "Test 4", "Test 5" },
                           new [] { "Test 1", "Test 2", "Test 3", "Test 4", "Test 5" }
                       } ;
        }

        public override string ResourceName
        {
            get { return "TestUrlHandler"; }
        }
    }
}