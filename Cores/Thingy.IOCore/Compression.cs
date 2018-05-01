using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Thingy.IOCore.Internals;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
namespace Thingy.IOCore
{
    public static partial class IO
    {
        public static async Task CreateZip(IList<string> sourcefiles,
                                           string zipfile,
                                           IProgress<string> progress,
                                           CancellationToken ct)
        {
            var startTime = DateTime.Now;
            long copied = 0;
            long total = 0;
            int read = 0;
            byte[] buffer = new byte[IOInternal.BufferSize];

            List<string> destinations = new List<string>(sourcefiles.Count);

            foreach (var file in sourcefiles)
            {
                FileInfo fi = new FileInfo(file);
                total += fi.Length;
                destinations.Add(fi.Name);
            }

            using (var target = File.Create(zipfile))
            {
                using (var zip = new ZipArchive(target, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < sourcefiles.Count; i++)
                    {
                        ct.ThrowIfCancellationRequested();
                        using (var source = File.OpenRead(sourcefiles[i]))
                        {
                            var entry = zip.CreateEntry(destinations[i]);
                            using (var packed = entry.Open())
                            {
                                do
                                {
                                    ct.ThrowIfCancellationRequested();
                                    read = source.Read(buffer, 0, buffer.Length);
                                    copied += read;
                                    packed.Write(buffer, 0, read);
                                    progress?.Report(IOInternal.ProgressString(total, copied, startTime));
                                }
                                while (read > 0);
                            }
                        }
                    }
                }
            }
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously