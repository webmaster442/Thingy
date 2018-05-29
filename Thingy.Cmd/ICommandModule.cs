namespace Thingy.Cmd
{
    internal interface ICommandModule
    {
        string HelpFile { get; }
        string InvokeName { get; }
        void Run(ICommandHost host, Parameters parameters);
    }
}
