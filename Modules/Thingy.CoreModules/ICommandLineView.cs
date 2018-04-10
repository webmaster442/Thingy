using AppLib.MVVM;
using CmdHost;

namespace Thingy.CoreModules
{
    public interface ICommandLineView: ICloseableView<ViewModels.CommandLineViewModel>, ITerminalBoxProvider
    {
    }
}
