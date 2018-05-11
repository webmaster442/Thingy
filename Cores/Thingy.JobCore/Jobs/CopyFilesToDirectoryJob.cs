using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Thingy.API.Jobs;

namespace Thingy.JobCore.Jobs
{
    public class CopyFilesToDirectoryJob : Job
    {
        private readonly IList<string> _files;
        private readonly string _destination;

        public override string Title
        {
            get { return "Coping files..."; }
        }

        public CopyFilesToDirectoryJob(IList<string> files, string destination)
        {
            _files = files;
            _destination = destination;
        }

        private void Job(CancellationToken token, IProgress<JobProgress> progress)
        {
            long total = 0;
            long copied = 0;
            DateTime startTime = DateTime.Now;
            byte[] buffer = new byte[BufferSize];
            int read;
            List<string> destinations = new List<string>(_files.Count);

            foreach (var file in _files)
            {
                FileInfo fi = new FileInfo(file);
                total += fi.Length;
                destinations.Add(Path.Combine(_destination, fi.Name));
            }

            progress.Report(ReportProgress(total, copied, startTime));
            for (int i = 0; i < _files.Count; i++)
            {
                token.ThrowIfCancellationRequested();

                using (var source = File.OpenRead(_files[i]))
                {
                    using (var target = File.Create(destinations[i]))
                    {
                        do
                        {
                            token.ThrowIfCancellationRequested();
                            read = source.Read(buffer, 0, buffer.Length);
                            token.ThrowIfCancellationRequested();
                            copied += read;
                            target.Write(buffer, 0, read);
                            progress.Report(ReportProgress(total, copied, startTime));
                        }
                        while (read > 0);
                    }
                }
            }
        }

        public override Task RunJob(CancellationToken token, IProgress<JobProgress> progress)
        {
            return Task.Run(() => Job(token, progress));
        }
    }
}
