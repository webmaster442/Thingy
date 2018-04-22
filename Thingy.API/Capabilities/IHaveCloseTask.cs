using System;

namespace Thingy.API.Capabilities
{
    public interface IHaveCloseTask
    {
        Action ClosingTask();
        bool CanExecuteAsync { get; }
    }
}
