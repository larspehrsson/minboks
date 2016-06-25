using System;
using System.IO;

namespace MinBoks
{
    public static class AppGlobals
    {
        private static readonly object _lkAppLog = new object();

        // writes a message to the log file
        public static void logMessage(string format, params object[] args)
        {
            lock (_lkAppLog)
            {
                try
                {
                    Console.WriteLine(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " " +
                                      string.Format(format, args));
                    var logFile = "eboksreceiver-" + DateTime.UtcNow.ToString("MM") + ".log";
                    using (var fp = new StreamWriter(logFile, true))
                    {
                        fp.WriteLine(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " " + string.Format(format, args));
                        fp.Flush();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("logMessage::Exception: " + ex.Message);
                }
            }
        }
    }
}