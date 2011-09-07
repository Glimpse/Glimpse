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
            coreContent = ProcessFile(coreContent, assets);

            File.WriteAllText(assets.BuildPath("glimpseCore2.js"), coreContent); 
        }

        static string ProcessFile(string fileContent, Assets assets)
        {
            var matches = Regex.Matches(fileContent, @"\/\*\(import:\S*\)\*\/", RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                var matchFileName = match.Value.Substring(10, match.Value.Length - 13);
                var matchContent = File.ReadAllText(assets.BuildScriptPath(matchFileName));
                matchContent = ProcessFile(PostProcessContent(matchFileName, matchContent), assets);

                fileContent = fileContent.Replace(String.Format("/*(import:{0})*/", matchFileName), matchContent);
            }

            return fileContent;
        }

        static string PostProcessContent(string fileName, string fileContent)
        {
            if (Regex.Match(fileName, @"(.htm|.css)").Success)
            {
                fileContent = Regex.Replace(fileContent, @"[\r|\n|\r\n|\t]", "");
                fileContent = Regex.Replace(fileContent, @"\s{2,}", ""); 
            }
            return fileContent;
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
