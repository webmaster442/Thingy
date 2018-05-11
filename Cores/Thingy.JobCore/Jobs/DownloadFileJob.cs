using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Thingy.API.Jobs;

namespace Thingy.JobCore.Jobs
{
    public class DownloadFileJob : Job
    {
        private readonly string _sourceuri;
        private readonly string _targetfile;

        public DownloadFileJob(string sourceUri, string targetFile)
        {
            _sourceuri = sourceUri;
            _targetfile = targetFile;
        }

        public override string Title
        {
            get { return "Downloading File..."; }
        }

        public override Task RunJob(CancellationToken token, IProgress<JobProgress> progress)
        {
            return Task.Run(() => Job(progress, token));
        }

        private void Job(IProgress<JobProgress> progress, CancellationToken token)
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
                    progress.Report(ReportProgress(totalsize, copied, startTime));
                    using (var source = client.OpenRead(_sourceuri))
                    {
                        int read = 0;
                        var size = client.ResponseHeaders.GetValues("Content-Length").FirstOrDefault();
                        totalsize = long.Parse(size);
                        byte[] buffer = new byte[BufferSize];
                        do
                        {
                            token.ThrowIfCancellationRequested();
                            read = source.Read(buffer, 0, buffer.Length);
                            tartet.Write(buffer, 0, read);
                            copied += read;
                            progress.Report(ReportProgress(totalsize, copied, startTime));
                        }
                        while (read > 0);
                    }
                }
            }
        }
    }
}
