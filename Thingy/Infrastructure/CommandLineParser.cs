using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLib.Common.Console;

namespace Thingy.Infrastructure
{
    public class CommandLineParser
    {
        private IApplication _app;
        private IModuleLoader _modules;

        public CommandLineParser(IApplication app, IModuleLoader moduleLoader)
        {
            _app = app;
            _modules = moduleLoader;
        }

        public void Parse(string args)
        {
            var parser = new ParametersParser(args, false);
        }
    }
}
