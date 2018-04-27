using System;
using System.IO;

namespace Thingy.CalculatorCore
{
    public class EventRedirectedStreamWriter : StreamWriter
    {
        private void FireEvent(object parameter, bool appendline = false)
        {
            if (!appendline)
                StreamWasWritten?.Invoke(this, parameter.ToString());
            else
                StreamWasWritten?.Invoke(this, $"{parameter}\n");
        }

        private void FireEvent(string format, params object[] pars)
        {
            var str = string.Format(format, pars);
            StreamWasWritten?.Invoke(this, str);
        }

        private void FireEventLine(string format, params object[] pars)
        {
            var str = string.Format(format, pars) + "\n";
            StreamWasWritten?.Invoke(this, str);
        }

        public event EventHandler<string> StreamWasWritten;

        public EventRedirectedStreamWriter(Stream stream) : base(stream)
        {
        }

        public override void Write(bool value)
        {
            FireEvent(value);
        }

        public override void Write(int value)
        {
            FireEvent(value);
        }

        public override void Write(uint value)
        {
            FireEvent(value);
        }

        public override void Write(long value)
        {
            FireEvent(value);
        }

        public override void Write(ulong value)
        {
            FireEvent(value);
        }

        public override void Write(float value)
        {
            FireEvent(value);
        }

        public override void Write(double value)
        {
            FireEvent(value);
        }

        public override void Write(decimal value)
        {
            FireEvent(value);
        }

        public override void Write(string format, object arg0)
        {
            FireEvent(format, arg0);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            FireEvent(format, arg0, arg1);
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            FireEvent(format, arg0, arg1, arg2);
        }

        public override void Write(string format, params object[] arg)
        {
            FireEvent(format, arg);
        }

        public override void Write(char value)
        {
            FireEvent(value);
        }

        public override void Write(char[] buffer)
        {
            var str = new string(buffer);
            FireEvent(str);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            var str = new string(buffer, index, count);
            FireEvent(str);
        }

        public override void Write(string value)
        {
            FireEvent(value);
        }

        public override void WriteLine()
        {
            FireEvent("\n");
        }

        public override void WriteLine(char value)
        {
            FireEvent(value, true);
        }

        public override void WriteLine(char[] buffer)
        {
            var str = new string(buffer);
            FireEvent(str as object, true);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            var str = new string(buffer, index, count);
            FireEvent(str as object, true);
        }

        public override void WriteLine(bool value)
        {
            FireEvent(value, true);
        }

        public override void WriteLine(int value)
        {
            FireEvent(value, true);
        }

        public override void WriteLine(uint value)
        {
            FireEvent(value, true);
        }

        public override void WriteLine(long value)
        {
            FireEvent(value, true);
        }

        public override void WriteLine(ulong value)
        {
            FireEvent(value, true);
        }

        public override void WriteLine(float value)
        {
            FireEvent(value, true);
        }

        public override void WriteLine(double value)
        {
            FireEvent(value, true);
        }

        public override void WriteLine(decimal value)
        {
            FireEvent(value, true);
        }

        public override void WriteLine(string value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(object value)
        {
            FireEvent(value, true);
        }

        public override void WriteLine(string format, object arg0)
        {
            FireEventLine(format, arg0);
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            FireEventLine(format, arg0, arg1);
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            FireEventLine(format, arg0, arg1, arg2);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            FireEventLine(format, arg);
        }
    }
}
