using System;
using System.Threading;
using System.Threading.Tasks;

namespace Thingy.API.Jobs
{
    public abstract class Job
    {
        protected const int BufferSize = 8192;

        protected static string FileSize(double value)
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
            return string.Format("{0:0.##} {1}", value, unit);
        }

        protected static JobProgress ReportProgress(long totalsize, long copied, DateTime startTime)
        {
            var Diff = DateTime.Now - startTime;
            double progress = (double)copied / totalsize;
            double speed = copied / Diff.TotalSeconds;

            if (double.IsNaN(progress)) progress = 0;
            if (double.IsNaN(speed)) speed = 0;

            return new JobProgress
            {
                Progress = progress,
                StatusText = $"{progress:P2}, Average speed: {FileSize(speed)}/s"
            };
        }

        public abstract string Title { get; }

        public abstract Task RunJob(CancellationToken token, IProgress<JobProgress> progress);
    }

    public abstract class Job<T> : Job where T : class, new()
    {
        public abstract T Result { get; set; }
    }
}
