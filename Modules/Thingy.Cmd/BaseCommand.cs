using System.Collections.Generic;
using System.Threading.Tasks;
using Thingy.API;

namespace Thingy.Cmd
{
    public abstract class BaseCommand : ICmdModule
    {
        public abstract string HelpLocation { get; }

        public abstract string InvokeName { get; }

        public ICmdHost Host { get; set; }

        public Task<IEnumerable<string>> Run(CmdArguments arguments)
        {
            return Task.Run(() => CmdMain(arguments));
        }

        protected abstract IEnumerable<string> CmdMain(CmdArguments arguments);
    }
}
