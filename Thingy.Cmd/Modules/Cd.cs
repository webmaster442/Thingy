using System;
using System.IO;

namespace Thingy.Cmd.Modules
{
    public class Cd : ICommandModule
    {
        public string HelpFile => "cd.txt";

        public string InvokeName => "cd";

        public void Run(ICommandHost host, Parameters parameters)
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
