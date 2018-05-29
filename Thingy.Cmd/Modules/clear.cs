namespace Thingy.Cmd.Modules
{
    public class Clear : ICommandModule
    {
        public string HelpFile => "clear.txt";

        public string InvokeName => "clear";

        public void Run(ICommandHost host, Parameters parameters)
        {
            host.Clear();
        }
    }
}
