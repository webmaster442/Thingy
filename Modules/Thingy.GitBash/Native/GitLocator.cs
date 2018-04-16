using System;
using System.IO;

namespace Thingy.GitBash.Native
{
    internal static class GitLocator
    {
        public static string GitPath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(@"%systemdrive%\Program Files\Git\usr\bin\mintty.exe");
            }
        }

        public static bool IsGitInstalled
        {
            get { return File.Exists(GitPath); }
        }
    }
}
