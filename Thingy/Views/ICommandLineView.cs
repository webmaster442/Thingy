using AppLib.MVVM;
using CmdHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.Views
{
    public interface ICommandLineView: ICloseableView, ITerminalBoxProvider
    {
    }
}
