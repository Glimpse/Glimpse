using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ManualBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            var assets = new Assets(args[0]);

            var coreContent = File.ReadAllText(assets.BuildScriptPath("glimpse.core.js"));

            var matches = Regex.Matches(coreContent, @"\/\*\(import:\S*\)\*\/", RegexOptions.Multiline); 
            foreach (Match match in matches)
            {
                var matchFileName = match.Value.Substring(10, match.Value.Length - 13);
                var matchContent = File.ReadAllText(assets.BuildScriptPath(matchFileName));

                coreContent = coreContent.Replace(String.Format("/*(import:{0})*/", matchFileName), matchContent); 
            }

            File.WriteAllText(assets.BuildPath("glimpseCore2.js"), coreContent);
            
        }
    }

    public class Assets
    {
        public Assets(string basePath)
        {
            BasePath = basePath;
        }

        public string BasePath { get; set; }

        public string BuildScriptPath(string relativeFilePath)
        {
            return Path.Combine(BasePath, "Scripts", relativeFilePath);
        }

        public string BuildPath(string relativeFilePath)
        {
            return Path.Combine(BasePath, relativeFilePath);
        }
    }
}
