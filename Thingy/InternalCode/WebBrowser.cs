namespace Thingy.InternalCode
{
    internal class WebBrowser
    {
        public string Name { get; }
        public string Path { get; }
        public string IconPath { get; }
        public string Version { get; }

        public WebBrowser(string name, string path, string version, string icon)
        {
            Name = name;
            Path = path;
            Version = version;
            IconPath = icon;
        }
    }
}
