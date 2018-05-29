using System;
using System.IO;

namespace Thingy.Cmd.Modules
{
    internal class Cd : IModule
    {
        public string HelpFile
        {
            get { return "cd.txt"; }
        }

        public string InvokeName
        {
            get { return "cd"; }
        }

        public void Run(ICmdHost host, Parameters parameters)
        {
            if (parameters.Files.Count > 0)
            {
                var argument = Environment.ExpandEnvironmentVariables(parameters.Files[0]);
                if (!Directory.Exists(argument))
                {
                    host.WriteLine(Properties.Resources.PATH_NOTEXIST);
                }
                else
                {
                    host.WorkingDirectory = argument;
                }
            }
        }
    }
}
