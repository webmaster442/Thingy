using System;

namespace Thingy.IOCore.Internals
{
    internal static class IOInternal
    {
        public const int BufferSize = 8192;

        internal static string FileSize(double value)
        {
            string unit = "Byte";
            if (value > 1099511627776D)
            {
                value /= 1099511627776D;
                unit = "TiB";
            }
            else if (value > 1073741824D)
            {
                value /= 1073741824D;
                unit = "GiB";
            }
            else if (value > 1048576D)
            {
                value /= 1048576D;
                unit = "MiB";
            }
            else if (value > 1024D)
            {
                value /= 1024D;
                unit = "kiB";
            }
            return string.Format("{0:0.###} {1}", value, unit);
        }

        internal static string ProgressString(long totalsize, long copied, DateTime startTime)
        {
            var Diff = DateTime.Now - startTime;
            double progress = (double)copied / totalsize;
            double speed = copied / Diff.TotalSeconds;
            return $"{progress:0.00}%\n{FileSize(speed)}/s";
        }
    }
}
