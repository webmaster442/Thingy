using AppLib.MVVM;
using CmdHost;

namespace Thingy.Views
{
    public interface ICommandLineView: ICloseableView<ViewModels.CommandLineViewModel>, ITerminalBoxProvider
    {
    }
}
