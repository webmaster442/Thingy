using AppLib.Common.Console;
using System;
using System.Collections.Generic;

namespace Thingy.Infrastructure
{
    public class CommandLineParser
    {
        private IApplication _app;
        private IModuleLoader _modules;
        private Dictionary<string, Action> _switchActions;

        public CommandLineParser(IApplication app, IModuleLoader moduleLoader)
        {
            _app = app;
            _modules = moduleLoader;
            _switchActions = new Dictionary<string, Action>();
            InitActions();
        }

        private void InitActions()
        {
            _switchActions.Add("/exit", () =>
            {
                _app.Close();
            });
            _switchActions.Add("/restart", () =>
            {
                _app.Restart();
            });
        }

        public void Parse(string args)
        {
            var parser = new ParameterParser(args, true);

            foreach (var action in _switchActions)
            {
                if (parser.StandaloneSwitches.Contains(action.Key))
                {
                    action.Value.Invoke();
                }
            }
        }
    }
}
