using System.Threading.Tasks;

namespace Thingy.API.Jobs
{
    public interface IJobRunner
    {
        Task RunJob(Job job);
        Task<T> RunJob<T>(Job<T> job) where T: class, new();
    }
}
