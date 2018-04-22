using System;

namespace Thingy.API.Capabilities
{
    public interface IHaveCloseTask
    {
        Action ClosingTask { get; }
        bool CanExecuteAsync { get; }
    }
}
