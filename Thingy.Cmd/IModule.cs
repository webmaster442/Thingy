using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.Cmd
{
    internal interface IModule
    {
        string HelpFile { get; }
        string InvokeName { get; }
        void Run(ICmdHost host, Parameters parameters);
    }
}
