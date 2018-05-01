using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Thingy.API;

namespace Thingy.Implementation
{
    internal class Log: ILog
    {
        private StringBuilder _buffer;
        private string _logfile;

        private void CheckBufferClean()
        {
            if (_buffer.Length > 2048 * 3)
                _buffer.Clear();
        }

        private void Write(string category, string msg, params object[] additional)
        {
            string line = $"{DateTime.Now} {category}: {string.Format(msg, additional)}\r\n";
            _buffer.Append(line);
            Debug.Write(line);
            if (_buffer.Length > 2048)
            {
                WriteToFile();
            }
        }

        public Log(string filename)
        {
            _buffer = new StringBuilder();
            _logfile = filename;
        }
        public void BigDivider()
        {
            var line = "\r\n===================================================================================\r\n";
            _buffer.AppendLine(line);
            Debug.Write(line);
        }

        public void Divider()
        {
            var line = "--------------------------------------------------------------------------------------";
            _buffer.AppendLine(line);
            Debug.Write(line);
        }

        public void Error(Exception ex)
        {
            Divider();
            Write("exception", "\r\nmessage: {0}\r\nsource: {1}\r\nstacktrace: {2}", ex.Message, ex.Source, ex.StackTrace);
            Divider();
        }

        public void Error(string msg, params object[] additional)
        {
            Write("error", msg, additional);
        }

        public void Info(string msg, params object[] additional)
        {
            Write("info", msg, additional);
        }

        public override string ToString()
        {
            return _buffer.ToString();
        }

        public void Warning(string msg, params object[] additional)
        {
            Write("warning", msg, additional);
        }

        public void WriteToFile()
        {
            try
            {
                using (var file = File.AppendText(_logfile))
                {
                    file.Write(_buffer.ToString());
                    CheckBufferClean();
                }
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
    }
}
