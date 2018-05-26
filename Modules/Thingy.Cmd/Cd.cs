using System;
using System.Collections.Generic;
using System.Linq;
using Thingy.API;

namespace Thingy.Cmd
{
    public class Cd : BaseCommand
    {
        public override string HelpLocation
        {
            get { return ""; }
        }

        public override string InvokeName
        {
            get { return "cd"; }
        }

        protected override IEnumerable<string> CmdMain(CmdArguments arguments)
        {
            var newpath = arguments.Files.FirstOrDefault();

            if (!string.IsNullOrEmpty(newpath))
            {
                Host.HostDispatcher.Invoke(() =>
                {
                    Host.CurrentDirectory = Environment.ExpandEnvironmentVariables(newpath);
                });
                return null;
            }
            else
            {
                string ret = null;
                Host.HostDispatcher.Invoke(() =>
                {
                    ret = Host.CurrentDirectory;
                });
                return new string[] { ret };
            }
        }
    }
}
