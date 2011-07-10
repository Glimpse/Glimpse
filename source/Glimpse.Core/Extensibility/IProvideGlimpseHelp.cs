using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    public interface IProvideGlimpseHelp
    {
        string HelpUrl { get; }
    }

    public interface IProvideGlimpseStructuredLayout
    {
        List<List<GlimpseStructuredLayoutRow>> StructuredLayout { get; }
    }
     
    public class GlimpseStructuredLayoutRow
    {
        public object Data { get; set; }

        public bool? Key { get; set; }

        public string Align { get; set; }

        public string Width { get; set; }

        public string Prefix { get; set; }

        public string Postfix { get; set; }

        public string ClassName { get; set; }

        public bool? IsCode { get; set; }

        public string CodeType { get; set; } 
    }



//[
//    [ { data : [{ data : 0, key : true, align : 'right' }, { data : 2, align : 'right' }, { data : '{{3}} - {{4}}', align : 'right' }, ], width : '200px' }, { data : 1 } ]
//];
}
