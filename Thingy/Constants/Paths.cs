using System;

namespace Thingy
{
    public static class Paths
    {
        public const string ConfigPath = @"%userprofile%\thingy.json";
        public const string LogPath = @"%appdir%\thingy.log";
        public const string DBPath = @"%userprofile%\thingy.litedb";

        public static string Resolve(string path)
        {
            if (path.Contains("%appdir%"))
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("%appdir%", dir);
            }

            return Environment.ExpandEnvironmentVariables(path);
        }
    }
}
