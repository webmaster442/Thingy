namespace Thingy.Cmd
{
    public interface ICommandHost
    {
        void Clear();
        void Exit();
        string WorkingDirectory { get; set; }
        void WriteLine(string str);
        void WriteLine(string str, params object[] args);
        void Write(string str);
        void Write(string str, params object[] args);
    }
}
