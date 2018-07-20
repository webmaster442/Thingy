using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thingy.API.Jobs;

namespace Thingy.JobCore.Jobs
{
    public sealed class HashJob: Job<string>, IDisposable
    {
        private readonly IList<string> _files;
        private HashAlgorithm _hashAlgorithm;

        public HashJob(IList<string> files, HashAlgorithm hashAlgorithm)
        {
            _files = files;
            _hashAlgorithm = hashAlgorithm;
        }

        public override string Title
        {
            get { return _hashAlgorithm.GetType().Name; }
        }

        public void Dispose()
        {
            if (_hashAlgorithm != null)
            {
                _hashAlgorithm.Dispose();
                _hashAlgorithm = null;
            }
        }

        public override Task RunJob(CancellationToken token, IProgress<JobProgress> progress)
        {
            return Task.Run(() => Job(token, progress));
        }

        private void Job(CancellationToken token, IProgress<JobProgress> progress)
        {
            long total = 0;
            long hashed = 0;
            DateTime startTime = DateTime.Now;
            byte[] buffer = new byte[BufferSize];
            int read, lastread = 0;
            StringBuilder result = new StringBuilder();

            foreach (var file in _files)
            {
                FileInfo fi = new FileInfo(file);
                total += fi.Length;
            }

            progress.Report(ReportProgress(total, hashed, startTime));

            for (int i = 0; i < _files.Count; i++)
            {
                token.ThrowIfCancellationRequested();
                using (var source = File.OpenRead(_files[i]))
                {
                    do
                    {
                        token.ThrowIfCancellationRequested();
                        read = source.Read(buffer, 0, buffer.Length);
                        if (read == 0)
                        {
                            _hashAlgorithm.TransformFinalBlock(buffer, 0, lastread);
                        }
                        else
                        {
                            _hashAlgorithm.TransformBlock(buffer, 0, read, buffer, 0);
                        }
                        token.ThrowIfCancellationRequested();
                        hashed += read;
                        lastread = read;
                        progress.Report(ReportProgress(total, hashed, startTime));

                        result.AppendLine(FormatHash(_files[i], _hashAlgorithm.Hash));

                    }
                    while (read > 0);
                }
            }

            Result = result.ToString();
        }

        private string FormatHash(string file, byte[] hash)
        {
            return $"{file} *{ByteArrayToString(hash)}";
        }

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
