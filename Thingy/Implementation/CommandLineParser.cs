using AppLib.Common.Console;
using System;
using System.Collections.Generic;
using Thingy.API;

namespace Thingy.Infrastructure
{
    public class CommandLineParser
    {
        private IApplication _app;
        private Dictionary<string, Action> _switchActions;
        private Dictionary<string, Action<string>> _argumentedSwitches;

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
            _argumentedSwitches.Add("/module", async (name) =>
            {
                if (!string.IsNullOrEmpty(name))
                {
                    await _app.TabManager.StartModule(name, true);
                }
            });
        }

        public CommandLineParser(IApplication app)
        {
            _app = app;
            _switchActions = new Dictionary<string, Action>();
            _argumentedSwitches = new Dictionary<string, Action<string>>();
            InitActions();
        }
        public void Parse(string args)
        {
            var parser = new ParameterParser(args, true, true);

            foreach (var action in _switchActions)
            {
                if (parser.StandaloneSwitches.Contains(action.Key))
                {
                    action.Value.Invoke();
                }
            }
            foreach (var action in _argumentedSwitches)
            {
                if (parser.SwitchesWithValue.ContainsKey(action.Key))
                {
                    action.Value.Invoke(parser.SwitchesWithValue[action.Key]);
                }
            }

            _app.HandleFiles(parser.Files);
        }
    }
}
