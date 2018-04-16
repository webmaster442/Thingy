using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GitBash
{
    public interface IGitBashView: IView, IDisposable
    {
        void SendText(string text);
    }
}
