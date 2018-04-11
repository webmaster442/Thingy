using System.Threading.Tasks;

namespace Thingy.API.Capabilities
{
    public interface IHaveCloseTask
    {
        Task ClosingTask();
    }
}
