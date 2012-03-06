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
            var assetsTest = new Assets(args[0] + "\\FunctionalTests\\Mock");

            var coreContent = File.ReadAllText(assets.BuildPath("glimpse.core.js"));
            coreContent = ProcessFile(coreContent, assets);

            var testContent = File.ReadAllText(assetsTest.BuildPath("test.glimpse.ajax.js"));
            testContent = ProcessFile(testContent, assetsTest);

            File.WriteAllText(assets.BuildPath("glimpse.js"), coreContent);
            File.WriteAllText(assetsTest.BuildPath("glimpseTest.js"), testContent);
        }

        static string ProcessFile(string fileContent, Assets assets)
        {
            var matches = Regex.Matches(fileContent, @"\/\*\(import:\S*\)\*\/", RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                var matchIdentifier = match.Value.Substring(10, match.Value.Length - 13);
                var matchFileName = matchIdentifier;
                var tabIndex = 0;
                if (matchFileName.IndexOf("|") > -1) 
                {
                    tabIndex = Convert.ToInt32(matchFileName.Substring(matchFileName.IndexOf("|") + 1, matchFileName.Length - matchFileName.IndexOf("|") - 1));
                    matchFileName = matchFileName.Substring(0, matchFileName.IndexOf("|"));
                }
                var tabs = "";
                for (var i = 0; i < tabIndex; i++)
                    tabs += "    ";
                var matchContent = File.ReadAllText(assets.BuildPath(matchFileName));
                matchContent = ProcessFile(PostProcessContent(matchFileName, matchContent), assets);
                if (tabIndex > 0)
                    matchContent = tabs + Regex.Replace(matchContent, "\n", "\n" + tabs); 

                fileContent = fileContent.Replace(String.Format("/*(import:{0})*/", matchIdentifier), matchContent);
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
         
        public string BuildPath(string relativeFilePath)
        {
            return Path.Combine(BasePath, relativeFilePath);
        }
    }
}
