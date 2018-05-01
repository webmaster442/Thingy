using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Thingy.JobCore.Jobs
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public class CopyFilesToDirectoryJob : AsyncJob
    {
        private readonly IList<string> _files;
        private readonly string _destination;

        public CopyFilesToDirectoryJob(IList<string> files, string destination)
        {
            _files = files;
            _destination = destination;
        }

        public override async Task<bool> Run(CancellationToken token, IProgress<JobProgress> progress)
        {
            long total = 0;
            long copied = 0;
            DateTime startTime = DateTime.Now;
            byte[] buffer = new byte[Internals.BufferSize];
            int read;
            List<string> destinations = new List<string>(_files.Count);

            foreach (var file in _files)
            {
                FileInfo fi = new FileInfo(file);
                total += fi.Length;
                destinations.Add(fi.Name);
            }

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
                            progress.Report(Internals.ReportProgress(total, copied, startTime));
                        }
                        while (read > 0);
                    }
                }
            }
            return true;
        }
    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
