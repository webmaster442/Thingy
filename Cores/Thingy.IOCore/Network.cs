using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Thingy.IOCore.Internals;

namespace Thingy.IOCore
{
    public static partial class IO
    {
        public static async Task DownloadFile(string sourceUri,
                                              string destination,
                                              IProgress<string> progress,
                                              CancellationToken ct)
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
                using (var tartet = File.Create(destination))
                {
                    ct.ThrowIfCancellationRequested();
                    using (var source = await client.OpenReadTaskAsync(sourceUri))
                    {
                        int read = 0;
                        totalsize = long.Parse(client.ResponseHeaders[HttpRequestHeader.ContentLength]);
                        byte[] buffer = new byte[IOInternal.BufferSize];
                        do
                        {
                            ct.ThrowIfCancellationRequested();
                            read = source.Read(buffer, 0, buffer.Length);
                            tartet.Write(buffer, 0, read);
                            copied += read;
                            progress?.Report(IOInternal.ProgressString(totalsize, copied, startTime));
                        }
                        while (read > 0);
                    }
                }
            }
        }
    }
}