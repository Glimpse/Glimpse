using System;
using System.Diagnostics;

namespace Glimpse.Core.Extensibility
{
    //TODO: Move to another namespace : Glimpse.WebForms.Application?
    public static class GlimpseTrace
    {
        public static void Info(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        public static void Warn(string format, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        public static void Error(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        public static void Fail(string format, params object[] args)
        {
            Fail(string.Format(format, args));
        }

        public static void Info(string message)
        {
            Trace.Write(message, "Info");
        }

        public static void Warn(string message)
        {
            Trace.Write(message, "Warn");
        }

        public static void Error(string message)
        {
            Trace.Write(message, "Error");
        }

        public static void Fail(string message)
        {
            Trace.Write(message, "Fail");
        }

        /*public static IDisposable Time(string format, params object[] args)
        {
            return new GlimpseTimer(format, args);
        }*/
    }

    /*public class GlimpseTimer:IDisposable{

        private string Format { get; set; }
        private object[] Arguments { get; set; }
        private Stopwatch Watch { get; set; }

        internal GlimpseTimer(string format, object[] args)
        {
            Format = format;
            Arguments = args;
            Watch = new Stopwatch();
            Watch.Start();
        }

        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            Watch.Stop();

            if (!Format.Contains(@"{t"))
                Format += " - {t}";

            Format = Format.Replace(@"{t", @"{" + Arguments.Length);

            var message ="";
            message = Arguments.Length == 0 ? string.Format(Format, Watch.Elapsed) : string.Format(Format, Arguments, Watch.Elapsed);

            Trace.Write(message, "Timing");
        }
    }*/
}
