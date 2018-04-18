using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GitBash
{
    public interface IGitBashView: IView
    {
        void SendText(string text);
        bool IsAlive { get; }
        void Close();
    }
}
