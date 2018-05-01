using System;
using System.Threading;
using System.Threading.Tasks;

namespace Thingy.JobCore
{
    public abstract class AsyncJob
    {
        public abstract Task<bool> Run(CancellationToken token, IProgress<JobProgress> progress);
    }
}
