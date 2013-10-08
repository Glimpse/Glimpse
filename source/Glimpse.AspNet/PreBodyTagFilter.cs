using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet
{
    public class PreBodyTagFilter : Stream
    {
        private const string BodyClosingTag = "</body>";
        private const string TroubleshootingDocsUri = "http://getglimpse.com/Help/Troubleshooting";

        private readonly ILogger logger;
        private readonly string htmlSnippet;
        private readonly Stream outputStream;
        private readonly Encoding contentEncoding;
        private readonly Regex bodyEndRegex;
        private readonly string currentRequestRawUrl;
        private string unwrittenCharactersFromPreviousCall;

        public PreBodyTagFilter(string htmlSnippet, Stream outputStream, Encoding contentEncoding, string currentRequestRawUrl, ILogger logger)
        {
            this.htmlSnippet = htmlSnippet + BodyClosingTag;
            this.outputStream = outputStream;
            this.contentEncoding = contentEncoding;
            this.bodyEndRegex = new Regex(BodyClosingTag, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            this.currentRequestRawUrl = currentRequestRawUrl ?? "unknown";
            this.logger = logger;
        }

        public override bool CanRead
        {
            get { return this.outputStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.outputStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.outputStream.CanWrite; }
        }

        public override long Length
        {
            get { return this.outputStream.Length; }
        }

        public override long Position
        {
            get { return this.outputStream.Position; }
            set { this.outputStream.Position = value; }
        }

        public override void Close()
        {
            this.outputStream.Close();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.outputStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.outputStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.outputStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
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

            string contentInBuffer = this.contentEncoding.GetString(buffer, offset, count);

            // Prepend remaining characters from the previous call, if any
            if (!string.IsNullOrEmpty(this.unwrittenCharactersFromPreviousCall))
            {
                contentInBuffer = this.unwrittenCharactersFromPreviousCall + contentInBuffer;
                this.unwrittenCharactersFromPreviousCall = null;
            }

            Match closingBodyTagMatch = this.bodyEndRegex.Match(contentInBuffer);
            if (closingBodyTagMatch.Success)
            {
                // Hooray, we found "a" </body> tag, but that doesn't mean that this is "the" last </body> tag we are looking for

                // so we write everything before that match to the output stream
                this.WriteToOutputStream(contentInBuffer.Substring(0, closingBodyTagMatch.Index));

                // and keep the remainder for the next call or the Flush if there is no next call
                this.unwrittenCharactersFromPreviousCall = contentInBuffer.Substring(closingBodyTagMatch.Index);
            }
            else
            {
                // there is no match found for </body> which could have different reasons like case 2.2 for instance
                // therefor we'll write everything except the last 10 characters to the output stream and we'll keep the last 10 characters for the next call or the Flush method
                if (contentInBuffer.Length <= 10)
                {
                    // the content has a maximum length of 10 characters, so we don't need to write anything to the output stream and we'll keep those 
                    // characters for the next call (most likely the Flush)
                    this.unwrittenCharactersFromPreviousCall = contentInBuffer;
                }
                else
                {
                    this.WriteToOutputStream(contentInBuffer.Substring(0, contentInBuffer.Length - 10));
                    this.unwrittenCharactersFromPreviousCall = contentInBuffer.Substring(contentInBuffer.Length - 10);
                }
            }
        }

        public override void Flush()
        {
            if (!string.IsNullOrEmpty(this.unwrittenCharactersFromPreviousCall))
            {
                string finalContentToWrite = this.unwrittenCharactersFromPreviousCall;

                if (this.bodyEndRegex.IsMatch(this.unwrittenCharactersFromPreviousCall))
                {
                    // apparently we did seem to match a </body> tag, which means we can replace the last match with our HTML snippet
                    finalContentToWrite = this.bodyEndRegex.Replace(this.unwrittenCharactersFromPreviousCall, this.htmlSnippet, 1);
                }
                else
                {
                    // there was no </body> tag found, so we write down a warning to the log
                    this.logger.Warn("Unable to locate '</body>' with content encoding '{0}' for request '{1}'. The response may be compressed or the markup may actually be missing a '</body>' tag. See {2} for information on troubleshooting this issue.", this.contentEncoding.EncodingName, this.currentRequestRawUrl, TroubleshootingDocsUri);
                }

                // either way, if a replacement has been done or a warning has been written to the logs, the remaining unwritten characters must be written to the output stream
                this.WriteToOutputStream(finalContentToWrite);
            }

            this.outputStream.Flush();
        }

        private void WriteToOutputStream(string content)
        {
            byte[] outputBuffer = this.contentEncoding.GetBytes(content);
            this.outputStream.Write(outputBuffer, 0, outputBuffer.Length);
        }
    }
}