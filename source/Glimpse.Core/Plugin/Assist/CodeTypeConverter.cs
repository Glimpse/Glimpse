using System.Collections.Generic;

namespace Glimpse.Core.Plugin.Assist
{
    public static class CodeTypeConverter
    {
        private static readonly IDictionary<CodeType, string> Map = new Dictionary<CodeType, string>
        {
            { CodeType.Bash, "bsh" },
            { CodeType.Csharp, "cs" },
            { CodeType.Javascript, "js" },
            { CodeType.Python, "py" },
            { CodeType.Ruby, "rb" },
            { CodeType.Shell, "sh" },
        };

        public static string Convert(CodeType codeType)
        {
            return Map.ContainsKey(codeType) ? Map[codeType] : codeType.ToString().ToLower();
        }
    }
}