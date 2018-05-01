using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Thingy.JobCore.Jobs
{
    public class DownloadFileJob : AsyncJob
    {
        private readonly string _sourceuri;
        private readonly string _targetfile;

        public DownloadFileJob(string sourceUri, string targetFile)
        {
            _sourceuri = sourceUri;
            _targetfile = targetFile;
        }

        public override async Task<bool> Run(CancellationToken token, IProgress<JobProgress> progress)
        {
            using (WebClient client = new WebClient())
            {
                IWebProxy defaultProxy = WebRequest.DefaultWebProxy;
                if (defaultProxy != null)
                {
                    defaultProxy.Credentials = CredentialCache.DefaultCredentials;
                    client.Proxy = defaultProxy;
                }

                var startTime = DateTime.Now;
                long copied = 0;
                long totalsize = 0;
                using (var tartet = File.Create(_targetfile))
                {
                    token.ThrowIfCancellationRequested();
                    using (var source = await client.OpenReadTaskAsync(_sourceuri))
                    {
                        int read = 0;
                        totalsize = long.Parse(client.ResponseHeaders[HttpRequestHeader.ContentLength]);
                        byte[] buffer = new byte[Internals.BufferSize];
                        do
                        {
                            token.ThrowIfCancellationRequested();
                            read = source.Read(buffer, 0, buffer.Length);
                            tartet.Write(buffer, 0, read);
                            copied += read;
                            progress.Report(Internals.ReportProgress(totalsize, copied, startTime));
                        }
                        while (read > 0);
                    }
                }
            }

            return true;
        }
    }
}
