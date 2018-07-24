using System;

namespace Thingy.API
{
    /// <summary>
    /// Loging
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Log an exception
        /// </summary>
        /// <param name="ex">Exception to log</param>
        void Exception(Exception ex);
        /// <summary>
        /// Log an error
        /// </summary>
        /// <param name="msg">Message with possible format chars</param>
        /// <param name="additional">Additional parameters to log</param>
        void Error(string msg, params object[] additional);
        /// <summary>
        /// Log a warning
        /// </summary>
        /// <param name="msg">Message with possible format chars</param>
        /// <param name="additional">Additional parameters to log</param>
        void Warning(string msg, params object[] additional);
        /// <summary>
        /// Log an information
        /// </summary>
        /// <param name="msg">Message with possible format chars</param>
        /// <param name="additional">Additional parameters to log</param>
        void Info(string msg, params object[] additional);
        /// <summary>
        /// Write log to file
        /// </summary>
        void WriteToFile();
        /// <summary>
        /// Wtites a divider
        /// </summary>
        void Divider();
        /// <summary>
        /// Writes a big divider
        /// </summary>
        void BigDivider();
    }
}
