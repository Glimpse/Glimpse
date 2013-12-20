using System;
using System.Diagnostics;
using System.IO;

namespace Glimpse.Test.MVCLoadTests
{
    public class SeleniumTest : IDisposable
    {
        
        const int iisPort = 2020;
        private string _applicationName;
        private Process _iisProcess;
 
        protected SeleniumTest(string applicationName) 
        {
            _applicationName = applicationName;
            // Start IISExpress
            StartIIS();
        }
 
        public void Dispose() 
        {
            // Ensure IISExpress is stopped
            if (_iisProcess.HasExited == false) {
                _iisProcess.Kill();
            }
        }
 
        private void StartIIS() {
            var applicationPath = GetApplicationPath(_applicationName);
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            //C:\Users\ptaylor.CORP\Documents\GitHub\Glimpse\source\Glimpse.Mvc4.MusicStore.Sample

            _iisProcess = new Process();
            _iisProcess.StartInfo.FileName = programFiles + "\\IIS Express\\iisexpress.exe";
            _iisProcess.StartInfo.Arguments = string.Format("/path:\"{0}\" /port:{1}", applicationPath, iisPort);
            _iisProcess.Start();
        }
 
 
        protected virtual string GetApplicationPath(string applicationName) {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            return Path.Combine(solutionFolder, applicationName);
        }
 
 
        public string GetAbsoluteUrl(string relativeUrl) {
            if (!relativeUrl.StartsWith("/")) {
                relativeUrl = "/" + relativeUrl;
            }
            return String.Format("http://localhost:{0}{1}", iisPort, relativeUrl);
        }
 
 
    }
}
