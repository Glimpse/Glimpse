using System.IO;
using System.Text.RegularExpressions;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// This class will inject the Glimpse script tags in the resulting HTML output.
    /// It will look for the last occurrence of the &lt;/body&gt; tag and inject the snippet right before that tag.
    /// </summary>
    public class ScriptTagsInjectionStream : Stream
    {
        private const string BodyClosingTag = "</body>";
        private const string TroubleshootingDocsUri = "http://getglimpse.com/Help/Troubleshooting";

        private Stream OutputStream { get; set; }
        private ScriptTagsInjectionOptions Options { get; set; }

        private Regex BodyEndRegex { get; set; }
        private string UnwrittenCharactersFromPreviousCall { get; set; }

        private bool InjectionDone { get; set; }

        public ScriptTagsInjectionStream(Stream outputStream, ScriptTagsInjectionOptions options)
        {
            Guard.ArgumentNotNull(outputStream, "outputStream");
            Guard.ArgumentNotNull(options, "options");

            OutputStream = outputStream;
            Options = options;
            BodyEndRegex = new Regex(BodyClosingTag, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
        }

        public override bool CanRead
        {
            get { return OutputStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return OutputStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return OutputStream.CanWrite; }
        }

        public override long Length
        {
            get { return OutputStream.Length; }
        }

        public override long Position
        {
            get { return OutputStream.Position; }
            set { OutputStream.Position = value; }
        }

        public override void Close()
        {
            if (Options.InjectionRequired && !InjectionDone)
            {
                Options.NotifyInjectionFailure(string.Format(
                    "Unable to locate '</body>' with content encoding '{0}'. The response may be compressed or the markup may actually be missing a '</body>' tag. See {1} for information on troubleshooting this issue.",
                    Options.ContentEncoding.EncodingName,
                    TroubleshootingDocsUri));
            }

            OutputStream.Close();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return OutputStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            OutputStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return OutputStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!Options.InjectionRequired)
            {
                OutputStream.Write(buffer, offset, count);
                return;
            }

            // There are different cases we need to deal with
            // Normally you would expect the contentInBuffer to contain the complete HTML code to return, but this is not always true because it is possible that 
            // the content that will be send back is larger than the buffer foreseen by ASP.NET (currently the buffer seems to be a little bit less than 16K)
            // and in that case this method will be called multiple times, which might result in false positives being written to the logs for not finding a </body> 
            // in the current chunk.

            // So we need to be able to deal with the following cases without writing those false positives
            // 1 - the </body> tag is found
            // 2 - the </body> tag was not found because
            //      2.1 - the </body> tag will be available in one of the next calls because the total length of the output is larger than 16K
            //      2.2 - the </body> tag is split up between this buffer and the next e.g.: "</bo" en "dy>"
            //      2.3 - the </body> tag will never be available (is missing)
            //      2.4 - Multiple </body> tags are available of which some might be part of a Javascript string or the markup is badly formatted

            // The easiest way to deal with this is to look for the last match for the </body> tag and if it is found we write everything before it to the
            // output stream and keep that </body> tag and everything that follows it (normally only a </html> tag but it can also be a 2.4 case) for the next call.
            // In case there is no match for the </body> tag, then we write everything to the output stream except for the last 10 characters (normally the last 6 would suffice, but we take a little margin to reassure us somehow ;-)) which we keep until the next call.

            // If there is a next call, then we first prepend the characters we kept from the previous call to the content inside the buffer (which might complete a chunked </body> tag for instance) 
            // and start our check all over again (which might result in finding a </body> tag or discarding a previously found </body> tag because that one was not the last one.
            // Anyhow, as long as we are not a the end and a </body> tag has been found previously, the output will be buffered, just to make sure there is no other </body> tag further down the stream.

            // If there is no next call, then the Flush method will be called and that one will deal with the current state, which means:
            // - in case there was a </body> tag found, the replacement will be done
            // - in case there was no </body> tag found, then the warning will be written to the log, indicating something went wrong
            // either way, the remaining unwritten characters will be sent down the output stream.
            string contentInBuffer = Options.ContentEncoding.GetString(buffer, offset, count);

            // Prepend remaining characters from the previous call, if any
            if (!string.IsNullOrEmpty(UnwrittenCharactersFromPreviousCall))
            {
                contentInBuffer = UnwrittenCharactersFromPreviousCall + contentInBuffer;
                UnwrittenCharactersFromPreviousCall = null;
            }

            Match closingBodyTagMatch = BodyEndRegex.Match(contentInBuffer);
            if (closingBodyTagMatch.Success)
            {
                // Hooray, we found "a" </body> tag, but that doesn't mean that this is "the" last </body> tag we are looking for

                // so we write everything before that match to the output stream
                WriteToOutputStream(contentInBuffer.Substring(0, closingBodyTagMatch.Index));

                // and keep the remainder for the next call or the Flush if there is no next call
                UnwrittenCharactersFromPreviousCall = contentInBuffer.Substring(closingBodyTagMatch.Index);
            }
            else
            {
                // there is no match found for </body> which could have different reasons like case 2.2 for instance
                // therefor we'll write everything except the last 10 characters to the output stream and we'll keep the last 10 characters for the next call or the Flush method
                if (contentInBuffer.Length <= 10)
                {
                    // the content has a maximum length of 10 characters, so we don't need to write anything to the output stream and we'll keep those 
                    // characters for the next call (most likely the Flush)
                    UnwrittenCharactersFromPreviousCall = contentInBuffer;
                }
                else
                {
                    WriteToOutputStream(contentInBuffer.Substring(0, contentInBuffer.Length - 10));
                    UnwrittenCharactersFromPreviousCall = contentInBuffer.Substring(contentInBuffer.Length - 10);
                }
            }
        }

        public override void Flush()
        {
            if (!Options.InjectionRequired)
            {
                OutputStream.Flush();
                return;
            }

            if (!string.IsNullOrEmpty(UnwrittenCharactersFromPreviousCall))
            {
                string finalContentToWrite = UnwrittenCharactersFromPreviousCall;

                if (BodyEndRegex.IsMatch(UnwrittenCharactersFromPreviousCall))
                {
                    // apparently we did seem to match a </body> tag, which means we can replace the last match with our HTML snippet
                    finalContentToWrite = BodyEndRegex.Replace(UnwrittenCharactersFromPreviousCall, Options.GetScriptTagsToInject() + BodyClosingTag, 1);
                    InjectionDone = true;
                }

                // either way, if a replacement has been done or a warning has been written to the logs, the remaining unwritten characters must be written to the output stream
                WriteToOutputStream(finalContentToWrite);
                UnwrittenCharactersFromPreviousCall = null;
            }

            OutputStream.Flush();
        }

        private void WriteToOutputStream(string content)
        {
            byte[] outputBuffer = Options.ContentEncoding.GetBytes(content);
            OutputStream.Write(outputBuffer, 0, outputBuffer.Length);
        }
    }
}