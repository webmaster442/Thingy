namespace Thingy.Cmd.Modules
{
    public class Exit : ICommandModule
    {
        public string HelpFile => "exit.txt";

        public string InvokeName => "exit";

        public void Run(ICommandHost host, Parameters parameters)
        {
            host.Exit();
        }
    }
}
