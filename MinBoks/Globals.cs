using System;
using System.Diagnostics;
using System.IO;

namespace MinBoks
{
    public static class AppGlobals
    {
        private static object _lkAppLog = new object();

        // writes a message to console
        public static void writeConsole(string format, params object[] args)
        {
            try
            {
                Debug.WriteLine(string.Format(format, args));
                logMessage(format, args);
                Console.Out.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.ffff") + " " + String.Format(format, args));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("writeConsole::Exception: " + ex.Message);
            }
        }

        // writes a message to the log file
        public static void logMessage(string format, params object[] args)
        {
            lock (_lkAppLog)
            {
                try
                {
                    Debug.WriteLine(string.Format(format, args));
                    string logFile = "eboksreceiver-" + DateTime.UtcNow.ToString("MM") + ".log";
                    StreamWriter fp = new StreamWriter(logFile, true);
                    fp.WriteLine(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " " + string.Format(format, args));
                    fp.Flush();
                    fp.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("logMessage::Exception: " + ex.Message);
                }
            }
        }
    }
}