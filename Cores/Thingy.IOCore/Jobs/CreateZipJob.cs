using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Thingy.JobCore.Jobs
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public class CreateZipJob : AsyncJob
    {
        private readonly IList<string> _sourcefiles;
        private readonly string _zipfile;

        public CreateZipJob(IList<string> sourcefiles, string zipfile)
        {
            _sourcefiles = sourcefiles;
            _zipfile = zipfile;
        }

        public override async Task<bool> Run(CancellationToken token, IProgress<JobProgress> progress)
        {
            var startTime = DateTime.Now;
            long copied = 0;
            long total = 0;
            int read = 0;
            byte[] buffer = new byte[Internals.BufferSize];

            List<string> destinations = new List<string>(_sourcefiles.Count);

            foreach (var file in _sourcefiles)
            {
                FileInfo fi = new FileInfo(file);
                total += fi.Length;
                destinations.Add(fi.Name);
            }

            using (var target = File.Create(_zipfile))
            {
                using (var zip = new ZipArchive(target, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < _sourcefiles.Count; i++)
                    {
                        token.ThrowIfCancellationRequested();
                        using (var source = File.OpenRead(_sourcefiles[i]))
                        {
                            var entry = zip.CreateEntry(destinations[i]);
                            using (var packed = entry.Open())
                            {
                                do
                                {
                                    token.ThrowIfCancellationRequested();
                                    read = source.Read(buffer, 0, buffer.Length);
                                    copied += read;
                                    packed.Write(buffer, 0, read);
                                    progress.Report(Internals.ReportProgress(total, copied, startTime));
                                }
                                while (read > 0);
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
